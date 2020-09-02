using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Abstractions.DependencyInjection;
using Automated.Arca.Abstractions.Specialized;
using Automated.Arca.Libraries;
using Automated.Arca.Tests.Dummies;
using NSubstitute;

namespace Automated.Arca.Tests
{
	public class AutomatedMocker : IAutomatedMocker
	{
		public bool MustAvoidMocking( IManager manager, Type type )
		{
			// Some types must not be mocked because they are essential for testing.

			return
				!type.IsInterface ||
				type.DerivesFromInterfaceOrGenericInterfaceWithUntypedParameter( typeof( ITenantManager ) ) ||
				type.DerivesFromInterfaceOrGenericInterfaceWithUntypedParameter( typeof( ITenantNameProvider ) ) ||
				type.DerivesFromInterfaceOrGenericInterfaceWithUntypedParameter( typeof( IMessageBus ) ) ||
				type.DerivesFromInterfaceOrGenericInterfaceWithUntypedParameter( typeof( IOutboxProcessor ) );
		}

		public object GetMock( IManager manager, Type type )
		{
			if( type == typeof( ISomeComponentWithInterfaceSpecifiedInAttribute ) ||
				type == typeof( ISomeComponentWithInterfaceSpecifiedInAttribute ) )
			{
				return Substitute.For( new Type[] { type, typeof( ISomeConfigurable ) }, new object[ 0 ] );
			}

			return Substitute.For( new Type[] { type }, new object[ 0 ] );
		}
	}
}
