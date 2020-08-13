using System;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class MessageBusSubscribeForExchangeCommandQueueTargetAttribute : MessageBusSubscribeAttribute
	{
		public MessageBusSubscribeForExchangeCommandQueueTargetAttribute( string targetBoundedContext, Type[] messageTypes )
			: base( targetBoundedContext, messageTypes )
		{
		}
	}
}
