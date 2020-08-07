using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	[InstantiatePerScopeAttribute]
	public class SomeTenantComponentLevel2 : IProcessable
	{
	}
}
