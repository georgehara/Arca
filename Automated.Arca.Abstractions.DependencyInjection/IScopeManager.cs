using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IScopeManager<TScopeName> : IExtensionDependency, IProcessable, IDontAutoMock
		where TScopeName : notnull
	{
		IScopedInstanceProvider<TScopeName> GetOrAdd( TScopeName scopeName );
	}
}
