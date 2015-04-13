using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Graphics
{
	public sealed class BlendMode
	{
		private class BlendModeContext : IDisposable
		{
			private BlendMode _previous;
			private bool _isDisposed = false;

			public BlendModeContext(BlendMode mode)
			{
				_previous = ColorMap.BlendMode;
				ColorMap.BlendMode = mode;
			}
			
			~BlendModeContext()
			{
				throw new InvalidOperationException("A context returned from GRaff.Graphics.Scissor.Use was garbage collected before Dispose was called.");
			}

			public void Dispose()
			{
				if (!_isDisposed)
				{
					GC.SuppressFinalize(this);
					_isDisposed = true;
					ColorMap.BlendMode = _previous;
				}
				else
					throw new InvalidOperationException("Object was already disposed");
			}
		}

		public BlendMode(BlendEquation equation, BlendingFactor sourceFactor, BlendingFactor destinationFactor)
		{
			this.Equation = equation;
			this.SourceFactor = sourceFactor;
			this.DestinationFactor = destinationFactor;
		}

		public static IDisposable Use(BlendMode mode)
		{
			return new BlendModeContext(mode);
		}

		public static BlendMode AlphaBlend { get { return new BlendMode(BlendEquation.Add, BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); } }

		public static BlendMode Additive { get { return new BlendMode(BlendEquation.Add, BlendingFactor.SrcAlpha, BlendingFactor.DstAlpha); } }

		public static BlendMode Subtractive { get { return new BlendMode(BlendEquation.ReverseSubtract, BlendingFactor.SrcAlpha, BlendingFactor.DstAlpha); } }

		public static BlendMode Max { get { return new BlendMode(BlendEquation.Max, BlendingFactor.SrcAlpha, BlendingFactor.DstAlpha); } }

		public BlendingFactor DestinationFactor { get; private set; }

		public BlendEquation Equation { get; private set; }

		public BlendingFactor SourceFactor { get; private set; }

		public override bool Equals(object obj)
		{
			var otherMode = obj as BlendMode;
			if (otherMode == null)
				return false;
			return DestinationFactor == otherMode.DestinationFactor && Equation == otherMode.Equation && SourceFactor == otherMode.SourceFactor;
		}

		public override int GetHashCode()
		{
			return GMath.HashCombine(DestinationFactor.GetHashCode(), SourceFactor.GetHashCode(), Equation.GetHashCode());
		}
	}
}
