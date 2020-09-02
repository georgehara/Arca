using System.Collections.Generic;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeMessageBus : IMessageBus
	{
		IDictionary<string, string> Registrations { get; }
	}

	[MessageBusAttribute]
	public class SomeMessageBus : ISomeMessageBus
	{
		public IDictionary<string, string> Registrations { get; } = new Dictionary<string, string>();

		public void Register<TMessage, TMessageListener>( string exchange, string queue )
			where TMessage : IMessage
			where TMessageListener : IMessageListener<TMessage>
		{
			Registrations.Add( exchange, queue );
		}
	}

	[MessageForExchangeCommandQueueTargetAttribute( "Some target bounded context", typeof( SomeMessageListener ) )]
	public class SomeMessage : IMessage
	{
	}

	[MessageForExchangePublicationQueueBetweenAttribute( "Some other source bounded context", "Some other target bounded context",
		typeof( SomeMessageListener ) )]
	public class SomeOtherMessage : IMessage
	{
	}

	[MessageListenerAttribute]
	public class SomeMessageListener : IMessageListener<SomeMessage>, IMessageListener<SomeOtherMessage>
	{
	}
}
