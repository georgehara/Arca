using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForChainMiddlewarePerContainerAttribute : ExtensionForChainMiddlewareAttribute
	{
		public override Type AttributeType => typeof( ChainMiddlewarePerContainerAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			ToInstantiatePerContainer( context, typeWithAttribute );
		}
	}
}
