using System;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

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
