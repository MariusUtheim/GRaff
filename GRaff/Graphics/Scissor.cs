using System;
using GRaff.Synchronization;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics
{
	/// <summary>
	/// Provides static methods for performing scissor testing. When scissor testing is active,
	/// pixels outside a specified region will not be drawn.
	/// </summary>
	public static class Scissor
	{
		private class ScissorContext : IDisposable
		{
			private IntRectangle _previous;
			private bool _wasEnabled;
			private bool _isDisposed = false;

			public ScissorContext(IntRectangle region)
			{
				this._wasEnabled = Scissor.IsEnabled;
				this._previous = Scissor.Region;
				Scissor.Region = region;
				Scissor.IsEnabled = true;
			}

			~ScissorContext()
			{
				Async.ThrowException(new InvalidOperationException("A context returned from GRaff.Graphics.Scissor.Context was garbage collected before Dispose was called."));
			}

			public void Dispose()
			{
				if (!_isDisposed)
				{
					GC.SuppressFinalize(this);
					_isDisposed = true;
					Scissor.Region = _previous;
					Scissor.IsEnabled = _wasEnabled;
				}
				else
					throw new ObjectDisposedException("Scissor");
			}
		}

		public static IDisposable Use(IntRectangle region)
		{
			return new ScissorContext(region);
		}

		public static IDisposable UseIntersection(IntRectangle region)
		{
			if (IsEnabled)
				return Use(region.Intersection(Region) ?? IntRectangle.Zero);
			else
				return Use(region);
		}

		public static bool IsEnabled
		{
			get
			{
				return GL.GetBoolean(GetPName.ScissorTest);
			}

			set
			{
				if (value)
					GL.Enable(EnableCap.ScissorTest);
				else
					GL.Disable(EnableCap.ScissorTest);
			}
		}

		public static IntRectangle Region
		{
			get
			{
				int[] scissorCoords = new int[4];
				GL.GetInteger(GetPName.ScissorBox, scissorCoords);
				return new IntRectangle(scissorCoords[0], Window.Height - scissorCoords[1] - scissorCoords[3], scissorCoords[2], scissorCoords[3]);
			}

			set
			{
				GL.Scissor(value.Left, Window.Height - value.Bottom, value.Width, value.Height);
            }
		}
	}
}
