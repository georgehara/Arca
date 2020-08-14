using System;
using System.Collections.Generic;

namespace Automated.Arca.Manager
{
	public interface IOrderedTypes
	{
		bool HasItems { get; }

		IEnumerable<Type> GetOrderedTypes();
		bool Contains( Type type );
		bool Contains<T>();
		void Add( Type type );
		void Add<T>();
	}
}
