using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Cqrs
{
	public interface IDomainEventHandlerRegistry : IProcessable
	{
		void Add( string eventStreamIdentifier, Type eventHandlerType );
	}
}
