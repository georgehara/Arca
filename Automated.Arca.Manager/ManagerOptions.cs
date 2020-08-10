using System;
using System.Collections.Generic;

namespace Automated.Arca.Manager
{
	public class ManagerOptions : IManagerOptions
	{
		public IManagerLogger? Logger { get; private set; }
		public IList<string> ProcessOnlyAssemblyNamesPrefixedWith { get; } = new List<string>();
		public bool ProcessOnlyClassesDerivedFromIProcessable { get; private set; }
		public ICollection<Type> ExcludeTypes { get; private set; } = new HashSet<Type>();

		public ManagerOptions( bool addAutomatedArcaAssemblyNamePrefix = true )
		{
			if( addAutomatedArcaAssemblyNamePrefix )
				AddAssemblyNamePrefix( "Automated.Arca." );
		}

		public IManagerOptions UseLogger( IManagerLogger? logger )
		{
			Logger = logger;

			return this;
		}

		public IManagerOptions AddAssemblyNamePrefix( string assemblyNamePrefix )
		{
			ProcessOnlyAssemblyNamesPrefixedWith.Add( assemblyNamePrefix );

			return this;
		}

		public IManagerOptions UseOnlyClassesDerivedFromIProcessable()
		{
			ProcessOnlyClassesDerivedFromIProcessable = true;

			return this;
		}

		public IManagerOptions Exclude( Type type )
		{
			ExcludeTypes.Add( type );

			return this;
		}

		public IManagerOptions Exclude<T>()
		{
			return Exclude( typeof( T ) );
		}
	}
}
