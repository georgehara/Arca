using System;

namespace Automated.Arca.Abstractions.Core
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public abstract class ProcessableWithInterfaceAttribute : ProcessableAttribute
	{
		public Type? InterfaceType { get; protected set; }

		public ProcessableWithInterfaceAttribute()
		{
		}

		public ProcessableWithInterfaceAttribute( Type interfaceType )
		{
			InterfaceType = interfaceType;
		}
	}
}
