using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GRaff.Synchronization;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics
{
	public sealed class Framebuffer : IDisposable
	{

        public Framebuffer(Texture texture)
		{
			Id = GL.GenFramebuffer();
			this.Texture = texture;

            var previous = Current;

            try
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
                GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, Texture.Id, 0);
                GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
                if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                    throw new GraphicsException("Unhandled exception in framebuffer");
            }
            finally
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, previous?.Id ?? 0);
            }         
		}

        public Framebuffer(IntVector size)
			: this(new Texture(size.X, size.Y, IntPtr.Zero)) { }

		public Framebuffer(int width, int height)
			: this(new Texture(width, height, IntPtr.Zero)) { }

        internal static void InitializeFramebufferDimensions(int width, int height)
        {
            ViewWidth = width;
            ViewHeight = height;
        }

        internal static int ViewWidth { get; private set; }
        internal static int ViewHeight { get; private set; }



        private static Color[,] _fill(int width, int height, Color clearColor)
		{
			var result = new Color[height, width];
			for (var y = 0; y < height; y++)
				for (var x = 0; x < width; x++)
					result[y, x] = clearColor;
			return result;
		}

        public Framebuffer(int width, int height, Color clearColor)
			: this(new Texture(_fill(width, height, clearColor))) { }


		public static Framebuffer Current { get; private set; }

        public static void BindDefault()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public static Framebuffer CopyFromScreen()
        {
            var previous = Current;
            var buffer = new Framebuffer(Window.Size);

            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, 0);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, buffer.Id);
            GL.BlitFramebuffer(0, 0, ViewWidth, ViewHeight, 0, Window.Height, Window.Width, 0, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);

            if (previous == null)
                BindDefault();
            else
                previous.Bind();
            _Graphics.ErrorCheck();
            return buffer;
        }


        public int Id { get; }

        public Texture Texture { get; private set; }

        public IDisposable Use()
        {
			return UseContext.CreateAt($"{typeof(Framebuffer).FullName}.{nameof(Use)}",
                                       (frameBuffer: Framebuffer.Current, 
			                            viewContext: View.Framebuffer().Use()),
                () => GL.BindFramebuffer(FramebufferTarget.Framebuffer, this.Id),
                capture =>
                {
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, capture.frameBuffer?.Id ?? 0);
                    capture.viewContext.Dispose();
                });
        }

        public void Bind()
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
            Current = this;
        }

        //TODO// Blitting

        #region IDisposable implementation 

        public bool IsDisposed { get; private set; }

        ~Framebuffer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                Async.Capture(Id).ThenQueue(id =>
                {
                    if (_Graphics.IsContextActive)
                    {
                        GL.DeleteFramebuffer(id);
                        _Graphics.ErrorCheck();
                    }
                });

                IsDisposed = true;
            }
        }

        #endregion


    }
}
