using System;

namespace Automated.Arca.Libraries
{
	public abstract class DisposableObject : IDisposableObject
	{
		private bool IsDisposed;

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		private void Dispose( bool disposing )
		{
			if( IsDisposed )
				return;

			if( disposing )
			{
				Disposing();
			}

			IsDisposed = true;
		}

		public abstract void Disposing();
	}
}
