using System;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class ScopeResolverAttribute : InstantiatePerScopeWithInterfaceAttribute
	{
		public ScopeResolverAttribute()
		{
		}

		public ScopeResolverAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}
}
