using System;

namespace Automated.Arca.Attributes.Cqrs
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class MessageBusSubscribeForExchangePublicationQueueBetweenAttribute : MessageBusSubscribeAttribute
	{
		public string SourceBoundedContext { get; protected set; }

		public MessageBusSubscribeForExchangePublicationQueueBetweenAttribute( string sourceBoundedContext,
				string targetBoundedContext, Type[] messageTypes )
			: base( targetBoundedContext, messageTypes )
		{
			SourceBoundedContext = sourceBoundedContext;
		}
	}
}
