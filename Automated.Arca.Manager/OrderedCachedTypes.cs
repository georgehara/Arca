using System;
using System.Collections.Generic;

namespace Automated.Arca.Manager
{
	internal class OrderedCachedTypes
	{
		private readonly IList<CachedType> OrderedListTypes = new List<CachedType>();
		private IDictionary<Type, CachedType>? QuickAccessTypes;
		private int HeightForAdd;
		private int HeightForContains;

		internal int Count => OrderedListTypes!.Count;
		internal bool HasItems => Count > 0;

		internal OrderedCachedTypes()
		{
			RecreateQuickAccessTypes();
		}

		internal CachedType? Get( Type type )
		{
			return QuickAccessTypes!.ContainsKey( type ) ? QuickAccessTypes[ type ] : null;
		}

		internal bool Contains( Type type )
		{
			if( HeightForContains < HeightForAdd )
				RecreateQuickAccessTypes();

			return QuickAccessTypes!.ContainsKey( type );
		}

		internal bool Contains<T>()
		{
			return Contains( typeof( T ) );
		}

		internal void Add( CachedType type )
		{
			OrderedListTypes.Add( type );

			HeightForAdd++;
		}

		private void RecreateQuickAccessTypes()
		{
			QuickAccessTypes = new Dictionary<Type, CachedType>();

			foreach( var cachedType in OrderedListTypes )
				QuickAccessTypes.Add( cachedType.Type, cachedType );

			HeightForContains = HeightForAdd;
		}
	}
}
