using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.DependencyInjection;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeComponentToSubstitute : IProcessable
	{
		string Get( string key );
	}

	[InstantiatePerScopeWithInterfaceAttribute]
	public class SomeComponentToSubstitute : ISomeComponentToSubstitute
	{
		public string Get( string key )
		{
			return "Some value";
		}
	}
}
