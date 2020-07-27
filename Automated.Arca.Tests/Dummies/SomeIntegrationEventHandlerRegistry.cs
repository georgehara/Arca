using System;
using Automated.Arca.Abstractions.Cqrs;
using Automated.Arca.Attributes.Cqrs;

namespace Automated.Arca.Tests.Dummies
{
	[IntegrationEventHandlerRegistryAttribute]
	public class SomeIntegrationEventHandlerRegistry : IIntegrationEventHandlerRegistry
	{
		public void Add( string eventStreamIdentifier, Type eventHandlerType )
		{
		}
	}
}
