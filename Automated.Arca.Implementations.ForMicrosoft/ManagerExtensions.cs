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
			IAutomatedMocker? automatedMocker, bool addGlobalInstanceProvider )
		{
			var instantiationRegistry = new DependencyInjectionInstantiationRegistry( manager, services,
				allowMultipleImplementationsPerBaseType, instantiatePerContainerInsteadOfScope, automatedMocker );

			manager.AddExtensionDependency<Abstractions.DependencyInjection.IInstantiationRegistry>( instantiationRegistry );

			if( addGlobalInstanceProvider )
				manager.AddGlobalInstanceProvider( services );

			return manager;
		}

		public static IManager AddSpecializedInstantiationRegistry( this IManager manager, IServiceCollection services )
		{
			var instantiationRegistry = new SpecializedInstantiationRegistry( services );

			return manager.AddExtensionDependency<Abstractions.Specialized.IInstantiationRegistry>( instantiationRegistry );
		}

		public static IManager AddInstantiationRegistries( this IManager manager, IServiceCollection services,
			bool allowMultipleImplementationsPerBaseType = false, bool instantiatePerContainerInsteadOfScope = false,
			IAutomatedMocker? automatedMocker = null, bool addGlobalInstanceProvider = true )
		{
			return manager
				.AddDependencyInjectionInstantiationRegistry( services, allowMultipleImplementationsPerBaseType,
					instantiatePerContainerInsteadOfScope, automatedMocker, addGlobalInstanceProvider )
				.AddSpecializedInstantiationRegistry( services );
		}

		[Obsolete( "Use 'AddInstantiationRegistries' instead" )]
		public static IManager AddInstantiationRegistry( this IManager manager, IServiceCollection services,
		bool instantiatePerContainerInsteadOfScope, bool addGlobalInstanceProvider )
		{
			return manager.AddInstantiationRegistries( services, false, instantiatePerContainerInsteadOfScope, null,
				addGlobalInstanceProvider );
		}

		public static IManager AddGlobalInstanceProvider( this IManager manager, IServiceCollection services )
		{
			// Don't call "ToInstantiatePerContainer" to avoid the possibility of mocking.

			services.AddSingleton( typeof( IGlobalInstanceProvider ),
				serviceProvider => new GlobalInstanceProvider( serviceProvider ) );

			services.AddSingleton( typeof( IInstanceProvider ),
				serviceProvider => serviceProvider.GetRequiredService<IGlobalInstanceProvider>() );

			return manager;
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

		public static Abstractions.DependencyInjection.IInstantiationRegistry ActivateManualMocking( this IManager manager )
		{
			var instantiationRegistry = manager.GetExtensionDependency<Abstractions.DependencyInjection.IInstantiationRegistry>();

			return instantiationRegistry.ActivateManualMocking();
		}

		public static Abstractions.DependencyInjection.IInstantiationRegistry WithManualMocking( this IManager manager,
			ManualMocker manualMocker )
		{
			var instantiationRegistry = manager.GetExtensionDependency<Abstractions.DependencyInjection.IInstantiationRegistry>();

			return instantiationRegistry.WithManualMocking( manualMocker );
		}
	}
}
