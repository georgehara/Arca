using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForOutboxForInvokeAttribute : ExtensionForOutboxAttribute
	{
		public override Type AttributeType => typeof( OutboxForInvokeAttribute );
		public override OutboxPublicationType OutboxPublicationType => OutboxPublicationType.Invoke;

		public ExtensionForOutboxForInvokeAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}
	}
}
