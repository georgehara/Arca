using System;
using System.Collections.Generic;
using System.Linq;

namespace Automated.Arca.Abstractions.Core
{
	public static class TypeExtensions
	{
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
			var allInterfaces = type.GetInterfaces();
			var immediateInterfaces = new HashSet<Type>( allInterfaces );

			foreach( var i in allInterfaces )
				immediateInterfaces.ExceptWith( i.GetInterfaces() );

			if( immediateInterfaces.Count >= 2 && type.BaseType != null )
			{
				var baseTypeInterfaces = type.BaseType.GetInterfaces();
				immediateInterfaces.ExceptWith( baseTypeInterfaces );
			}

			return immediateInterfaces.ToArray();
		}
	}
}
