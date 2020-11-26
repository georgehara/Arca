﻿using System;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public class OutboxForPublishAttribute : OutboxAttribute
	{
		public OutboxForPublishAttribute( string boundedContext )
			: base( boundedContext )
		{
		}
	}
}
