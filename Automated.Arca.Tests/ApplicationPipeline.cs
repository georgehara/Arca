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

		private readonly IConfiguration ApplicationOptionsProvider;
		private readonly IServiceCollection Services = new ServiceCollection();
		private readonly IManager Manager;
		private readonly IServiceProvider ServiceProvider;
		private readonly IApplicationBuilder ApplicationBuilder;

		public ApplicationPipeline( Assembly rootAssembly, bool processOnlyTypesDerivedFromIProcessable,
			ICollection<Type>? excludeTypes, IList<Type>? priorityTypes, bool instantiatePerContainerInsteadOfScope,
			Action<ApplicationPipeline> onCreateManager, Action<ApplicationPipeline> onManagerRegister,
			Action<ApplicationPipeline> onManagerConfigure )
		{
			InstantiatePerContainerInsteadOfScope = instantiatePerContainerInsteadOfScope;

			ApplicationOptionsProvider = GetApplicationOptionsProvider();

			Services = new ServiceCollection();

			Manager = GetManager( rootAssembly, processOnlyTypesDerivedFromIProcessable, excludeTypes, priorityTypes,
				ApplicationOptionsProvider );

			onCreateManager( this );
			onManagerRegister( this );

			ServiceProvider = BuildServiceProvider( Services );

			ApplicationBuilder = new ApplicationBuilder( ServiceProvider );

			Manager.AddMiddlewareRegistry( ApplicationBuilder );

			onManagerConfigure( this );
		}

		public static ApplicationPipeline GetInstanceAndCallRegisterAndConfigure( Assembly rootAssembly,
			bool processOnlyTypesDerivedFromIProcessable, ICollection<Type>? excludeTypes, IList<Type>? priorityTypes,
			bool instantiatePerContainerInsteadOfScope )
		{
			return new ApplicationPipeline( rootAssembly, processOnlyTypesDerivedFromIProcessable,
				excludeTypes, priorityTypes, instantiatePerContainerInsteadOfScope, x => { },
				x => x.RegisterFirst(), x => x.ConfigureFirst() );
		}

		public ApplicationPipeline AddAssemblyContainingType<T>()
		{
			Manager.AddAssemblyContainingType<T>();

			return this;
		}

		public ApplicationPipeline RegisterFirst()
		{
			Manager
				.AddInstantiationRegistry( Services, InstantiatePerContainerInsteadOfScope, true )
				.Register();

			return this;
		}

		public ApplicationPipeline RegisterSecond()
		{
			Manager
				.Register();

			return this;
		}

		public ApplicationPipeline ConfigureFirst()
		{
			Manager
				.AddGlobalInstanceProvider( ServiceProvider )
				.Configure();

			return this;
		}

		public ApplicationPipeline ConfigureSecond()
		{
			Manager
				.Configure();

			return this;
		}

		public IScopedInstanceProvider<string> GetOrAddScopedProvider( string scopeName )
		{
			var scopeManager = Manager.GetExtensionDependency<IGlobalInstanceProvider>().GetRequiredInstance<ITenantManager>();

			return scopeManager.GetOrAdd( scopeName );
		}

		public T GetRequiredInstance<T>()
		{
			return Manager.GetExtensionDependency<IGlobalInstanceProvider>().GetRequiredInstance<T>();
		}

		public IEnumerable<Type> GetPriorityTypes()
		{
			return Manager.GetPriorityTypes();
		}

		private IConfiguration GetApplicationOptionsProvider()
		{
			return new ConfigurationBuilder()
				.SetBasePath( Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) )
				.AddJsonFile( "appsettings.json", optional: false, reloadOnChange: false )
				.AddEnvironmentVariables()
				.Build();
		}

		private IManager GetManager( Assembly rootAssembly, bool processOnlyTypesDerivedFromIProcessable,
			ICollection<Type>? excludeTypes, IList<Type>? priorityTypes, IConfiguration applicationOptionsProvider )
		{
			var managerOptions = new ManagerOptions()
				.UseLogger( new TraceLogger() );

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

			return new Manager.Manager( managerOptions )
				.AddAssembly( rootAssembly )
				.AddAssemblyContainingType( typeof( ExtensionForInstantiatePerScopeAttribute ) )
				.AddAssemblyContainingType( typeof( ExtensionForBoundedContextAttribute ) )
				.AddKeyedOptionsProvider( applicationOptionsProvider );
		}

		private IServiceProvider BuildServiceProvider( IServiceCollection services )
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
