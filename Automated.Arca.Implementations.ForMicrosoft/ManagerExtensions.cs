using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Abstractions.Specialized;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public static class ManagerExtensions
	{
		public static IManager AddKeyedOptionsProvider( this IManager manager, IConfiguration optionsProvider )
		{
			return manager.AddExtensionDependency<IKeyedOptionsProvider>( new KeyedOptionsProvider( optionsProvider ) );
		}

		public static IManager AddDependencyInjectionInstantiationRegistry( this IManager manager, IServiceCollection services,
			bool allowMultipleImplementationsPerBaseType, bool instantiatePerContainerInsteadOfScope,
			AutomatedMockingProvider? automatedMockingProvider, bool addGlobalInstanceProvider )
		{
			var instantiationRegistry = new DependencyInjectionInstantiationRegistry( manager, services,
				allowMultipleImplementationsPerBaseType, instantiatePerContainerInsteadOfScope, automatedMockingProvider,
				addGlobalInstanceProvider );

			return manager.AddExtensionDependency<Abstractions.DependencyInjection.IInstantiationRegistry>( instantiationRegistry );
		}

		public static IManager AddSpecializedInstantiationRegistry( this IManager manager, IServiceCollection services )
		{
			var instantiationRegistry = new SpecializedInstantiationRegistry( services );

			return manager.AddExtensionDependency<Abstractions.Specialized.IInstantiationRegistry>( instantiationRegistry );
		}

		public static IManager AddInstantiationRegistries( this IManager manager, IServiceCollection services,
			bool allowMultipleImplementationsPerBaseType, bool instantiatePerContainerInsteadOfScope,
			AutomatedMockingProvider? automatedMockingProvider, bool addGlobalInstanceProvider )
		{
			return manager
				.AddDependencyInjectionInstantiationRegistry( services, allowMultipleImplementationsPerBaseType,
					instantiatePerContainerInsteadOfScope, automatedMockingProvider, addGlobalInstanceProvider )
				.AddSpecializedInstantiationRegistry( services );
		}

		[Obsolete( "Use 'AddInstantiationRegistries' instead" )]
		public static IManager AddInstantiationRegistry( this IManager manager, IServiceCollection services,
			bool instantiatePerContainerInsteadOfScope, bool addGlobalInstanceProvider )
		{
			return manager.AddInstantiationRegistries( services, false, instantiatePerContainerInsteadOfScope, null,
				addGlobalInstanceProvider );
		}

		public static IManager AddGlobalInstanceProvider( this IManager manager, IServiceProvider serviceProvider )
		{
			var globalInstanceProvider = serviceProvider.GetRequiredService<IGlobalInstanceProvider>();

			return manager
				.AddExtensionDependency<IGlobalInstanceProvider>( globalInstanceProvider )
				.AddExtensionDependency<IInstanceProvider>( globalInstanceProvider );
		}

		public static IManager AddMiddlewareRegistry( this IManager manager, IApplicationBuilder applicationBuilder )
		{
			var middlewareRegistry = new MiddlewareRegistry( applicationBuilder );

			return manager.AddExtensionDependency<IMiddlewareRegistry>( middlewareRegistry );
		}

		public static IManager ActivateManualMocking( this IManager manager, ManualMockingRegistrator manualMockingRegistrator )
		{
			var instantiationRegistry = manager.GetExtensionDependency<Abstractions.DependencyInjection.IInstantiationRegistry>();

			instantiationRegistry.ActivateManualMocking();

			manualMockingRegistrator( instantiationRegistry );

			return manager;
		}
	}
}
