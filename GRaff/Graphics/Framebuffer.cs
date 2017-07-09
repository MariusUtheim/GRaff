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
	public sealed class Framebuffer : IDisposable
	{
		internal static int ExpectedViewWidth, ExpectedViewHeight;
        internal static IntVector ExpectedViewSize => (ExpectedViewWidth, ExpectedViewHeight);

        public Framebuffer(IntVector size)
            : this(size.X, size.Y) { }

		public Framebuffer(int width, int height)
		{
			Id = GL.GenFramebuffer();
			Texture = new Texture(width, height, IntPtr.Zero);

            var previous = Current;

            try
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, Texture.Id, 0);
                GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
                if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                    throw new InvalidOperationException("Unhandled exception in framebuffer");
            }
            finally
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, previous?.Id ?? 0);
            }
            
		}

		public static Framebuffer Current { get; private set; }

        public static void BindDefault()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public int Id { get; }

        public Texture Texture { get; private set; }

        public IDisposable Use()
        {
            return UseContext.CreateAt($"{typeof(Framebuffer).FullName}.{nameof(Use)}",
                                       (frameBuffer: Framebuffer.Current, viewContext: View.Rectangle((Point.Zero, ExpectedViewSize)).Use()),
                () => GL.BindFramebuffer(FramebufferTarget.Framebuffer, this.Id),
                previous =>
                {
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, previous.frameBuffer?.Id ?? 0);
                    previous.viewContext.Dispose();
                });
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
        }

        //TODO// Blitting

        #region IDisposable Support
        private bool _isDisposed = false;

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                Async.Capture(Id).ThenQueue(id =>
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


    }
}
