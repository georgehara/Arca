using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public abstract class AutomatedMockingProvider : IAutomatedMockingProvider
	{
		public virtual bool MustAvoidMocking( IManager manager, Type type )
		{
			// Some types must not be mocked because they are (normally) essential for the application.

			return
				!type.IsInterface ||
				typeof( IDontAutoMock ).IsAssignableFrom( type ) ||
				manager.ContainsExtensionDependency( type );
		}

		public abstract object GetMock( IManager manager, Type type );
	}
}
