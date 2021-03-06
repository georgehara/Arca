﻿using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
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
