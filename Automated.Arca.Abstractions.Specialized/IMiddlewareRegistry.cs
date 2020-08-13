using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.Specialized
{
	public interface IMiddlewareRegistry : IExtensionDependency
	{
		void Chain( Type type );
	}
}
