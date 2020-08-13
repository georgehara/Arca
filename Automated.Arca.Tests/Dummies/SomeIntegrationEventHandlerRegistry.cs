using System;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

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
