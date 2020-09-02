using System;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class MessageForExchangePublicationQueueBetweenAttribute : MessageAttribute
	{
		public string SourceBoundedContext { get; protected set; }

		public MessageForExchangePublicationQueueBetweenAttribute( string sourceBoundedContext,
				string targetBoundedContext, Type messageListenerType )
			: base( targetBoundedContext, messageListenerType )
		{
			SourceBoundedContext = sourceBoundedContext;
		}
	}
}
