using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests
{
	public interface ITenantNameProvider : IScopeNameProvider<string>
	{
	}

	[ScopeNameProviderAttribute]
	public class TenantNameProvider : ITenantNameProvider
	{
		public string ScopeName { get; set; } = "";
	}
}
