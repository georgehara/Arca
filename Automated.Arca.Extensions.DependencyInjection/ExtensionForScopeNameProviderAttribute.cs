using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForScopeNameProviderAttribute : ExtensionForInstantiatePerScopeWithInterfaceAttribute
	{
		public override Type AttributeType => typeof( ScopeNameProviderAttribute );
		public override Type? RootInterfaceOfTypeWithAttribute => typeof( IScopeNameProvider<> );
	}
}
