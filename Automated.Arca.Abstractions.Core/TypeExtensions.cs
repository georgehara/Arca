using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Automated.Arca.Abstractions.Core
{
	public static class TypeExtensions
	{
		public static T? GetProcessableAttribute<T>( this Type type )
			where T : ProcessableAttribute
		{
			var attributes = type.GetCustomAttributes<T>( false );
			if( attributes == null )
				return null;

			var count = attributes.Count();
			if( count <= 0 )
				return null;

			if( count > 1 )
			{
				throw new InvalidOperationException( $"Class '{type.Name}' may have applied on it only one attribute" +
					$" derived from '{nameof( T )}', not {count}." );
			}

			return attributes.SingleOrDefault();
		}

		public static ProcessableAttribute? GetProcessableAttribute( this Type type )
		{
			return GetProcessableAttribute<ProcessableAttribute>( type );
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
			var typeWhoseBaseTypeHasFewerInterfaces = GetTypeWhoseBaseTypeHasFewerInterfaces( type );

			var allInterfaces = typeWhoseBaseTypeHasFewerInterfaces.GetInterfaces();
			var immediateInterfaces = new HashSet<Type>( allInterfaces );

			if( typeWhoseBaseTypeHasFewerInterfaces.BaseType != null )
				immediateInterfaces.ExceptWith( typeWhoseBaseTypeHasFewerInterfaces.BaseType.GetInterfaces() );

			foreach( var i in allInterfaces )
				immediateInterfaces.ExceptWith( i.GetInterfaces() );

			return immediateInterfaces.ToArray();
		}

		private static Type GetTypeWhoseBaseTypeHasFewerInterfaces( Type type )
		{
			if( type.BaseType == null )
				return type;

			if( type.GetInterfaces().Length != type.BaseType.GetInterfaces().Length )
				return type;

			return GetTypeWhoseBaseTypeHasFewerInterfaces( type.BaseType );
		}
	}
}
