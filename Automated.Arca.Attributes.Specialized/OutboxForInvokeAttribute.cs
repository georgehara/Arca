using System;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class OutboxForInvokeAttribute : OutboxAttribute
	{
		public OutboxForInvokeAttribute( string boundedContext )
			: base( boundedContext )
		{
		}
	}
}
