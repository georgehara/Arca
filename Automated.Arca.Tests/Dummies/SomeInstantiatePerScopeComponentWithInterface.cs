using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeInstantiatePerScopeComponentWithInterface : IProcessable
	{
	}

	[InstantiatePerScopeWithInterfaceAttribute( typeof( ISomeInstantiatePerScopeComponentWithInterface ) )]
	public class SomeInstantiatePerScopeComponentWithInterface : ISomeInstantiatePerScopeComponentWithInterface
	{
	}
}
