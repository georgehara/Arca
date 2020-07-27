using System.Collections.Generic;

namespace Automated.Arca.Manager
{
	public class ManagerOptions
	{
		public IManagerLogger? Logger { get; private set; }
		public IList<string> ProcessOnlyAssemblyNamesPrefixedWith { get; } = new List<string>();
		public bool ProcessOnlyClassesDerivedFromIProcessable { get; private set; }

		public ManagerOptions( bool addAutomatedArcaAssemblyNamePrefix = true )
		{
			if( addAutomatedArcaAssemblyNamePrefix )
				AddAssemblyNamePrefix( "Automated.Arca." );
		}

		public ManagerOptions UseLogger( IManagerLogger? logger )
		{
			Logger = logger;

			return this;
		}

		public ManagerOptions AddAssemblyNamePrefix( string assemblyNamePrefix )
		{
			ProcessOnlyAssemblyNamesPrefixedWith.Add( assemblyNamePrefix );

			return this;
		}

		public ManagerOptions UseOnlyClassesDerivedFromIProcessable()
		{
			ProcessOnlyClassesDerivedFromIProcessable = true;

			return this;
		}
	}
}
