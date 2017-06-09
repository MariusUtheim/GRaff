using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GRaff.Synchronization;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics
{
#warning Needs cleaning
	public sealed class Framebuffer : IDisposable
	{
		private int _id;
		internal static int ExpectedViewWidth, ExpectedViewHeight;

		public Framebuffer(int width, int height)
		{
			_id = GL.GenFramebuffer();
			Buffer = new TextureBuffer(width, height, IntPtr.Zero);
			
			using (new FramebufferBindContext(this, false))
			{
				Buffer.Bind();
				GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, Buffer.Id, 0);
				GL.DrawBuffer(DrawBufferMode.ColorAttachment0);

				if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
					throw new Exception("Unhandled exception in framebuffer");
			}

		}

		public static Framebuffer CurrentTarget { get; private set; }

		#region IDisposable Support
		private bool _isDisposed = false; // To detect redundant calls

		private void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				Console.WriteLine("[Framebuffer] Disposed");
				Async.Capture(_id).ThenQueue(id =>
				{
                    if (Giraffe.IsRunning)
                    {
                        GL.DeleteFramebuffer(id);
                        _Graphics.ErrorCheck();
                    }
				});

				_isDisposed = true;
			}
		}

		~Framebuffer()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion


		private class FramebufferBindContext : IDisposable
		{
			private Framebuffer _previous;
			private bool _isDisposed = false;
			private IDisposable _viewContext;

			public FramebufferBindContext(Framebuffer current, bool setView)
			{
				this._previous = Framebuffer.CurrentTarget;
				GL.BindFramebuffer(FramebufferTarget.Framebuffer, current._id);

				if (setView)
					_viewContext = View.UseView(ExpectedViewWidth / 2, ExpectedViewHeight / 2, ExpectedViewWidth, -ExpectedViewHeight);
			}

			~FramebufferBindContext()
			{
				Async.Throw(new ObjectDisposedIncorrectlyException($"A context returned from {nameof(GRaff.Graphics.Framebuffer.Bind)} was garbage collected before Dispose was called."));
            }

			public void Dispose()
			{
				if (!_isDisposed)
				{
					GC.SuppressFinalize(this);
					_isDisposed = true;
					GL.BindFramebuffer(FramebufferTarget.Framebuffer, _previous?._id ?? 0);

					if (_previous != null)
						GL.DrawBuffer(DrawBufferMode.Back);

					_viewContext?.Dispose();
				}
				else
					throw new ObjectDisposedException(nameof(Framebuffer));
			}

		}
		
		public IDisposable Bind()
		{
			return new FramebufferBindContext(this, true);
		}

		public TextureBuffer Buffer { get; private set; }
	}
}
