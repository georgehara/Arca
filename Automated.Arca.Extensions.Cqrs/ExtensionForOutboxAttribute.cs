using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.Cqrs;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.Cqrs;

namespace Automated.Arca.Extensions.Cqrs
{
	public class ExtensionForOutboxAttribute : ExtensionForProcessableAttribute
	{
		public override Type AttributeType => typeof( OutboxAttribute );

		public override void Register( IRegistrationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			ToInstantiatePerScope( context, typeWithAttribute );
		}

		public override void Configure( IConfigurationContext context, ProcessableAttribute attribute, Type typeWithAttribute )
		{
			var boundedContext = ((OutboxAttribute)attribute).BoundedContext;
			var outboxProcessor = Provider( context ).GetRequiredInstance<IOutboxProcessor>();

			GenericsHelper.RegisterOutboxToOutboxProcessor( outboxProcessor, typeWithAttribute, boundedContext,
				OutboxPublicationType.Publish );
		}
	}
}
