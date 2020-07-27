using Automated.Arca.Abstractions.Core;
using Automated.Arca.Attributes.Cqrs;

namespace Automated.Arca.Tests.Dummies
{
	[CommandHandlerAttribute]
	public class SomeCommandHandler : IProcessable
	{
	}
}
