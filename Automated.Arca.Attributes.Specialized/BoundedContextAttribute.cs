using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public class BoundedContextAttribute : ProcessableAttribute
	{
		public string ConnectionStringConfigurationKey { get; protected set; }
		public string MigrationsHistoryTable { get; protected set; }
		public string MigrationsHistoryTableSchema { get; protected set; }
		public int RetryCount { get; protected set; }
		public int RetryDelaySeconds { get; protected set; }

		public BoundedContextAttribute( string connectionStringConfigurationKey, string migrationsHistoryTable,
			string migrationsHistoryTableSchema, int retryCount = 3, int retryDelaySeconds = 30 )
		{
			ConnectionStringConfigurationKey = connectionStringConfigurationKey;
			MigrationsHistoryTable = migrationsHistoryTable;
			MigrationsHistoryTableSchema = migrationsHistoryTableSchema;
			RetryCount = retryCount;
			RetryDelaySeconds = retryDelaySeconds;
		}
	}
}
