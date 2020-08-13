using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public interface IDomainEventHandlerRegistry : IProcessable
	{
		void Add( string eventStreamIdentifier, Type eventHandlerType );
	}
}
