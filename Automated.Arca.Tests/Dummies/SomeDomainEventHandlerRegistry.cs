using System;
using Automated.Arca.Abstractions.Cqrs;
using Automated.Arca.Attributes.Cqrs;

namespace Automated.Arca.Tests.Dummies
{
	[DomainEventHandlerRegistryAttribute]
	public class SomeDomainEventHandlerRegistry : IDomainEventHandlerRegistry
	{
		public void Add( string eventStreamIdentifier, Type eventHandlerType )
		{
		}
	}
}
