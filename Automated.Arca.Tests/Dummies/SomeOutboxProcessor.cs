using Automated.Arca.Abstractions.Cqrs;
using Automated.Arca.Attributes.Cqrs;

namespace Automated.Arca.Tests.Dummies
{
	[OutboxProcessorAttribute( typeof( IOutboxProcessor ), "Hangfire:OutboxProcessor:ScheduleCronExpression" )]
	public class SomeOutboxProcessor : IOutboxProcessor
	{
		public void Schedule( string scheduleConfiguration )
		{
		}

		public void Register<TOutbox>( string boundedContext, OutboxPublicationType publicationType )
			where TOutbox : IOutbox
		{
		}
	}
}
