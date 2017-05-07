using System;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif

namespace GRaff.Graphics
{
	internal static class _Initializer
	{
		public static void Initialize()
		{
			try
			{
				ColorMap.BlendMode = BlendMode.AlphaBlend;
				GL.Enable(EnableCap.Blend);
				GlobalEvent.EndStep += () =>
				{
					var err = GL.GetError();
					if (err != ErrorCode.NoError)
						throw new Exception($"A GL error occurred: {Enum.GetName(err.GetType(), err)}");
				};
            }
			catch (TypeInitializationException ex)
			{
				var innerException = ex.InnerException;
#warning TODO
				throw innerException;
			}
		}
	}
}
