using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForBoundedContextAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( BoundedContextAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (BoundedContextAttribute)attribute;

			var connectionStringConfigurationKey = attributeTyped.ConnectionStringConfigurationKey;
			var connectionString = Options( context ).GetRequiredString( connectionStringConfigurationKey );
			var migrationsHistoryTable = attributeTyped.MigrationsHistoryTable;
			var migrationsHistoryTableSchema = attributeTyped.MigrationsHistoryTableSchema;
			var retryCount = attributeTyped.RetryCount;
			var retryDelaySeconds = attributeTyped.RetryDelaySeconds;

			Registry( context ).AddDatabaseContext( typeWithAttribute, connectionString, migrationsHistoryTable,
				migrationsHistoryTableSchema, retryCount, retryDelaySeconds );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
