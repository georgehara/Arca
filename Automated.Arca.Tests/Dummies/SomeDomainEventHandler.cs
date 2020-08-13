using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Tests.Dummies
{
	[DomainEventHandlerAttribute( SomeDomainEventStream.Identifier, SomeDomainEvent.Identifier, typeof( SomeDomainEvent ) )]
	public class SomeDomainEventHandler : IProcessable
	{
	}

	public class SomeDomainEventStream
	{
		public const string Identifier = "SomeDomainEventStream";
	}

	public class SomeDomainEvent
	{
		public const string Identifier = "SomeDomainEvent";
	}
}
