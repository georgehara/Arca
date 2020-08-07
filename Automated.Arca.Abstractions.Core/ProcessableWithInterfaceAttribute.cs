using System;

namespace Automated.Arca.Abstractions.Core
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
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

		public Type GetInterfaceOrDefault( Type typeWithAttribute )
		{
			return InterfaceType ?? typeWithAttribute.GetDefaultInterface();
		}
	}
}
