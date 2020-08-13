using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public interface IIntegrationEventHandlerRegistry : IProcessable
	{
		void Add( string eventStreamIdentifier, Type eventHandlerType );
	}
}
