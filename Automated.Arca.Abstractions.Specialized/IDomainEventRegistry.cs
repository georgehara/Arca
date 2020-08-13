using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public interface IDomainEventRegistry : IProcessable
	{
		void Add( string eventIdentifier, Type eventType );
	}
}
