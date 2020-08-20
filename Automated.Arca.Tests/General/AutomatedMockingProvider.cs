using System;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Tests.Dummies;
using NSubstitute;

namespace Automated.Arca.Tests
{
	public class AutomatedMockingProvider : Abstractions.DependencyInjection.AutomatedMockingProvider
	{
		public override object GetMock( IManager manager, Type type )
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
