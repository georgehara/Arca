using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForMessageBusSubscribeForExchangePublicationQueueBetweenAttribute :
		ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( MessageBusSubscribeForExchangePublicationQueueBetweenAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (MessageBusSubscribeForExchangePublicationQueueBetweenAttribute)attribute;

			var messageBus = Provider( context ).GetRequiredInstance<IMessageBus>();
			var messageTypes = attributeTyped.MessageTypes;

			var sourceBoundedContext = attributeTyped.SourceBoundedContext;
			var targetBoundedContext = attributeTyped.TargetBoundedContext;
			var exchangeName = Exchange.PublicationsFor( sourceBoundedContext );
			var queueName = Queue.Between( sourceBoundedContext, targetBoundedContext );

			GenericsHelper.SubscribeToMessageBus( messageBus, messageTypes, typeWithAttribute, exchangeName, queueName );
		}
	}
}
