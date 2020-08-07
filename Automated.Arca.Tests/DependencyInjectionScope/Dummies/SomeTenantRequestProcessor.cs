using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeTenantRequestProcessor : IProcessable
	{
		string ScopeName { get; }
		SomeTenantComponentLevel1 Level1 { get; }

		void HandleRequest( object request );
	}

	[InstantiatePerScopeWithInterfaceAttribute]
	public class SomeTenantRequestProcessor : ISomeTenantRequestProcessor
	{
		private readonly ITenantHolder TenantHolder;

		public SomeTenantRequestProcessor( ITenantHolder tenantHolder, SomeTenantComponentLevel1 someTenantComponentLevel1 )
		{
			TenantHolder = tenantHolder;
			Level1 = someTenantComponentLevel1;
		}

		public string ScopeName => TenantHolder.ScopeName;
		public SomeTenantComponentLevel1 Level1 { get; }

		public void HandleRequest( object request )
		{
		}
	}
}
