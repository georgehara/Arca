using System;

namespace Automated.Arca.Libraries
{
	public static class TypeExtensions
	{
		public static void EnsureDerivesFrom( this Type implementationType, Type baseType )
		{
			// "Type.IsAssignableFrom" also handles derivation from interfaces.
			if( baseType == implementationType || !baseType.IsAssignableFrom( implementationType ) )
				throw new InvalidCastException( $"Type '{implementationType.Name}' must derive from '{baseType.Name}'" );
		}

		public static void EnsureDerivesFromInterface( this Type implementationType, Type interfaceType )
		{
			// "Type.IsAssignableFrom" also handles derivation from interfaces.
			if( interfaceType == implementationType || !interfaceType.IsInterface
				|| !interfaceType.IsAssignableFrom( implementationType ) )
			{
				throw new InvalidCastException( $"Type '{implementationType.Name}' must derive from the interface" +
					$" '{interfaceType.Name}'" );
			}
		}

		public static void EnsureDerivesFromGenericInterface( this Type implementationType, Type genericInterfaceType
			, Type genericParameterType )
		{
			var realizedGenericType = genericInterfaceType.MakeGenericType( genericParameterType );

			// "Type.IsAssignableFrom" also handles derivation from interfaces.
			if( realizedGenericType == implementationType || realizedGenericType == genericInterfaceType
				|| !genericInterfaceType.IsInterface || !realizedGenericType.IsAssignableFrom( implementationType ) )
			{
				throw new InvalidCastException( $"Type '{implementationType.Name}' must derive from the generic interface" +
					$" '{genericInterfaceType.Name}<{genericParameterType.FullName}>'" );
			}
		}
	}
}
