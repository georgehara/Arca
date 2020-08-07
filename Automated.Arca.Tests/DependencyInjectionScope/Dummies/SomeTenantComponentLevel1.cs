using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	[InstantiatePerScopeAttribute]
	public class SomeTenantComponentLevel1 : IProcessable
	{
		public SomeTenantComponentLevel1( SomeTenantComponentLevel2 someTenantComponentLevel2 )
		{
			Level2 = someTenantComponentLevel2;
		}

		public SomeTenantComponentLevel2 Level2 { get; }
	}
}
