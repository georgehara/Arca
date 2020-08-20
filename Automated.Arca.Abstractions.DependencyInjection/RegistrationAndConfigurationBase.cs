using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public abstract class RegistrationAndConfigurationBase
	{
		protected object GetExtensionDependency( IProcessingContext context, Type type )
		{
			return context.GetExtensionDependency( type );
		}

		protected T GetExtensionDependency<T>( IProcessingContext context )
		{
			return context.GetExtensionDependency<T>();
		}

		protected object GetRequiredInstance( IConfigurationContext context, Type type )
		{
			var globalProvider = GetExtensionDependency<IInstanceProvider>( context );

			return globalProvider.GetRequiredInstance( type );
		}

		protected T GetRequiredInstance<T>( IConfigurationContext context )
		{
			var globalProvider = GetExtensionDependency<IInstanceProvider>( context );

			return globalProvider.GetRequiredInstance<T>();
		}

		protected object GetGlobalRequiredInstance( IConfigurationContext context, Type type )
		{
			var globalProvider = GetExtensionDependency<IGlobalInstanceProvider>( context );

			return globalProvider.GetRequiredInstance( type );
		}

		protected T GetGlobalRequiredInstance<T>( IConfigurationContext context )
		{
			var globalProvider = GetExtensionDependency<IGlobalInstanceProvider>( context );

			return globalProvider.GetRequiredInstance<T>();
		}

		protected TInstance GetScopedRequiredInstance<TScopeManager, TScopeName, TInstance>( IConfigurationContext context
				, TScopeName scopeName )
			where TScopeManager : IScopeManager<TScopeName>
			where TScopeName : notnull
		{
			var globalProvider = GetExtensionDependency<IGlobalInstanceProvider>( context );
			var scopeManager = globalProvider.GetRequiredInstance<TScopeManager>();
			var scopedProvider = scopeManager.GetOrAdd( scopeName );

			return scopedProvider.GetRequiredInstance<TInstance>();
		}

		protected IKeyedOptionsProvider Options( IProcessingContext context )
		{
			return GetExtensionDependency<IKeyedOptionsProvider>( context );
		}

		protected IInstantiationRegistry Registry( IRegistrationContext context )
		{
			return GetExtensionDependency<IInstantiationRegistry>( context );
		}

		protected void ToInstantiatePerContainer( IRegistrationContext context, Type type, bool overrideExisting = false )
		{
			Registry( context ).ToInstantiatePerContainer( type, overrideExisting );
		}

		protected void ToInstantiatePerContainer( IRegistrationContext context, Type baseType, Type implementationType,
			bool overrideExisting = false )
		{
			Registry( context ).ToInstantiatePerContainer( baseType, implementationType, overrideExisting );
		}

		protected void ToInstantiatePerContainer( IRegistrationContext context, Type baseType,
			Func<IServiceProvider, object> implementationFactory, bool overrideExisting = false )
		{
			Registry( context ).ToInstantiatePerContainer( baseType, implementationFactory, overrideExisting );
		}

		protected void ToInstantiatePerScope( IRegistrationContext context, Type type, bool overrideExisting = false )
		{
			Registry( context ).ToInstantiatePerScope( type, overrideExisting );
		}

		protected void ToInstantiatePerScope( IRegistrationContext context, Type baseType, Type implementationType,
			bool overrideExisting = false )
		{
			Registry( context ).ToInstantiatePerScope( baseType, implementationType, overrideExisting );
		}

		protected void ToInstantiatePerScope( IRegistrationContext context, Type baseType,
			Func<IServiceProvider, object> implementationFactory, bool overrideExisting = false )
		{
			Registry( context ).ToInstantiatePerScope( baseType, implementationFactory, overrideExisting );
		}

		protected void ToInstantiatePerInjection( IRegistrationContext context, Type type, bool overrideExisting = false )
		{
			Registry( context ).ToInstantiatePerInjection( type, overrideExisting );
		}

		protected void ToInstantiatePerInjection( IRegistrationContext context, Type baseType, Type implementationType,
			bool overrideExisting = false )
		{
			Registry( context ).ToInstantiatePerInjection( baseType, implementationType, overrideExisting );
		}

		protected void ToInstantiatePerInjection( IRegistrationContext context, Type baseType,
			Func<IServiceProvider, object> implementationFactory, bool overrideExisting = false )
		{
			Registry( context ).ToInstantiatePerInjection( baseType, implementationFactory, overrideExisting );
		}

		protected IGlobalInstanceProvider Provider( IConfigurationContext context )
		{
			return GetExtensionDependency<IGlobalInstanceProvider>( context );
		}

		protected TScopedProvider Provider<TScopedProvider, TScopeName>( IConfigurationContext context )
				where TScopedProvider : IScopedInstanceProvider<TScopeName>
		{
			return GetExtensionDependency<TScopedProvider>( context );
		}
	}
}
