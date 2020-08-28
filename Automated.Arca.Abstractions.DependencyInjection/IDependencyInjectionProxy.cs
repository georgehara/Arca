using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IDependencyInjectionProxy : IExtensionDependency
	{
		IKeyedOptionsProvider O { get; }
		IInstantiationRegistry R { get; }
		IMultiInstantiationRegistry M { get; }
		IInstanceProvider P { get; }
		IGlobalInstanceProvider GP { get; }

		IScopedInstanceProvider<TScopeName> SP<TScopeManager, TScopeName>( TScopeName scopeName )
			where TScopeManager : IScopeManager<TScopeName>
			where TScopeName : notnull;

		object MI( IInstanceProvider instanceProvider, Type interfaceType, string implementationKey );
		object MI( Type interfaceType, string implementationKey );
		T MI<T>( IInstanceProvider instanceProvider, string implementationKey );
		T MI<T>( string implementationKey );
	}
}
