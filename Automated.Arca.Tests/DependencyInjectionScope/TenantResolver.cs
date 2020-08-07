using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests
{
	public interface ITenantResolver : IScopeResolver<string>
	{
		void SetScopeName( string scopeName );
	}

	[ScopeResolverAttribute]
	public class TenantResolver : ITenantResolver
	{
		private string ScopeName { get; set; } = "";

		public void SetScopeName( string scopeName )
		{
			ScopeName = scopeName;
		}

		public string GetScopeName()
		{
			return ScopeName;
		}
	}
}
