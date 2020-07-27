using System;

namespace Automated.Arca.Abstractions.Core
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public abstract class ProcessableAttribute : Attribute
	{
	}
}
