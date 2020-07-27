using System;
using System.Text;

namespace Automated.Arca.Manager
{
	public class CollectorLogger : IManagerLogger
	{
		private readonly StringBuilder Collector = new StringBuilder();

		public CollectorLogger()
		{
			Collector.AppendLine( $"Created instance of '{nameof( CollectorLogger )}' at {DateTime.Now:s}" );
		}

		public void Log( string message )
		{
			Collector.AppendLine( message );
		}

		public string GetLogs()
		{
			return Collector.ToString();
		}
	}
}
