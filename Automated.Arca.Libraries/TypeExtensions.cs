using System;
using System.Linq;

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

		public static void EnsureDerivesFromGenericInterfaceWithSingleParameter( this Type implementationType,
			Type genericInterfaceType )
		{
			if( !implementationType.DerivesFromGenericInterfaceWithSingleParameter( genericInterfaceType ) )
			{
				throw new InvalidCastException( $"Type '{implementationType.Name}' must derive from the generic interface" +
					$" '{genericInterfaceType.Name}<TScopeName>'" );
			}
		}

		public static bool DerivesFromGenericInterfaceWithSingleParameter( this Type implementationType, Type genericInterfaceType )
		{
			if( implementationType == null || genericInterfaceType == null )
				return false;

			if( genericInterfaceType == implementationType || !genericInterfaceType.IsInterface )
				return false;

			var extractedInterface = implementationType.GetInterfaces().FirstOrDefault( x => x.Name == genericInterfaceType.Name );

			return extractedInterface != null && extractedInterface.GenericTypeArguments.Length == 1;
		}
	}
}
