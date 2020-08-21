using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Libraries;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public abstract class AutomatedMocker : IAutomatedMocker
	{
		public virtual bool MustAvoidMocking( IManager manager, Type type )
		{
			// Some types must not be mocked because they are (normally) essential for the application.

			return
				!type.IsInterface ||
				typeof( IInstanceProvider ).IsAssignableFrom( type ) ||
				type.DerivesFromGenericInterfaceWithSingleParameter( typeof( IScopeManager<> ) ) ||
				type.DerivesFromGenericInterfaceWithSingleParameter( typeof( IScopeNameProvider<> ) ) ||
				manager.ContainsExtensionDependency( type );
		}

		public abstract object GetMock( IManager manager, Type type );
	}
}
