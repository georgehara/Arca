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
		private readonly ITenantResolver TenantResolver;

		public SomeTenantRequestProcessor( ITenantResolver tenantResolver, SomeTenantComponentLevel1 someTenantComponentLevel1 )
		{
			TenantResolver = tenantResolver;
			Level1 = someTenantComponentLevel1;
		}

		public string ScopeName => TenantResolver.GetScopeName();
		public SomeTenantComponentLevel1 Level1 { get; }

		public void HandleRequest( object request )
		{
		}
	}
}
