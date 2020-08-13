using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class IntegrationEventHandlerAttribute : ProcessableAttribute
	{
		public string EventStreamIdentifier { get; protected set; }

		public IntegrationEventHandlerAttribute( string eventStreamIdentifier )
		{
			EventStreamIdentifier = eventStreamIdentifier;
		}
	}
}
