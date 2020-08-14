using System;
using System.Collections.Generic;

namespace Automated.Arca.Manager
{
	internal class CachedTypes
	{
		private readonly IOrderedTypes OptionsPriorityTypes;
		private readonly OrderedCachedTypes PriorityTypes = new OrderedCachedTypes();
		private readonly IDictionary<Type, CachedType> OrdinaryTypes = new Dictionary<Type, CachedType>();

		internal CachedTypes( IOrderedTypes optionsPriorityTypes )
		{
			OptionsPriorityTypes = optionsPriorityTypes;
		}

		internal bool HasItems => PriorityTypes.HasItems || OrdinaryTypes.Count > 0;

		internal IEnumerable<CachedType> GetPriorityTypes()
		{
			return OrderFirstLikeSecond( PriorityTypes, OptionsPriorityTypes );
		}

		internal void Add( Type type, CachedType cachedType )
		{
			if( OptionsPriorityTypes.Contains( type ) )
				PriorityTypes.Add( cachedType );
			else
				OrdinaryTypes.Add( type, cachedType );
		}

		internal bool ContainsKey( Type type )
		{
			return OptionsPriorityTypes.Contains( type ) ? PriorityTypes.Contains( type ) : OrdinaryTypes.ContainsKey( type );
		}

		internal void ForAll( Action<CachedType> action )
		{
			foreach( var cachedType in GetPriorityTypes() )
				action( cachedType );

			foreach( var kvp in OrdinaryTypes )
				action( kvp.Value );
		}

		private IEnumerable<CachedType> OrderFirstLikeSecond( OrderedCachedTypes unorderedTypes, IOrderedTypes orderedTypes )
		{
			var result = new List<CachedType>();

			foreach( var type in orderedTypes.GetOrderedTypes() )
			{
				if( unorderedTypes.Contains( type ) )
				{
					var x = unorderedTypes.Get( type )!;

					result.Add( x );
				}
			}

			return result;
		}
	}
}
