﻿using System;
using System.Collections.Generic;

namespace Automated.Arca.Manager
{
	public interface IManagerOptions
	{
		IManagerLogger? Logger { get; }
		IList<string> ProcessOnlyAssemblyNamesPrefixedWith { get; }
		IList<string> ExcludedAssemblyNames { get; }
		bool ProcessOnlyClassesDerivedFromIProcessable { get; }
		ICollection<Type> ExcludedTypes { get; }
		IOrderedTypes PriorityTypes { get; }

		IManagerOptions UseLogger( IManagerLogger? logger );
		IManagerOptions AddAssemblyNamePrefix( string assemblyNamePrefix );
		IManagerOptions ExcludeAssemblyName( string assemblyName );
		IManagerOptions UseOnlyClassesDerivedFromIProcessable();
		IManagerOptions Exclude( Type type );
		IManagerOptions Exclude<T>();
		IManagerOptions Prioritize( Type type );
		IManagerOptions Prioritize<T>();
	}
}
