using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public interface IMessageListener<TMessage> : IProcessable
		 where TMessage : class
	{
	}
}
