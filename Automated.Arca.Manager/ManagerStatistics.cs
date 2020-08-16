using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Manager
{
	public class ManagerStatistics : IManagerStatistics
	{
		public int LoadedAssemblies { get; set; }
		public long AssemblyLoadingTime { get; set; }
		public long CachingTime { get; set; }
		public int CachedTypes { get; set; }
		public int RegisteredClasses { get; set; }
		public long ClassRegistrationTime { get; set; }
		public long ClassConfigurationTime { get; set; }
	}
}
