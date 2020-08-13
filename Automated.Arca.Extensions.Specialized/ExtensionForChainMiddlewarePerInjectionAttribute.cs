using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForChainMiddlewarePerInjectionAttribute : ExtensionForChainMiddlewareAttribute
	{
		public override Type AttributeType => typeof( ChainMiddlewarePerInjectionAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			ToInstantiatePerInjection( context, typeWithAttribute );
		}
	}
}
