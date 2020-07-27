using System;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForScopeManagerAttribute : ExtensionForInstantiatePerContainerWithInterfaceAttribute
	{
		public override Type AttributeType => typeof( ScopeManagerAttribute );
	}
}
