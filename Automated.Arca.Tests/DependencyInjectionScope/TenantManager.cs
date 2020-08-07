using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests
{
	public interface ITenantManager : IScopeManager<string>
	{
	}

	[ScopeManagerAttribute]
	public class TenantManager : ScopeManager<string>, ITenantManager
	{
		public TenantManager( IGlobalInstanceProvider parentProvider )
			: base( parentProvider )
		{
		}
	}
}
