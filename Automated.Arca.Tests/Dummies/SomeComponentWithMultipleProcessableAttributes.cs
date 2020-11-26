using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeComponentWithInterfaceSpecifiedInAttribute1 : IProcessable
	{
	}

	public interface ISomeComponentWithInterfaceSpecifiedInAttribute2 : IProcessable
	{
	}

	[InstantiatePerScopeWithInterfaceAttribute( typeof( ISomeComponentWithInterfaceSpecifiedInAttribute1 ) )]
	[InstantiatePerScopeWithInterfaceAttribute( typeof( ISomeComponentWithInterfaceSpecifiedInAttribute2 ) )]
	public class SomeComponentWithMultipleProcessableAttributes : ISomeComponentWithInterfaceSpecifiedInAttribute1,
		ISomeComponentWithInterfaceSpecifiedInAttribute2
	{
	}
}
