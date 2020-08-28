using System;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class MultiInstantiatePerScopeAttribute : MultiInstantiateAttribute
	{
		public MultiInstantiatePerScopeAttribute( Type interfaceType, string implementationKey )
			: base( interfaceType, implementationKey )
		{
		}
	}
}
