using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	public interface ITenantRequestProcessor : IProcessable
	{
		string ScopeName { get; }
		SomeTenantComponent OtherComponent { get; }

		void HandleRequest( object request );
	}

	[InstantiatePerScopeWithInterfaceAttribute]
	public class TenantRequestProcessor : ITenantRequestProcessor
	{
		private readonly ITenantResolver TenantResolver;

		public TenantRequestProcessor( ITenantResolver tenantResolver, SomeTenantComponent otherComponent )
		{
			TenantResolver = tenantResolver;
			OtherComponent = otherComponent;
		}

		public string ScopeName => TenantResolver.GetScopeName();
		public SomeTenantComponent OtherComponent { get; }

		public void HandleRequest( object request )
		{
		}
	}
}
