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
		private readonly ITenantNameProvider TenantNameProvider;

		public SomeTenantRequestProcessor( ITenantNameProvider tenantNameProvider,
			SomeTenantComponentLevel1 someTenantComponentLevel1 )
		{
			TenantNameProvider = tenantNameProvider;
			Level1 = someTenantComponentLevel1;
		}

		public string ScopeName => TenantNameProvider.ScopeName;
		public SomeTenantComponentLevel1 Level1 { get; }

		public void HandleRequest( object request )
		{
		}
	}
}
