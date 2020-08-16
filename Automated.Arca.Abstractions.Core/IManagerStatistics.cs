namespace Automated.Arca.Abstractions.Core
{
	public interface IManagerStatistics
	{
		int LoadedAssemblies { get; set; }
		long AssemblyLoadingTime { get; set; }
		long CachingTime { get; set; }
		int CachedTypes { get; set; }
		int RegisteredClasses { get; set; }
		long ClassRegistrationTime { get; set; }
		long ClassConfigurationTime { get; set; }
	}
}
