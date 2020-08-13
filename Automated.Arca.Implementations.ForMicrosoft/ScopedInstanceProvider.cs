using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public class ScopedInstanceProvider<TScopeName> : InstanceProvider, IScopedInstanceProvider<TScopeName>
	{
		public ScopedInstanceProvider( TScopeName scopeName, IServiceProvider serviceProvider )
			: base( serviceProvider )
		{
			ScopeName = scopeName;
		}

		public TScopeName ScopeName { get; private set; }

		public IDisposable CreateScope( TScopeName scopeName, out IScopedInstanceProvider<TScopeName> provider )
		{
			var scopeTyped = ServiceProvider.CreateScope();

			provider = new ScopedInstanceProvider<TScopeName>( scopeName, scopeTyped.ServiceProvider );

			return scopeTyped;
		}

		public IScopedInstanceProvider<TScopeName> CreateScopedProvider( TScopeName scopeName, out IDisposable scope )
		{
			var scopeTyped = ServiceProvider.CreateScope();

			scope = scopeTyped;

			return new ScopedInstanceProvider<TScopeName>( scopeName, scopeTyped.ServiceProvider );
		}
	}
}
