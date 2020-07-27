using System;
using Automated.Arca.Abstractions.Cqrs;
using Automated.Arca.Attributes.Cqrs;

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
