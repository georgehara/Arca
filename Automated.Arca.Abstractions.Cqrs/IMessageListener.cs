using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Cqrs
{
	public interface IMessageListener<TMessage> : IProcessable
		 where TMessage : class
	{
	}
}
