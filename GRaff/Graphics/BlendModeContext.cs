using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Graphics
{
	public sealed class BlendModeContext : IDisposable
	{
		private BlendMode _previous, _current;
		private bool _isDisposed = false;

		public BlendModeContext(BlendMode mode) /*C#6.0*/
		{
			_previous = ColorMap.BlendMode;
			_current = ColorMap.BlendMode = mode;
		}

		void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				ColorMap.BlendMode = _previous;
				_previous = _current = null;
			}
		}

		~BlendModeContext() {
		  Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
