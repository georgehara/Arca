﻿using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public abstract class OutboxAttribute : ProcessableAttribute
	{
		public string BoundedContext { get; private set; }

		public OutboxAttribute( string boundedContext )
		{
			BoundedContext = boundedContext;
		}
	}
}
