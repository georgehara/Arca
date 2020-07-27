using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IInstantiationRegistry : IExtensionDependency
	{
		void ToInstantiatePerContainer( Type type );
		void ToInstantiatePerContainer( Type baseType, Type implementationType );
		void ToInstantiatePerContainer( Type baseType, Func<IServiceProvider, object> implementationFactory );
		void ToInstantiatePerScope( Type type );
		void ToInstantiatePerScope( Type baseType, Type implementationType );
		void ToInstantiatePerScope( Type baseType, Func<IServiceProvider, object> implementationFactory );
		void ToInstantiatePerInjection( Type type );
		void ToInstantiatePerInjection( Type baseType, Type implementationType );
		void ToInstantiatePerInjection( Type baseType, Func<IServiceProvider, object> implementationFactory );

		void AddInstancePerContainer( Type baseType, object implementationInstance );
		void AddInstancePerContainer<T>( T instance ) where T : notnull;

		void AddGlobalInstanceProvider();

		void AddExternalService( Type serviceInterfaceType, Type serviceImplementationType, string baseAddress, int retryCount,
			int retryDelayMilliseconds, int circuitBreakerEventCount, int circuitBreakerDurationSeconds );
		void AddHostedService( Type hostedServiceType );
		void AddDatabaseContext( Type dbContextType, string connectionString, string migrationsHistoryTable,
			string migrationsHistoryTableSchema, int retryCount, int retryDelaySeconds );
	}
}
