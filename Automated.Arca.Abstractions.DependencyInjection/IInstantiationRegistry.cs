using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IInstantiationRegistry : IExtensionDependency
	{
		void InstantiatePerContainer( Type type, bool overrideExisting );
		void InstantiatePerContainer( Type baseType, Type implementationType, bool overrideExisting );
		void InstantiatePerContainer( Type baseType, Func<IServiceProvider, object> implementationFactory, bool overrideExisting );
		void InstantiatePerScope( Type type, bool overrideExisting );
		void InstantiatePerScope( Type baseType, Type implementationType, bool overrideExisting );
		void InstantiatePerScope( Type baseType, Func<IServiceProvider, object> implementationFactory, bool overrideExisting );
		void InstantiatePerInjection( Type type, bool overrideExisting );
		void InstantiatePerInjection( Type baseType, Type implementationType, bool overrideExisting );
		void InstantiatePerInjection( Type baseType, Func<IServiceProvider, object> implementationFactory, bool overrideExisting );

		void AddInstancePerContainer( Type baseType, object implementationInstance, bool overrideExisting );
		void AddInstancePerContainer<T>( T instance, bool overrideExisting ) where T : notnull;

		IInstantiationRegistry ActivateManualMocking();
		IInstantiationRegistry WithManualMocking( ManualMocker manualMocker );
	}
}
