﻿using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.Specialized
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public class ChainMiddlewarePerContainerAttribute : ProcessableAttribute
	{
	}
}
