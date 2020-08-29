using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForChainMiddlewarePerInjectionAttribute : ExtensionForChainMiddlewareAttribute
	{
		public override Type AttributeType => typeof( ChainMiddlewarePerInjectionAttribute );

		public ExtensionForChainMiddlewarePerInjectionAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			D.R.InstantiatePerInjection( typeWithAttribute, false );
		}
	}
}
