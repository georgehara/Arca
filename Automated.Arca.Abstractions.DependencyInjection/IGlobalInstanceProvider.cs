using System;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IGlobalInstanceProvider : IInstanceProvider
	{
		IDisposable CreateScope<TScopeName>( TScopeName scopeName, out IScopedInstanceProvider<TScopeName> provider );
		IScopedInstanceProvider<TScopeName> CreateScopedProvider<TScopeName>( TScopeName scopeName, out IDisposable scope );
	}
}
