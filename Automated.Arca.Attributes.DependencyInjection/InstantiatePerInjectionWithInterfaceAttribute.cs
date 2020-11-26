using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public class InstantiatePerInjectionWithInterfaceAttribute : ProcessableWithInterfaceAttribute
	{
		public InstantiatePerInjectionWithInterfaceAttribute()
		{
		}

		public InstantiatePerInjectionWithInterfaceAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}
}
