using System;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = false )]
	public class MultiInstantiatePerContainerAttribute : MultiInstantiateAttribute
	{
		public MultiInstantiatePerContainerAttribute( string implementationKey )
			: base( implementationKey )
		{
		}

		public MultiInstantiatePerContainerAttribute( Type interfaceType, string implementationKey )
			: base( interfaceType, implementationKey )
		{
		}
	}
}
