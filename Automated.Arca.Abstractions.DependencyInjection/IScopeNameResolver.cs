using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IScopeNameResolver<TScopeName> : IProcessable
		where TScopeName : notnull
	{
		TScopeName GetScopeName();
	}
}
