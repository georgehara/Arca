using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public interface IAutomatedMockingProvider
	{
		bool MustAvoidMocking( IManager manager, Type type );
		object GetMock( IManager manager, Type type );
	}
}
