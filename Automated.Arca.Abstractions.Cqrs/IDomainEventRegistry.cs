using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Cqrs
{
	public interface IDomainEventRegistry : IProcessable
	{
		void Add( string eventIdentifier, Type eventType );
	}
}
