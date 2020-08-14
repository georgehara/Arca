using System.Collections.Generic;
using System.Text;

namespace Automated.Arca.Libraries
{
	public static class StringExtensions
	{
		public static string JoinWithFormat( this IEnumerable<string> texts, string format, string separator )
		{
			StringBuilder sb = new StringBuilder();

			var enumerator = texts.GetEnumerator();

			bool hasMore = enumerator.MoveNext();
			while( hasMore )
			{
				if( enumerator.Current != null )
					sb.AppendFormat( format, enumerator.Current );

				hasMore = enumerator.MoveNext();

				if( hasMore )
					sb.Append( separator );
			}

			return sb.ToString();
		}
	}
}
