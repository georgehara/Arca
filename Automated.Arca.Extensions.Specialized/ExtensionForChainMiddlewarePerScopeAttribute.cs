using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForChainMiddlewarePerScopeAttribute : ExtensionForChainMiddlewareAttribute
	{
		public override Type AttributeType => typeof( ChainMiddlewarePerScopeAttribute );

		public ExtensionForChainMiddlewarePerScopeAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			D.R.ToInstantiatePerScope( typeWithAttribute, false );
		}
	}
}
