using System;
using Automated.Arca.Abstractions.Core;

namespace Automated.Arca.Manager
{
	internal class CachedType
	{
		internal Type Type { get; }
		internal string FullName { get; }

		internal ProcessingState State { get; set; } = ProcessingState.None;
		internal ProcessableAttribute[]? ProcessableAttributes { get; set; }
		internal bool HasProcessableAttributes => ProcessableAttributes != null;
		internal IRegistratorConfigurator? RegistratorConfigurator { get; set; }

		internal CachedType( Type type )
		{
			Type = type;

			FullName = type.FullName!;
		}
	}
}
