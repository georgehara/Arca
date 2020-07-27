using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Cqrs
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class DomainEventHandlerAttribute : ProcessableAttribute
	{
		public string EventStreamIdentifier { get; protected set; }
		public string EventIdentifier { get; protected set; }
		public Type EventType { get; protected set; }

		public DomainEventHandlerAttribute( string eventStreamIdentifier, string eventIdentifier, Type eventType )
		{
			EventStreamIdentifier = eventStreamIdentifier;
			EventIdentifier = eventIdentifier;
			EventType = eventType;
		}
	}
}
