using System;
using System.Diagnostics;
using OpenTK.Graphics;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif

namespace GRaff.Graphics
{
	internal static class _Graphics
	{
		public static bool IsContextActive
		{
			get { return GraphicsContext.CurrentContext != null; }
		}

        public static void Initialize()
        {
            Draw.Device = new RenderDevice();
            View.UpdateGLToScreenMatrix();

            BlendMode.Current = BlendMode.AlphaBlend;
            GL.Enable(EnableCap.Blend);

            ShaderProgram.Default.Bind();

            Framebuffer.ExpectedViewWidth = (int)(Window.Width * Window.DisplayScale.X);
            Framebuffer.ExpectedViewHeight = (int)(Window.Height * Window.DisplayScale.Y);

#if DEBUG
            GlobalEvent.EndStep += () =>
            {
                _Graphics.ErrorCheck();
            };
#endif
        }

        [Conditional("DEBUG")]
        public static void ErrorCheck()
        {
            var err = GL.GetError();
            if (err != ErrorCode.NoError)
               throw new Exception($"An OpenGL operation threw an exception with error code {Enum.GetName(typeof(ErrorCode), err)}");
        }

        public static bool ClearError()
        {
            return (GL.GetError() != ErrorCode.NoError);
        }
    }
}
