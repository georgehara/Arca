using Automated.Arca.Abstractions.Cqrs;
using Automated.Arca.Attributes.Cqrs;

namespace Automated.Arca.Tests.Dummies
{
	[OutboxAttribute( "SomeOutbox" )]
	public class SomeOutbox : IOutbox
	{
	}
}
