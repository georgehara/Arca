using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Automated.Arca.Implementations.ForMicrosoft
{
	public class GlobalInstanceProvider : InstanceProvider, IGlobalInstanceProvider
	{
		public GlobalInstanceProvider( IServiceProvider serviceProvider )
			: base( serviceProvider )
		{
		}

		public IDisposable CreateScope<TScopeName>( TScopeName scopeName, out IScopedInstanceProvider<TScopeName> provider )
		{
			var scopeTyped = ServiceProvider.CreateScope();

			provider = new ScopedInstanceProvider<TScopeName>( scopeName, scopeTyped.ServiceProvider );

			return scopeTyped;
		}

		public IScopedInstanceProvider<TScopeName> CreateScopedProvider<TScopeName>( TScopeName scopeName,
			out IDisposable scope )
		{
			var scopeTyped = ServiceProvider.CreateScope();

			scope = scopeTyped;

			return new ScopedInstanceProvider<TScopeName>( scopeName, scopeTyped.ServiceProvider );
		}
	}
}
