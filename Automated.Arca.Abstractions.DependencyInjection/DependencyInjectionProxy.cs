using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public class DependencyInjectionProxy : IDependencyInjectionProxy
	{
		public IExtensionDependencyProvider ExtensionDependencyProvider { get; }

		public object X( Type type ) => ExtensionDependencyProvider.GetExtensionDependency( type );
		public T X<T>() => ExtensionDependencyProvider.GetExtensionDependency<T>();

		public IKeyedOptionsProvider O => X<IKeyedOptionsProvider>();
		public IInstantiationRegistry R => X<IInstantiationRegistry>();
		public IMultiInstantiationRegistry M => X<IMultiInstantiationRegistry>();
		public IInstanceProvider P => X<IInstanceProvider>();
		public IGlobalInstanceProvider GP => X<IGlobalInstanceProvider>();

		public DependencyInjectionProxy( IExtensionDependencyProvider extensionDependencyProvider )
		{
			ExtensionDependencyProvider = extensionDependencyProvider;
		}

		public IScopedInstanceProvider<TScopeName> SP<TScopeManager, TScopeName>( TScopeName scopeName )
			where TScopeManager : IScopeManager<TScopeName>
			where TScopeName : notnull
		{
			return GP.GetRequiredInstance<TScopeManager>().GetOrAdd( scopeName );
		}

		public object MI( IInstanceProvider instanceProvider, Type interfaceType, string implementationKey )
		{
			var implementationType = M.GetRequiredImplementationType( interfaceType, implementationKey );

			return instanceProvider.GetRequiredInstance( implementationType );
		}

		public object MI( Type interfaceType, string implementationKey )
		{
			return MI( P, interfaceType, implementationKey );
		}

		public T MI<T>( IInstanceProvider instanceProvider, string implementationKey )
		{
			return (T)MI( instanceProvider, typeof( T ), implementationKey );
		}

		public T MI<T>( string implementationKey )
		{
			return MI<T>( P, implementationKey );
		}
	}
}
