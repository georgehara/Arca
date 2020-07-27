using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeInstantiatePerContainerComponentWithInterface : IProcessable
	{
	}

	[InstantiatePerContainerWithInterfaceAttribute( typeof( ISomeInstantiatePerContainerComponentWithInterface ) )]
	public class SomeInstantiatePerContainerComponentWithInterface : ISomeInstantiatePerContainerComponentWithInterface
	{
	}
}
