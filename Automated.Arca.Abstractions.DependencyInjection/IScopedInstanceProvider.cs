using System;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IScopedInstanceProvider<TScopeName> : IInstanceProvider
	{
		TScopeName ScopeName { get; }

		IDisposable CreateScope( TScopeName scopeName, out IScopedInstanceProvider<TScopeName> provider );
		IScopedInstanceProvider<TScopeName> CreateScopedProvider( TScopeName scopeName, out IDisposable scope );
	}
}
