using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Tests.Dummies
{
	[OutboxAttribute( "SomeOutbox" )]
	public class SomeOutbox : IOutbox
	{
	}
}
