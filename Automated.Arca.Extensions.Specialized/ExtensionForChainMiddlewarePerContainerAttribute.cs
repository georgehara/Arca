using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForChainMiddlewarePerContainerAttribute : ExtensionForChainMiddlewareAttribute
	{
		public override Type AttributeType => typeof( ChainMiddlewarePerContainerAttribute );

		public ExtensionForChainMiddlewarePerContainerAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			D.R.InstantiatePerContainer( typeWithAttribute, false );
		}
	}
}
