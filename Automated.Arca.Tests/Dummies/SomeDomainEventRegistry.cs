using System;
using Automated.Arca.Abstractions.Cqrs;
using Automated.Arca.Attributes.Cqrs;

namespace Automated.Arca.Tests.Dummies
{
	[DomainEventRegistryAttribute]
	public class SomeDomainEventRegistry : IDomainEventRegistry
	{
		public void Add( string eventIdentifier, Type eventType )
		{
		}
	}
}
