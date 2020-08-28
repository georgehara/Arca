using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public interface ISpecializedRegistry : IExtensionDependency
	{
		void AddExternalService( Type serviceInterfaceType, Type serviceImplementationType, string baseAddress, int retryCount,
			int retryDelayMilliseconds, int circuitBreakerEventCount, int circuitBreakerDurationSeconds );
		void AddHostedService( Type hostedServiceType );
		void AddDatabaseContext( Type dbContextType, string connectionString, string migrationsHistoryTable,
			string migrationsHistoryTableSchema, int retryCount, int retryDelaySeconds );
	}
}
