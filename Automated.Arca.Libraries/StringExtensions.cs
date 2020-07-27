using System.Collections.Generic;
using System.Text;

namespace Automated.Arca.Libraries
{
	public static class StringExtensions
	{
		public static string JoinWithFormat( this IList<string> texts, string format, string separator )
		{
			StringBuilder sb = new StringBuilder();

			var count = texts.Count;
			for( int i = 0; i < count; i++ )
			{
				sb.AppendFormat( format, texts[ i ] );

				if( i < count - 1 )
					sb.Append( separator );
			}

			return sb.ToString();
		}
	}
}
