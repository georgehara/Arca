using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class InstantiatePerScopeWithInterfaceAttribute : ProcessableWithInterfaceAttribute
	{
		public InstantiatePerScopeWithInterfaceAttribute()
		{
		}

		public InstantiatePerScopeWithInterfaceAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}
}
