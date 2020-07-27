using System;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForScopeResolverAttribute : ExtensionForInstantiatePerContainerWithInterfaceAttribute
	{
		public override Type AttributeType => typeof( ScopeResolverAttribute );
	}
}
