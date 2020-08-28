using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Abstractions.Specialized;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	/// <summary>
	/// Don't call "ToInstantiatePerContainer" to avoid the possibility of mocking.
	/// </summary>
	public static class ManagerExtensions
	{
		public static IManager AddExtensionDependencyProvider( this IManager manager, IServiceCollection services )
		{
			// No need to add the manager as an extension dependency provider (of type "IExtensionDependencyProvider"), since
			// extension dependencies can only be retrieved through an instance of "IExtensionDependencyProvider" (= itself).

			services.AddSingleton<IExtensionDependencyProvider>( manager );

			return manager;
		}

		public static IManager AddDependencyInjectionProxy( this IManager manager, IServiceCollection services )
		{
			var extensionDependencyProxy = new DependencyInjectionProxy( manager );

			manager.AddExtensionDependency<IDependencyInjectionProxy>( extensionDependencyProxy );

			services.AddSingleton<IDependencyInjectionProxy>( extensionDependencyProxy );

			return manager;
		}

		public static IManager AddSpecializedProxy( this IManager manager, IServiceCollection services )
		{
			var extensionDependencyProxy = new SpecializedProxy( manager );

			manager.AddExtensionDependency<ISpecializedProxy>( extensionDependencyProxy );

			services.AddSingleton<ISpecializedProxy>( extensionDependencyProxy );

			return manager;
		}

		public static IManager AddKeyedOptionsProvider( this IManager manager, IConfiguration optionsProvider )
		{
			return manager.AddExtensionDependency<IKeyedOptionsProvider>( new KeyedOptionsProvider( optionsProvider ) );
		}

		public static IManager AddKeyedOptionsProvider( this IManager manager, IServiceCollection services )
		{
			var keyedOptionsProvider = manager.GetExtensionDependency<IKeyedOptionsProvider>();

			services.AddSingleton<IKeyedOptionsProvider>( keyedOptionsProvider );

			return manager;
		}

		public static IManager AddInstantiationRegistry( this IManager manager, IServiceCollection services,
			bool instantiatePerContainerInsteadOfScope, IAutomatedMocker? automatedMocker )
		{
			var instantiationRegistry = new InstantiationRegistry( manager, services, instantiatePerContainerInsteadOfScope,
				automatedMocker );

			manager.AddExtensionDependency<IInstantiationRegistry>( instantiationRegistry );

			services.AddSingleton<IInstantiationRegistry>( instantiationRegistry );

			services.AddSingleton<IGlobalInstanceProvider>( serviceProvider => new GlobalInstanceProvider( serviceProvider ) );

			services.AddSingleton<IInstanceProvider>(
				serviceProvider => serviceProvider.GetRequiredService<IGlobalInstanceProvider>() );

			return manager;
		}

		public static IManager AddMultiInstantiationRegistry( this IManager manager, IServiceCollection services )
		{
			var multiInstantiationRegistry = new MultiInstantiationRegistry();

			manager.AddExtensionDependency<IMultiInstantiationRegistry>( multiInstantiationRegistry );

			services.AddSingleton<IMultiInstantiationRegistry>( multiInstantiationRegistry );

			return manager;
		}

		public static IManager AddSpecializedRegistry( this IManager manager, IServiceCollection services )
		{
			var instantiationRegistry = new SpecializedRegistry( services );

			manager.AddExtensionDependency<ISpecializedRegistry>( instantiationRegistry );

			services.AddSingleton<ISpecializedRegistry>( instantiationRegistry );

			return manager;
		}

		public static IManager AddRegistries( this IManager manager, IServiceCollection services,
			bool instantiatePerContainerInsteadOfScope, IAutomatedMocker? automatedMocker )
		{
			return manager
				.AddInstantiationRegistry( services, instantiatePerContainerInsteadOfScope, automatedMocker )
				.AddMultiInstantiationRegistry( services )
				.AddSpecializedRegistry( services );
		}

		public static IManager AddDependencies( this IManager manager, IServiceCollection services,
			bool instantiatePerContainerInsteadOfScope = false, IAutomatedMocker? automatedMocker = null )
		{
			return manager
				.AddExtensionDependencyProvider( services )
				.AddDependencyInjectionProxy( services )
				.AddSpecializedProxy( services )
				.AddKeyedOptionsProvider( services )
				.AddRegistries( services, instantiatePerContainerInsteadOfScope, automatedMocker );
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

		public static IInstantiationRegistry ActivateManualMocking( this IManager manager )
		{
			var instantiationRegistry = manager.GetExtensionDependency<IInstantiationRegistry>();

			return instantiationRegistry.ActivateManualMocking();
		}

		public static IInstantiationRegistry WithManualMocking( this IManager manager, ManualMocker manualMocker )
		{
			var instantiationRegistry = manager.GetExtensionDependency<IInstantiationRegistry>();

			return instantiationRegistry.WithManualMocking( manualMocker );
		}

		[Obsolete( "Use 'AddDependencies' instead" )]
		public static IManager AddInstantiationRegistry( this IManager manager, IServiceCollection services,
			bool instantiatePerContainerInsteadOfScope, bool addGlobalInstanceProvider )
		{
			return manager.AddDependencies( services, instantiatePerContainerInsteadOfScope, null );
		}

		[Obsolete( "Use 'AddInstantiationRegistry' instead" )]
		public static IManager AddDependencyInjectionInstantiationRegistry( this IManager manager, IServiceCollection services,
			bool instantiatePerContainerInsteadOfScope, IAutomatedMocker? automatedMocker )
		{
			return AddInstantiationRegistry( manager, services, instantiatePerContainerInsteadOfScope, automatedMocker );
		}

		[Obsolete( "Use 'AddSpecializedRegistry' instead" )]
		public static IManager AddSpecializedInstantiationRegistry( this IManager manager, IServiceCollection services )
		{
			return AddSpecializedRegistry( manager, services );
		}

		[Obsolete( "Use 'AddDependencies' instead" )]
		public static IManager AddInstantiationRegistries( this IManager manager, IServiceCollection services,
				bool allowMultipleImplementationsPerBaseType = false, bool instantiatePerContainerInsteadOfScope = false,
				IAutomatedMocker? automatedMocker = null, bool addGlobalInstanceProvider = true )
		{
			return manager.AddDependencies( services, instantiatePerContainerInsteadOfScope, automatedMocker );
		}
	}
}
