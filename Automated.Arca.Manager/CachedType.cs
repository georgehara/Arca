using System;

namespace Automated.Arca.Manager
{
	internal class CachedType
	{
		internal Type Type { get; }

		internal string FullName { get; }
		internal ProcessingState State { get; set; } = ProcessingState.None;

		internal CachedType( Type type )
		{
			Type = type;

			FullName = type.FullName!;
		}
	}
}
