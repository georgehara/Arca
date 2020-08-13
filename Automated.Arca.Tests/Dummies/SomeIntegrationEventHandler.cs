using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Tests.Dummies
{
	[IntegrationEventHandlerAttribute( SomeIntegrationEventStream.Identifier )]
	public class SomeIntegrationEventHandler : IProcessable
	{
	}

	public class SomeIntegrationEventStream
	{
		public const string Identifier = "SomeIntegrationEventStream";
	}
}
