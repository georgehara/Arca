using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Cqrs
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class OutboxAttribute : ProcessableAttribute
	{
		public string BoundedContext { get; private set; }

		public OutboxAttribute( string boundedContext )
		{
			BoundedContext = boundedContext;
		}
	}
}
