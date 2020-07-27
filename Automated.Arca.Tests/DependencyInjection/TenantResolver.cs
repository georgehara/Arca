using System;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests
{
	public interface ITenantResolver : IScopeResolver<string>
	{
	}

	[ScopeResolverAttribute]
	public class TenantResolver : ITenantResolver
	{
		public string GetScopeName()
		{
			return DateTime.Now.Millisecond.ToString();
		}
	}
}
