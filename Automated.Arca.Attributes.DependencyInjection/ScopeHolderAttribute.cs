using System;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class ScopeHolderAttribute : InstantiatePerScopeWithInterfaceAttribute
	{
		public ScopeHolderAttribute()
		{
		}

		public ScopeHolderAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}
}
