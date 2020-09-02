using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Extensions.Specialized
{
	public abstract class ExtensionForOutboxAttribute : ExtensionForSpecializedAttribute
	{
		public override Type? BaseInterfaceOfTypeWithAttribute => typeof( IOutbox );
		public abstract OutboxPublicationType OutboxPublicationType { get; }

		public ExtensionForOutboxAttribute( IExtensionDependencyProvider extensionDependencyProvider )
			: base( extensionDependencyProvider )
		{
		}

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			// The registration is performed without an interface because each type of outbox has to be registered.

			D.R.InstantiatePerScope( typeWithAttribute, false );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var boundedContext = ((OutboxAttribute)attribute).BoundedContext;
			var outboxProcessor = D.P.GetRequiredInstance<IOutboxProcessor>();

			GenericsHelper.RegisterOutboxToOutboxProcessor( outboxProcessor, typeWithAttribute, boundedContext,
				OutboxPublicationType );
		}
	}
}
