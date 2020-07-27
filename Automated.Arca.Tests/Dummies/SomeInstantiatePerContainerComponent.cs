using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	[InstantiatePerContainerAttribute]
	public class SomeInstantiatePerContainerComponent : IProcessable
	{
	}
}
