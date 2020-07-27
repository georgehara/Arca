using System;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class ScopeManagerAttribute : InstantiatePerContainerWithInterfaceAttribute
	{
		public ScopeManagerAttribute()
		{
		}

		public ScopeManagerAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}
}
