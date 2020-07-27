using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	[InstantiatePerScopeAttribute]
	public class SomeScopedComponent : IProcessable
	{
		public readonly SomeInstantiatePerScopeComponent OtherComponent;

		public SomeScopedComponent( SomeInstantiatePerScopeComponent otherComponent )
		{
			OtherComponent = otherComponent;
		}
	}
}
