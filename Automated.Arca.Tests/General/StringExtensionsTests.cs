using System.Collections.Generic;
using Automated.Arca.Libraries;
using Xunit;

namespace Automated.Arca.Tests
{
	public class StringExtensionsTests
	{
		[Fact]
		public void JoinWithFormat_MultipleTexts_Succeeds()
		{
			var list = new List<string> { "1", "2", "3" };

			var joinedText = list.JoinWithFormat( "{0}", ", " );

			Assert.Equal( "1, 2, 3", joinedText );
		}

		[Fact]
		public void JoinWithFormat_Empty_Succeeds()
		{
			var list = new List<string> { };

			var joinedText = list.JoinWithFormat( "{0}", ", " );

			Assert.Equal( "", joinedText );
		}
	}
}
