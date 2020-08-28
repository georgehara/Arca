using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForMessageBusSubscribeForExchangeCommandQueueTargetAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( MessageBusSubscribeForExchangeCommandQueueTargetAttribute );

		public ExtensionForMessageBusSubscribeForExchangeCommandQueueTargetAttribute(
				IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (MessageBusSubscribeForExchangePublicationQueueBetweenAttribute)attribute;

			var messageBus = D.P.GetRequiredInstance<IMessageBus>();
			var messageTypes = attributeTyped.MessageTypes;

			var targetBoundedContext = attributeTyped.TargetBoundedContext;
			var exchangeName = Exchange.CommandsFor( targetBoundedContext );
			var queueName = Queue.For( targetBoundedContext );

			GenericsHelper.SubscribeToMessageBus( messageBus, messageTypes, typeWithAttribute, exchangeName, queueName );
		}
	}
}
