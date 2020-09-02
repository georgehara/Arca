using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public abstract class MessageAttribute : ProcessableAttribute
	{
		public string TargetBoundedContext { get; protected set; }
		public Type MessageListenerType { get; protected set; }

		public MessageAttribute( string targetBoundedContext, Type messageListenerType )
		{
			TargetBoundedContext = targetBoundedContext;
			MessageListenerType = messageListenerType;
		}
	}
}
