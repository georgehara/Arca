using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IInstantiationRegistry : IExtensionDependency
	{
		void ToInstantiatePerContainer( Type type, bool overrideExisting );
		void ToInstantiatePerContainer( Type baseType, Type implementationType, bool overrideExisting );
		void ToInstantiatePerContainer( Type baseType, Func<IServiceProvider, object> implementationFactory, bool overrideExisting );
		void ToInstantiatePerScope( Type type, bool overrideExisting );
		void ToInstantiatePerScope( Type baseType, Type implementationType, bool overrideExisting );
		void ToInstantiatePerScope( Type baseType, Func<IServiceProvider, object> implementationFactory, bool overrideExisting );
		void ToInstantiatePerInjection( Type type, bool overrideExisting );
		void ToInstantiatePerInjection( Type baseType, Type implementationType, bool overrideExisting );
		void ToInstantiatePerInjection( Type baseType, Func<IServiceProvider, object> implementationFactory, bool overrideExisting );

		void AddInstancePerContainer( Type baseType, object implementationInstance, bool overrideExisting );
		void AddInstancePerContainer<T>( T instance, bool overrideExisting ) where T : notnull;

		IInstantiationRegistry ActivateManualMocking();
		IInstantiationRegistry WithManualMocking( ManualMocker manualMocker );
	}
}
