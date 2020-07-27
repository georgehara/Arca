using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Tests.Dummies
{
	public interface ISomeConfigurable : IProcessable
	{
		bool Configured { get; set; }
	}
}
