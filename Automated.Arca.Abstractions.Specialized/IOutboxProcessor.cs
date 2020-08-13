using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public interface IOutboxProcessor : IProcessable
	{
		void Schedule( string scheduleConfiguration );
		void Register<TOutbox>( string boundedContext, OutboxPublicationType publicationType )
			where TOutbox : IOutbox;
	}
}
