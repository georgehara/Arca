using System;

namespace Automated.Arca.Libraries
{
	public interface IDisposableObject : IDisposable
	{
		void Disposing();
	}
}
