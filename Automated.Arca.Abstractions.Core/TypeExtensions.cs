using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Automated.Arca.Abstractions.Core
{
	public static class TypeExtensions
	{
		public static T[]? GetProcessableAttributes<T>( this Type type )
			where T : ProcessableAttribute
		{
			var attributes = type.GetCustomAttributes<T>( false );
			if( attributes == null )
				return null;

			return attributes.ToArray();
		}

		public static ProcessableAttribute[]? GetProcessableAttributes( this Type type )
		{
			return GetProcessableAttributes<ProcessableAttribute>( type );
		}

		public static Type GetDefaultInterface( this Type type )
		{
			var interfaces = GetImmediateInterfaces( type );

			if( interfaces == null || interfaces.Length <= 0 )
				throw new InvalidOperationException( $"Type '{type.Name}' doesn't derive from any interface, so a default" +
					$" one can't be determined." );

			if( interfaces.Length > 1 )
			{
				throw new InvalidOperationException( $"Type '{type.Name}' derives from more than one interface, so a default" +
					$" one can't be determined." );
			}

			return interfaces.Single();
		}

		private static Type[] GetImmediateInterfaces( Type type )
		{
			var (typeInterfaces, baseTypeInterfaces) = GetInterfacesOfTypeWhoseBaseTypeHasFewerInterfaces( type );

			var immediateInterfaces = new HashSet<Type>( typeInterfaces );

			if( baseTypeInterfaces != null )
				immediateInterfaces.ExceptWith( baseTypeInterfaces );

			if( immediateInterfaces.Count > 1 )
			{
				foreach( var i in typeInterfaces )
					immediateInterfaces.ExceptWith( i.GetInterfaces() );
			}

			return immediateInterfaces.ToArray();
		}

		private static (Type[], Type[]?) GetInterfacesOfTypeWhoseBaseTypeHasFewerInterfaces( Type type )
		{
			if( type.BaseType == null )
				return (type.GetInterfaces(), null);

			if( type.GetInterfaces().Length != type.BaseType.GetInterfaces().Length )
				return (type.GetInterfaces(), type.BaseType.GetInterfaces());

			return GetInterfacesOfTypeWhoseBaseTypeHasFewerInterfaces( type.BaseType );
		}
	}
}
