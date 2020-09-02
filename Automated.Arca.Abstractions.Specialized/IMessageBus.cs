using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public interface IMessageBus : IProcessable
	{
		void Register<TMessage, TMessageListener>( string exchange, string queue )
			where TMessage : IMessage
			where TMessageListener : IMessageListener<TMessage>;
	}
}
