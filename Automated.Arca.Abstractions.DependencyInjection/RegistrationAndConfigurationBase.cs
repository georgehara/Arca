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

		protected void ToInstantiatePerContainer( IRegistrationContext context, Type type )
		{
			Registry( context ).ToInstantiatePerContainer( type );
		}

		protected void ToInstantiatePerContainer( IRegistrationContext context, Type baseType, Type implementationType )
		{
			Registry( context ).ToInstantiatePerContainer( baseType, implementationType );
		}

		protected void ToInstantiatePerScope( IRegistrationContext context, Type type )
		{
			Registry( context ).ToInstantiatePerScope( type );
		}

		protected void ToInstantiatePerScope( IRegistrationContext context, Type baseType, Type implementationType )
		{
			Registry( context ).ToInstantiatePerScope( baseType, implementationType );
		}

		protected void ToInstantiatePerInjection( IRegistrationContext context, Type type )
		{
			Registry( context ).ToInstantiatePerInjection( type );
		}

		protected void ToInstantiatePerInjection( IRegistrationContext context, Type baseType, Type implementationType )
		{
			Registry( context ).ToInstantiatePerInjection( baseType, implementationType );
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
