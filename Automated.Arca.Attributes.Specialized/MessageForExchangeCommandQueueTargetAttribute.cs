using System;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class MessageForExchangeCommandQueueTargetAttribute : MessageAttribute
	{
		public MessageForExchangeCommandQueueTargetAttribute( string targetBoundedContext, Type messageListenerType )
			: base( targetBoundedContext, messageListenerType )
		{
		}
	}
}
