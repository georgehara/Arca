using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Cqrs
{
	public interface ICommandHandlerRegistry : IProcessable
	{
		void Add( Type commandHandlerType );
	}
}
