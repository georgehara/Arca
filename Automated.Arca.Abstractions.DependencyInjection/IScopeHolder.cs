using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IScopeHolder<TScopeName> : IProcessable
		where TScopeName : notnull
	{
		TScopeName ScopeName { get; set; }
	}
}
