using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	[InstantiatePerScopeAttribute]
	public class SomeScopedComponentWithScopedComponent : IProcessable
	{
		public readonly ITenantResolver TenantResolver;
		public readonly SomeScopedComponent OtherComponent;

		public SomeScopedComponentWithScopedComponent( ITenantResolver tenantResolver, SomeScopedComponent otherComponent )
		{
			TenantResolver = tenantResolver;
			OtherComponent = otherComponent;
		}
	}
}
