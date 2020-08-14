using System;
using System.Collections.Generic;

namespace Automated.Arca.Manager
{
	public class OrderedTypes : IOrderedTypes
	{
		private readonly IList<Type> OrderedListTypes = new List<Type>();
		private ICollection<Type>? QuickAccessTypes;
		private int HeightForAdd;
		private int HeightForContains;

		public OrderedTypes()
		{
			RecreateQuickAccessTypes();
		}

		public bool HasItems => OrderedListTypes!.Count > 0;

		public IEnumerable<Type> GetOrderedTypes()
		{
			return OrderedListTypes;
		}

		public bool Contains( Type type )
		{
			if( HeightForContains < HeightForAdd )
				RecreateQuickAccessTypes();

			return QuickAccessTypes!.Contains( type );
		}

		public bool Contains<T>()
		{
			return Contains( typeof( T ) );
		}

		public void Add( Type type )
		{
			OrderedListTypes.Add( type );

			HeightForAdd++;
		}

		public void Add<T>()
		{
			Add( typeof( T ) );
		}

		private void RecreateQuickAccessTypes()
		{
			QuickAccessTypes = new HashSet<Type>();

			foreach( var type in OrderedListTypes )
				QuickAccessTypes.Add( type );

			HeightForContains = HeightForAdd;
		}
	}
}
