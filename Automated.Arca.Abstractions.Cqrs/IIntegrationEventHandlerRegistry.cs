using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Cqrs
{
	public interface IIntegrationEventHandlerRegistry : IProcessable
	{
		void Add( string eventStreamIdentifier, Type eventHandlerType );
	}
}
