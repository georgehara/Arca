using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Cqrs
{
	public interface IOutboxProcessor : IProcessable
	{
		void Schedule( string scheduleConfiguration );
		void Register<TOutbox>( string boundedContext, OutboxPublicationType publicationType )
			where TOutbox : IOutbox;
	}
}
