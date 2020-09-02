using System.Collections.Generic;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeOutboxProcessor : IOutboxProcessor
	{
		IDictionary<string, OutboxPublicationType> Registrations { get; }
	}

	[OutboxProcessorAttribute( "Hangfire:OutboxProcessor:ScheduleCronExpression" )]
	public class SomeOutboxProcessor : ISomeOutboxProcessor
	{
		public IDictionary<string, OutboxPublicationType> Registrations { get; } = new Dictionary<string, OutboxPublicationType>();

		public void Schedule( string scheduleConfiguration )
		{
		}

		public void Register<TOutbox>( string boundedContext, OutboxPublicationType publicationType )
			where TOutbox : IOutbox
		{
			Registrations.Add( boundedContext, publicationType );
		}
	}

	[OutboxForInvokeAttribute( "Some bounded context for invoke" )]
	public class SomeOutboxForInvoke : IOutbox
	{
	}

	[OutboxForPublishAttribute( "Some bounded context for publish" )]
	public class SomeOutboxForPublish : IOutbox
	{
	}
}
