using System;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public class ScopeNameResolverAttribute : InstantiatePerScopeWithInterfaceAttribute
	{
		public ScopeNameResolverAttribute()
		{
		}

		public ScopeNameResolverAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}
}
