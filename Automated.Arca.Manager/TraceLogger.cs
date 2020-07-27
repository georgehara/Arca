using System.Diagnostics;

namespace Automated.Arca.Manager
{
	public class TraceLogger : IManagerLogger
	{
		public TraceLogger()
		{
		}

		public void Log( string message )
		{
			Trace.WriteLine( message, "Automated.Arca" );
		}
	}
}
