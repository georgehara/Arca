using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Extensions.Cqrs;
using Automated.Arca.Extensions.DependencyInjection;
using Automated.Arca.Implementations.ForMicrosoft;
using Automated.Arca.Manager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Tests
{
	public class ManagerTooling
	{
		private readonly bool InstantiatePerContainerInsteadOfScope;

		private readonly IConfiguration ApplicationOptionsProvider;
		private readonly IManager Manager;
		private readonly IServiceCollection Services = new ServiceCollection();
		private IServiceProvider? ServiceProvider;

		public ManagerTooling( Assembly rootAssembly, bool processOnlyTypesDerivedFromIProcessable,
			ICollection<Type>? excludeTypes, bool instantiatePerContainerInsteadOfScope )
		{
			InstantiatePerContainerInsteadOfScope = instantiatePerContainerInsteadOfScope;

			ApplicationOptionsProvider = GetApplicationOptionsProvider();
			Manager = GetManager( rootAssembly, processOnlyTypesDerivedFromIProcessable, excludeTypes );

			Services = new ServiceCollection();
		}

		public static ManagerTooling GetInstanceAndCallRegisterAndConfigure( Assembly rootAssembly,
			bool processOnlyTypesDerivedFromIProcessable, ICollection<Type>? excludeTypes,
			bool instantiatePerContainerInsteadOfScope )
		{
			return new ManagerTooling( rootAssembly, processOnlyTypesDerivedFromIProcessable,
					excludeTypes, instantiatePerContainerInsteadOfScope )
				.Register()
				.Configure();
		}

		public ManagerTooling AddAssemblyContainingType<T>()
		{
			Manager.AddAssemblyContainingType<T>();

			return this;
		}

		public ManagerTooling Register()
		{
			Manager
				.AddInstantiationRegistry( Services, InstantiatePerContainerInsteadOfScope, true )
				.Register();

			return this;
		}

		public ManagerTooling Configure()
		{
			Manager
				.AddGlobalInstanceProvider( BuildServiceProvider() )
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

		private IConfiguration GetApplicationOptionsProvider()
		{
			return new ConfigurationBuilder()
				.SetBasePath( Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) )
				.AddJsonFile( "appsettings.json", optional: false, reloadOnChange: false )
				.AddEnvironmentVariables()
				.Build();
		}

		private IManager GetManager( Assembly rootAssembly, bool processOnlyTypesDerivedFromIProcessable,
			ICollection<Type>? excludeTypes )
		{
			var managerOptions = new ManagerOptions()
				.UseLogger( new TraceLogger() );

			if( processOnlyTypesDerivedFromIProcessable )
				managerOptions.UseOnlyClassesDerivedFromIProcessable();

			if( excludeTypes != null )
			{
				foreach( var excludeType in excludeTypes )
					managerOptions.Exclude( excludeType );
			}

			return new Manager.Manager( managerOptions )
				.AddAssembly( rootAssembly )
				.AddAssemblyContainingType( typeof( ExtensionForInstantiatePerScopeAttribute ) )
				.AddAssemblyContainingType( typeof( ExtensionForBoundedContextAttribute ) )
				.AddKeyedOptionsProvider( ApplicationOptionsProvider );
		}

		private IServiceProvider BuildServiceProvider()
		{
			if( ServiceProvider != null )
				return ServiceProvider;

			var serviceProviderOptions = new ServiceProviderOptions
			{
				ValidateOnBuild = true,
				ValidateScopes = true
			};

			ServiceProvider = Services.BuildServiceProvider( serviceProviderOptions );

			return ServiceProvider;
		}
	}
}
