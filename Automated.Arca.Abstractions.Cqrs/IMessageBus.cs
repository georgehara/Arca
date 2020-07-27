using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Cqrs
{
	public interface IMessageBus : IProcessable
	{
		void Subscribe<TMessage, TMessageListener>( string exchange, string queue )
			where TMessage : class
			where TMessageListener : IMessageListener<TMessage>;
	}
}
