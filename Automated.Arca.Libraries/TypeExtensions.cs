using System;
using System.Linq;

namespace Automated.Arca.Libraries
{
	public static class TypeExtensions
	{
		public static void EnsureDerivesFromNotEqual( this Type implementationType, Type baseType )
		{
			// "Type.IsAssignableFrom" also handles derivation from interfaces.
			if( baseType == implementationType || !baseType.IsAssignableFrom( implementationType ) )
				throw new InvalidCastException( $"Type '{implementationType.Name}' must derive from '{baseType.Name}'" );
		}

		public static void EnsureDerivesFromGenericInterfaceNotEqual( this Type implementationType, Type genericInterfaceType
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

		public static void EnsureDerivesFromInterface( this Type implementationType, Type interfaceType )
		{
			if( !implementationType.DerivesFromInterface( interfaceType ) )
			{
				throw new InvalidCastException( $"Type '{implementationType.Name}' must derive from the interface" +
					$" '{interfaceType.Name}'" );
			}
		}

		public static void EnsureDerivesFromGenericInterfaceWithUntypedParameter( this Type implementationType,
			Type genericInterfaceType )
		{
			if( !implementationType.DerivesFromGenericInterfaceWithUntypedParameter( genericInterfaceType ) )
			{
				throw new InvalidCastException( $"Type '{implementationType.Name}' must derive from the generic interface" +
					$" '{genericInterfaceType.Name}<T>'" );
			}
		}

		public static void EnsureDerivesFromInterfaceOrGenericInterfaceWithUntypedParameter( this Type implementationType,
			Type interfaceType )
		{
			if( !implementationType.DerivesFromInterfaceOrGenericInterfaceWithUntypedParameter( interfaceType ) )
			{
				throw new InvalidCastException( $"Type '{implementationType.Name}' must derive from the interface" +
					$" '{interfaceType.Name}' or from the generic interface '{interfaceType.Name}<T>'" );
			}
		}

		public static bool DerivesFromInterface( this Type implementationType, Type interfaceType )
		{
			if( implementationType == null || interfaceType == null )
				return false;

			if( !interfaceType.IsInterface )
				return false;

			// "Type.IsAssignableFrom" also handles derivation from interfaces.
			return interfaceType.IsAssignableFrom( implementationType );
		}

		public static bool DerivesFromGenericInterfaceWithUntypedParameter( this Type implementationType,
			Type genericInterfaceType )
		{
			if( implementationType == null || genericInterfaceType == null )
				return false;

			if( !genericInterfaceType.IsInterface )
				return false;

			var extractedInterface = implementationType.GetInterfaces().FirstOrDefault( x => x.Name == genericInterfaceType.Name );

			return extractedInterface != null && extractedInterface.GenericTypeArguments.Length == 1;
		}

		public static bool DerivesFromInterfaceOrGenericInterfaceWithUntypedParameter( this Type implementationType,
			Type interfaceType )
		{
			return implementationType.DerivesFromInterface( interfaceType ) ||
				implementationType.DerivesFromGenericInterfaceWithUntypedParameter( interfaceType );
		}
	}
}
