using System;

namespace Automated.Arca.Abstractions.Core
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public abstract class ProcessableAttribute : Attribute
	{
	}
}
