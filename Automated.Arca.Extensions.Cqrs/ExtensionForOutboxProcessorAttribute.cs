using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Cqrs;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.Cqrs;

namespace Automated.Arca.Extensions.Cqrs
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
