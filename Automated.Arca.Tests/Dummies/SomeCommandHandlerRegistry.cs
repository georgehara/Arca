using System;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Attributes.Specialized;

namespace Automated.Arca.Tests.Dummies
{
	[CommandHandlerRegistryAttribute]
	public class SomeCommandHandlerRegistry : ICommandHandlerRegistry
	{
		public void Add( Type commandHandlerType )
		{
		}
	}
}
