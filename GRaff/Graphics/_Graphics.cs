using System;
using System.Diagnostics;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif

namespace GRaff.Graphics
{
	internal static class _Graphics
	{
		public static void Initialize()
		{
			try
			{
				ColorMap.BlendMode = BlendMode.AlphaBlend;
				GL.Enable(EnableCap.Blend);
				GL.Clear(ClearBufferMask.ColorBufferBit);

				ShaderProgram.CurrentColored = ShaderProgram.DefaultColored;
				ShaderProgram.CurrentTextured = ShaderProgram.DefaultTextured;
				ShaderProgram.CurrentColored.UpdateUniformValues();
				ShaderProgram.CurrentTextured.UpdateUniformValues();

				Framebuffer.ExpectedViewWidth = Window.Width;
				Framebuffer.ExpectedViewHeight = Window.Height;

#if DEBUG
				GlobalEvent.EndStep += () =>
				{
					_Graphics.ErrorCheck();
				};
#endif
			}
			catch (TypeInitializationException ex)
			{
				var innerException = ex.InnerException;
#warning TODO
				throw innerException;
			}
		}

        [Conditional("DEBUG")]
        public static void ErrorCheck()
        {
            var err = GL.GetError();
            if (err != ErrorCode.NoError)
                throw new Exception($"An OpenGL operation threw an exception with error code {Enum.GetName(typeof(ErrorCode), err)}");
        }
    }
}
