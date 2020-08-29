using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForOutboxProcessorAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( OutboxProcessorAttribute );
		public override Type? BaseInterfaceOfTypeWithAttribute => typeof( IOutboxProcessor );

		public ExtensionForOutboxProcessorAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (ProcessableWithInterfaceAttribute)attribute;
			var interfaceType = attributeTyped.GetInterfaceOrDefault( typeWithAttribute );

			D.R.InstantiatePerContainer( interfaceType, typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var key = ((OutboxProcessorAttribute)attribute).ScheduleConfigurationKey;
			var scheduleConfiguration = D.O.GetRequiredString( key );
			var outboxProcessor = D.P.GetRequiredInstance<IOutboxProcessor>();

			outboxProcessor.Schedule( scheduleConfiguration );
		}
	}
}
