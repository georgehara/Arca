using System;

namespace Automated.Arca.Attributes.DependencyInjection
{
	[AttributeUsage( validOn: AttributeTargets.Class, AllowMultiple = true )]
	public class MultiInstantiatePerScopeAttribute : MultiInstantiateAttribute
	{
		public MultiInstantiatePerScopeAttribute( string implementationKey )
			: base( implementationKey )
		{
		}

		public MultiInstantiatePerScopeAttribute( Type interfaceType, string implementationKey )
			: base( interfaceType, implementationKey )
		{
		}
	}
}
