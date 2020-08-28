using System;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class MultiInstantiatePerInjectionAttribute : MultiInstantiateAttribute
	{
		public MultiInstantiatePerInjectionAttribute( Type interfaceType, string implementationKey )
			: base( interfaceType, implementationKey )
		{
		}
	}
}
