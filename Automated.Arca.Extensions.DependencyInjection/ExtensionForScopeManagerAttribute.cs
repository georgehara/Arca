using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForScopeManagerAttribute : ExtensionForInstantiatePerContainerWithInterfaceAttribute
	{
		public override Type AttributeType => typeof( ScopeManagerAttribute );
		public override Type? RootInterfaceOfTypeWithAttribute => typeof( IScopeManager<> );
	}
}
