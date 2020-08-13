using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public interface ICommandHandlerRegistry : IProcessable
	{
		void Add( Type commandHandlerType );
	}
}
