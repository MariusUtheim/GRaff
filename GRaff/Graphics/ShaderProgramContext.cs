using System;
using System.Diagnostics.Contracts;

namespace GRaff.Graphics
{
	public sealed class ShaderProgramContext : IDisposable
	{
		private int _previous;
		private bool _isDisposed = false;

		public ShaderProgramContext(ShaderProgram program)
		{
			Contract.Requires<ArgumentNullException>(program != null);
			_previous = ShaderProgram.GetCurrentId();
			program.SetCurrent();
		}

		void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				ShaderProgram.SetCurrent(_previous);
			}
		}

		~ShaderProgramContext()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
