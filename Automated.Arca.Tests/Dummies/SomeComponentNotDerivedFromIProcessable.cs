using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	[InstantiatePerContainerAttribute]
	public class SomeComponentNotDerivedFromIProcessable
	{
	}
}
