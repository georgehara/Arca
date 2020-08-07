using System;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForScopeResolverAttribute : ExtensionForInstantiatePerScopeWithInterfaceAttribute
	{
		public override Type AttributeType => typeof( ScopeResolverAttribute );
	}
}
