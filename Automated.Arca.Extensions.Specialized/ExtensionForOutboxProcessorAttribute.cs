using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForOutboxProcessorAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( OutboxProcessorAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var interfaceType = ((ProcessableWithInterfaceAttribute)attribute).GetInterfaceOrDefault( typeWithAttribute );

			ToInstantiatePerContainer( context, interfaceType, typeWithAttribute );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var key = ((OutboxProcessorAttribute)attribute).ScheduleConfigurationKey;
			var scheduleConfiguration = Options( context ).GetRequiredString( key );
			var outboxProcessor = Provider( context ).GetRequiredInstance<IOutboxProcessor>();

			outboxProcessor.Schedule( scheduleConfiguration );
		}
	}
}
