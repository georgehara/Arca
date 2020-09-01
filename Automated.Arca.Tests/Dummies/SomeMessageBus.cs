using System;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeMessageBus : IMessageBus
	{
		string? Exchange { get; }
		string? Queue { get; }
	}

	[MessageBusAttribute]
	public class SomeMessageBus : ISomeMessageBus
	{
		public string? Exchange { get; private set; }
		public string? Queue { get; private set; }

		public void Subscribe<TMessage, TMessageListener>( string exchange, string queue )
			where TMessage : class
			where TMessageListener : IMessageListener<TMessage>
		{
			Exchange = exchange;
			Queue = queue;
		}
	}

	public class SomeMessage
	{
	}

	[MessageBusSubscribeForExchangePublicationQueueBetweenAttribute( "Some source bounded context", "Some target bounded context",
		new Type[] { typeof( SomeMessage ) } )]
	public class SomeMessageListener : IMessageListener<SomeMessage>
	{
	}
}
