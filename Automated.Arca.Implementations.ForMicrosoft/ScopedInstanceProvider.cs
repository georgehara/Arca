using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public class ScopedInstanceProvider<TScopeName> : InstanceProvider, IScopedInstanceProvider<TScopeName>
	{
		public ScopedInstanceProvider( TScopeName scopeName, IServiceProvider dependency )
			: base( dependency )
		{
			ScopeName = scopeName;
		}

		public TScopeName ScopeName { get; private set; }

		public IDisposable CreateScope( TScopeName scopeName, out IScopedInstanceProvider<TScopeName> provider )
		{
			var scopeTyped = Dependency.CreateScope();

			provider = new ScopedInstanceProvider<TScopeName>( scopeName, scopeTyped.ServiceProvider );

			return scopeTyped;
		}

		public IScopedInstanceProvider<TScopeName> CreateScopedProvider( TScopeName scopeName, out IDisposable scope )
		{
			var scopeTyped = Dependency.CreateScope();

			scope = scopeTyped;

			return new ScopedInstanceProvider<TScopeName>( scopeName, scopeTyped.ServiceProvider );
		}
	}
}
