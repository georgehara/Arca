using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests
{
	public interface ITenantHolder : IScopeHolder<string>
	{
	}

	[ScopeHolderAttribute]
	public class TenantHolder : ITenantHolder
	{
		public string ScopeName { get; set; } = "";
	}
}
