using System.Reflection;

namespace Automated.Arca.Manager
{
	internal class CachedAssembly
	{
		public Assembly Assembly { get; }

		public string FullName { get; }
		public bool TypesAndExtensionsAreCached { get; set; }

		public CachedAssembly( Assembly assembly )
		{
			Assembly = assembly;

			FullName = assembly.FullName!;
		}
	}
}
