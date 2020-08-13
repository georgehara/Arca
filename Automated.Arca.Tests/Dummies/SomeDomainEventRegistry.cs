using System;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

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
