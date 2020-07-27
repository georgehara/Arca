using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public abstract class RegistrationAndConfigurationBase
	{
		public object GetExtensionDependency( IProcessingContext context, Type type )
		{
			return context.GetExtensionDependency( type );
		}

		public T GetExtensionDependency<T>( IProcessingContext context )
		{
			return context.GetExtensionDependency<T>();
		}

		public object GetRequiredInstance( IConfigurationContext context, Type type )
		{
			var globalProvider = GetExtensionDependency<IInstanceProvider>( context );

			return globalProvider.GetRequiredInstance( type );
		}

		public T GetRequiredInstance<T>( IConfigurationContext context )
		{
			var globalProvider = GetExtensionDependency<IInstanceProvider>( context );

			return globalProvider.GetRequiredInstance<T>();
		}

		public object GetGlobalRequiredInstance( IConfigurationContext context, Type type )
		{
			var globalProvider = GetExtensionDependency<IGlobalInstanceProvider>( context );

			return globalProvider.GetRequiredInstance( type );
		}

		public T GetGlobalRequiredInstance<T>( IConfigurationContext context )
		{
			var globalProvider = GetExtensionDependency<IGlobalInstanceProvider>( context );

			return globalProvider.GetRequiredInstance<T>();
		}

		public TInstance GetScopedRequiredInstance<TScopeManager, TScopeName, TInstance>( IConfigurationContext context
				, TScopeName scopeName )
			where TScopeManager : IScopeManager<TScopeName>
			where TScopeName : notnull
		{
			var globalProvider = GetExtensionDependency<IGlobalInstanceProvider>( context );
			var scopeManager = globalProvider.GetRequiredInstance<TScopeManager>();
			var scopedProvider = scopeManager.GetOrAdd( scopeName );

			return scopedProvider.GetRequiredInstance<TInstance>();
		}

		public IKeyedOptionsProvider Options( IProcessingContext context )
		{
			return GetExtensionDependency<IKeyedOptionsProvider>( context );
		}

		public IInstantiationRegistry Registry( IRegistrationContext context )
		{
			return GetExtensionDependency<IInstantiationRegistry>( context );
		}

		public void ToInstantiatePerContainer( IRegistrationContext context, Type type )
		{
			Registry( context ).ToInstantiatePerContainer( type );
		}

		public void ToInstantiatePerContainer( IRegistrationContext context, Type baseType, Type implementationType )
		{
			Registry( context ).ToInstantiatePerContainer( baseType, implementationType );
		}

		public void ToInstantiatePerScope( IRegistrationContext context, Type type )
		{
			Registry( context ).ToInstantiatePerScope( type );
		}

		public void ToInstantiatePerScope( IRegistrationContext context, Type baseType, Type implementationType )
		{
			Registry( context ).ToInstantiatePerScope( baseType, implementationType );
		}

		public void ToInstantiatePerInjection( IRegistrationContext context, Type type )
		{
			Registry( context ).ToInstantiatePerInjection( type );
		}

		public void ToInstantiatePerInjection( IRegistrationContext context, Type baseType, Type implementationType )
		{
			Registry( context ).ToInstantiatePerInjection( baseType, implementationType );
		}

		public IGlobalInstanceProvider Provider( IConfigurationContext context )
		{
			return GetExtensionDependency<IGlobalInstanceProvider>( context );
		}

		public TScopedProvider Provider<TScopedProvider, TScopeName>( IConfigurationContext context )
				where TScopedProvider : IScopedInstanceProvider<TScopeName>
		{
			return GetExtensionDependency<TScopedProvider>( context );
		}
	}
}
