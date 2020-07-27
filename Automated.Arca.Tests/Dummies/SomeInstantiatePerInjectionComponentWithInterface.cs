using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeInstantiatePerInjectionComponentWithInterface : IProcessable
	{
	}

	[InstantiatePerInjectionWithInterfaceAttribute( typeof( ISomeInstantiatePerInjectionComponentWithInterface ) )]
	public class SomeInstantiatePerInjectionComponentWithInterface : ISomeInstantiatePerInjectionComponentWithInterface
	{
	}
}
