using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Extensions.DependencyInjection
{
	public class ExtensionForScopeNameResolverAttribute : ExtensionForInstantiatePerScopeWithInterfaceAttribute
	{
		public override Type AttributeType => typeof( ScopeNameResolverAttribute );
		public override Type? BaseInterfaceOfTypeWithAttribute => typeof( IScopeNameResolver<> );

		public ExtensionForScopeNameResolverAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}
	}
}
