using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForScopeResolverAttribute : ExtensionForInstantiatePerScopeWithInterfaceAttribute
	{
		public override Type AttributeType => typeof( ScopeResolverAttribute );
		public override Type? RootInterfaceOfTypeWithAttribute => typeof( IScopeResolver<> );
	}
}
