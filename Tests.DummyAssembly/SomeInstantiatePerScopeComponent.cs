using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Tests.DummyAssembly
{
	[InstantiatePerScopeAttribute]
	public class SomeInstantiatePerScopeComponent : IProcessable
	{
	}
}
