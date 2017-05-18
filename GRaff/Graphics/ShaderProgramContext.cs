using System;
using System.Diagnostics.Contracts;

namespace GRaff.Graphics
{
	public sealed class ShaderProgramContext : IDisposable
	{
		private ShaderProgram _previous;
		private bool _isDisposed = false;

		public ShaderProgramContext(ShaderProgram program)
		{
			Contract.Requires<ArgumentNullException>(program != null);
			_previous = ShaderProgram.Current;
			program.SetCurrent();
		}

		void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_previous.SetCurrent();
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
