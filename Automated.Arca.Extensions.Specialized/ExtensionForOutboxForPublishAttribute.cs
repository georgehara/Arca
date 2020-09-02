using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public class ExtensionForOutboxForPublishAttribute : ExtensionForOutboxAttribute
	{
		public override Type AttributeType => typeof( OutboxForPublishAttribute );
		public override OutboxPublicationType OutboxPublicationType => OutboxPublicationType.Publish;

		public ExtensionForOutboxForPublishAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}
	}
}
