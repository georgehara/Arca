using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Extensions.DependencyInjection;
using Automated.Arca.Extensions.Specialized;
using Automated.Arca.Implementations.ForMicrosoft;
using Automated.Arca.Manager;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Tests
{
	public class ApplicationPipeline
	{
		private readonly bool InstantiatePerContainerInsteadOfScope;
		private readonly AutomatedMocker? AutomatedMocker;

		private readonly IConfiguration ApplicationOptionsProvider;
		private readonly IServiceCollection Services = new ServiceCollection();
		private readonly IManager Manager;
		private readonly IServiceProvider ServiceProvider;
		private readonly IApplicationBuilder ApplicationBuilder;

		public IManagerStatistics Statistics => Manager.Statistics;
		public IDependencyInjectionProxy D => Manager.GetExtensionDependency<IDependencyInjectionProxy>();
		public IScopedInstanceProvider<string> SP( string scopeName ) => D.SP<ITenantManager, string>( scopeName );

		public ApplicationPipeline( Action<IManagerOptions> onCreateManagerOptions, bool useLogging,
			bool processOnlyTypesDerivedFromIProcessable, ICollection<Type>? excludeTypes, IList<Type>? priorityTypes,
			bool simulateOnlyUnprocessableTypes, bool instantiatePerContainerInsteadOfScope, AutomatedMocker? automatedMocker,
			Assembly rootAssembly, Action<IManager> onCreateManager, Action<ApplicationPipeline> onManagerRegister,
			Action<ApplicationPipeline> onManagerConfigure )
		{
			InstantiatePerContainerInsteadOfScope = instantiatePerContainerInsteadOfScope;
			AutomatedMocker = automatedMocker;

			ApplicationOptionsProvider = GetApplicationOptionsProvider();

			Services = new ServiceCollection();

			var managerOptions = GetManagerOptions( onCreateManagerOptions, useLogging, processOnlyTypesDerivedFromIProcessable,
				excludeTypes, priorityTypes );

			Manager = GetManager( managerOptions, simulateOnlyUnprocessableTypes, ApplicationOptionsProvider, rootAssembly );

			onCreateManager( Manager );
			onManagerRegister( this );

			ServiceProvider = BuildServiceProvider( Services );

			ApplicationBuilder = new ApplicationBuilder( ServiceProvider );

			onManagerConfigure( this );
		}

		public static ApplicationPipeline GetInstanceAndCallRegisterAndConfigure( bool processOnlyTypesDerivedFromIProcessable,
			ICollection<Type>? excludeTypes, IList<Type>? priorityTypes, bool instantiatePerContainerInsteadOfScope,
			AutomatedMocker? automatedMocker, Assembly rootAssembly )
		{
			return new ApplicationPipeline( x => { }, true, processOnlyTypesDerivedFromIProcessable, excludeTypes, priorityTypes,
				false, instantiatePerContainerInsteadOfScope, automatedMocker, rootAssembly, x => { }, x => x.RegisterFirst(),
				x => x.ConfigureFirst() );
		}

		public IEnumerable<Type> GetPriorityTypes()
		{
			return Manager.GetPriorityTypes();
		}

		public ApplicationPipeline AddAssemblyContainingType<T>()
		{
			Manager.AddAssemblyContainingType<T>();

			return this;
		}

		public ApplicationPipeline AddAssembliesLoadedInProcess()
		{
			Manager.AddAssembliesLoadedInProcess();

			return this;
		}

		public ApplicationPipeline RegisterFirst()
		{
			Manager
				.AddDependencies( Services, InstantiatePerContainerInsteadOfScope, AutomatedMocker )
				.Register();

			return this;
		}

		public ApplicationPipeline RegisterSecond()
		{
			Manager
				.Register();

			return this;
		}

		public IInstantiationRegistry WithManualMocking( ManualMocker manualMocker )
		{
			return Manager.WithManualMocking( manualMocker );
		}

		public ApplicationPipeline ConfigureFirst()
		{
			Manager
				.AddGlobalInstanceProvider( ServiceProvider )
				.AddMiddlewareRegistry( ApplicationBuilder )
				.Configure();

			return this;
		}

		public ApplicationPipeline ConfigureSecond()
		{
			Manager
				.Configure();

			return this;
		}

		private static IConfiguration GetApplicationOptionsProvider()
		{
			return new ConfigurationBuilder()
				.SetBasePath( Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location )! )
				.AddJsonFile( "appsettings.json", optional: false, reloadOnChange: false )
				.AddEnvironmentVariables()
				.Build();
		}

		private static IManager GetManager( IManagerOptions managerOptions, bool simulateOnlyUnprocessableTypes,
			IConfiguration applicationOptionsProvider, Assembly rootAssembly )
		{
			return new Manager.Manager( managerOptions, simulateOnlyUnprocessableTypes )
				.AddAssembly( rootAssembly )
				.AddAssemblyContainingType<ExtensionForInstantiatePerScopeAttribute>()
				.AddAssemblyContainingType<ExtensionForBoundedContextAttribute>()
				.AddKeyedOptionsProvider( applicationOptionsProvider );
		}

#pragma warning disable CA1859 // Use concrete types when possible for improved performance
		private static IManagerOptions GetManagerOptions( Action<IManagerOptions> onCreateManagerOptions, bool useLogging,
#pragma warning restore CA1859 // Use concrete types when possible for improved performance
			bool processOnlyTypesDerivedFromIProcessable, ICollection<Type>? excludeTypes, IList<Type>? priorityTypes )
		{
			var managerOptions = new ManagerOptions();

			onCreateManagerOptions( managerOptions );

			if( useLogging )
				managerOptions.UseLogger( new TraceLogger() );

			if( processOnlyTypesDerivedFromIProcessable )
				managerOptions.UseOnlyClassesDerivedFromIProcessable();

			if( excludeTypes != null )
			{
				foreach( var type in excludeTypes )
					managerOptions.Exclude( type );
			}

			if( priorityTypes != null )
			{
				foreach( var type in priorityTypes )
					managerOptions.Prioritize( type );
			}

			return managerOptions;
		}

#pragma warning disable CA1859 // Use concrete types when possible for improved performance
		private static IServiceProvider BuildServiceProvider( IServiceCollection services )
#pragma warning restore CA1859 // Use concrete types when possible for improved performance
		{
			var serviceProviderOptions = new ServiceProviderOptions
			{
				ValidateOnBuild = true,
				ValidateScopes = true
			};

			return services.BuildServiceProvider( serviceProviderOptions );
		}
	}
}
