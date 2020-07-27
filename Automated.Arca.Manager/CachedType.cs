using System;

namespace Automated.Arca.Manager
{
	internal class CachedType
	{
		public Type Type { get; }

		public string FullName { get; }
		public ProcessingState State { get; set; } = ProcessingState.None;

		public CachedType( Type type )
		{
			Type = type;

			FullName = type.FullName!;
		}
	}
}
