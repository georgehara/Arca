using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForBoundedContextAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( BoundedContextAttribute );

		public ExtensionForBoundedContextAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (BoundedContextAttribute)attribute;

			var connectionStringConfigurationKey = attributeTyped.ConnectionStringConfigurationKey;
			var connectionString = D.O.GetRequiredString( connectionStringConfigurationKey );
			var migrationsHistoryTable = attributeTyped.MigrationsHistoryTable;
			var migrationsHistoryTableSchema = attributeTyped.MigrationsHistoryTableSchema;
			var retryCount = attributeTyped.RetryCount;
			var retryDelaySeconds = attributeTyped.RetryDelaySeconds;

			S.R.AddDatabaseContext( typeWithAttribute, connectionString, migrationsHistoryTable, migrationsHistoryTableSchema,
				retryCount, retryDelaySeconds );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}
	}
}
