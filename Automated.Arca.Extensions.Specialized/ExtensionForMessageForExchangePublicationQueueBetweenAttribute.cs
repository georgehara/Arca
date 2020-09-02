using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForMessageForExchangePublicationQueueBetweenAttribute :
		ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( MessageForExchangePublicationQueueBetweenAttribute );

		public ExtensionForMessageForExchangePublicationQueueBetweenAttribute(
				IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (MessageForExchangePublicationQueueBetweenAttribute)attribute;

			var messageBus = D.P.GetRequiredInstance<IMessageBus>();
			var messageListenerType = attributeTyped.MessageListenerType;

			var sourceBoundedContext = attributeTyped.SourceBoundedContext;
			var targetBoundedContext = attributeTyped.TargetBoundedContext;
			var exchangeName = Exchange.PublicationsFor( sourceBoundedContext );
			var queueName = Queue.Between( sourceBoundedContext, targetBoundedContext );

			GenericsHelper.RegisterMessageToMessageBus( messageBus, typeWithAttribute, messageListenerType, exchangeName, queueName );
		}
	}
}
