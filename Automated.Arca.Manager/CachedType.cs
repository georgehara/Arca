using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Manager
{
	internal class CachedType
	{
		internal Type Type { get; }

		internal string FullName { get; }
		internal ProcessableAttribute? ProcessableAttribute { get; set; }
		internal bool HasProcessableAttribute => ProcessableAttribute != null;
		internal ProcessingState State { get; set; } = ProcessingState.None;

		internal CachedType( Type type )
		{
			Type = type;

			FullName = type.FullName!;
		}
	}
}
