using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public abstract class MessageBusSubscribeAttribute : ProcessableAttribute
	{
		public string TargetBoundedContext { get; protected set; }
		public Type[] MessageTypes { get; protected set; }

		public MessageBusSubscribeAttribute( string targetBoundedContext, Type[] messageTypes )
		{
			TargetBoundedContext = targetBoundedContext;
			MessageTypes = messageTypes;
		}
	}
}
