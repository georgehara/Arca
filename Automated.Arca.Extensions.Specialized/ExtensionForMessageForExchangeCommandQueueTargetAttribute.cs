using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForMessageForExchangeCommandQueueTargetAttribute : ExtensionForSpecializedAttribute
	{
		public override Type AttributeType => typeof( MessageForExchangeCommandQueueTargetAttribute );

		public ExtensionForMessageForExchangeCommandQueueTargetAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var attributeTyped = (MessageForExchangeCommandQueueTargetAttribute)attribute;

			var messageBus = D.P.GetRequiredInstance<IMessageBus>();
			var messageListenerType = attributeTyped.MessageListenerType;

			var targetBoundedContext = attributeTyped.TargetBoundedContext;
			var exchangeName = Exchange.CommandsFor( targetBoundedContext );
			var queueName = Queue.For( targetBoundedContext );

			GenericsHelper.RegisterMessageToMessageBus( messageBus, typeWithAttribute, messageListenerType, exchangeName, queueName );
		}
	}
}
