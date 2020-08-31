using System;
using System.Collections.Generic;
using Automated.Arca.Abstractions.Core;
using Automated.Arca.Libraries;

namespace Automated.Arca.Abstractions.DependencyInjection
{
	public class MultiImplementationRegistry : IMultiImplementationRegistry
	{
		private readonly IDictionary<Type, IDictionary<string, Type>> Registrations =
			new Dictionary<Type, IDictionary<string, Type>>();

		public MultiImplementationRegistry()
		{
		}

		public void Add( Type interfaceType, string implementationKey, Type implementationType )
		{
			implementationType.EnsureDerivesFromInterfaceOrGenericInterfaceWithUntypedParameter( interfaceType );

			if( !Registrations.ContainsKey( interfaceType ) )
				Registrations.Add( interfaceType, new Dictionary<string, Type>() );

			var implementationKeys = Registrations[ interfaceType ];

			if( implementationKeys.ContainsKey( implementationKey ) )
			{
				throw new InvalidOperationException( $"A multi-implementation record for the implementation key" +
					$" '{implementationKey}' already exists for the interface '{interfaceType.Name}'" );
			}

			implementationKeys.Add( implementationKey, implementationType );
		}

		public Type GetRequiredImplementationType( Type interfaceType, string implementationKey )
		{
			if( !Registrations.ContainsKey( interfaceType ) )
				throw new InvalidOperationException( GetErrorMessage( interfaceType, implementationKey ) );

			if( !Registrations[ interfaceType ].ContainsKey( implementationKey ) )
				throw new InvalidOperationException( GetErrorMessage( interfaceType, implementationKey ) );

			return Registrations[ interfaceType ][ implementationKey ];
		}

		private string GetErrorMessage( Type interfaceType, string implementationKey )
		{
			var nl = Environment.NewLine;

			return
				$"A multi-implementation record doesn't exist for the interface '{interfaceType.Name}' and implementation key" +
					$" '{implementationKey}'" +
				$"{nl}Check that there is an implementation class which:" +
				$"{nl}\t* Is public, concrete, and implements '{interfaceType.Name}'." +
				$"{nl}\t* Has applied on it a multi-instantiation attribute with the interface '{interfaceType.Name}' and" +
					$" multi-implementation key '{implementationKey}'." +
				$"{nl}\t* Implements '{nameof( IProcessable )}', if the manager option requires it.";
		}
	}
}
