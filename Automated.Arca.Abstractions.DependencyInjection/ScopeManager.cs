using System;
using System.Collections.Generic;
using Automated.Arca.Libraries;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public abstract class ScopeManager<TScopeName> : DisposableObject, IScopeManager<TScopeName>
		where TScopeName : notnull
	{
		private class Pair
		{
			public IDisposable Scope;
			public IScopedInstanceProvider<TScopeName> InstanceProvider;

			public Pair( IDisposable scope, IScopedInstanceProvider<TScopeName> instanceProvider )
			{
				Scope = scope;
				InstanceProvider = instanceProvider;
			}
		}

		private readonly IGlobalInstanceProvider ParentProvider;

		private readonly object Lock = new object();
		private readonly IDictionary<TScopeName, Pair> ScopePairs = new Dictionary<TScopeName, Pair>();

		public ScopeManager( IGlobalInstanceProvider parentProvider )
		{
			ParentProvider = parentProvider;
		}

		public IScopedInstanceProvider<TScopeName> GetOrAdd( TScopeName scopeName )
		{
			lock( Lock )
			{
				if( ScopePairs.ContainsKey( scopeName ) )
					return ScopePairs[ scopeName ].InstanceProvider;

				var scope = ParentProvider.CreateScope( scopeName, out IScopedInstanceProvider<TScopeName> provider );

				ScopePairs.Add( scopeName, new Pair( scope, provider ) );

				return provider;
			}
		}

		public override void Disposing()
		{
			lock( Lock )
			{
				foreach( var kvp in ScopePairs )
					kvp.Value.Scope.Dispose();

				ScopePairs.Clear();
			}
		}
	}
}
