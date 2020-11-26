using System;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public class ScopeNameProviderAttribute : InstantiatePerScopeWithInterfaceAttribute
	{
		public ScopeNameProviderAttribute()
		{
		}

		public ScopeNameProviderAttribute( Type interfaceType )
			: base( interfaceType )
		{
		}
	}
}
