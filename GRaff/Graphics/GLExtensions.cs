#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using System;
using System.Diagnostics.Contracts;
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics
{
	public static class GLExtensions
	{
		public static GraphicsPoint[] QuadCoordinates(this Rectangle rect)
		{
			return new[] {
				new GraphicsPoint(rect.Left, rect.Top),
				new GraphicsPoint(rect.Right, rect.Top),
				new GraphicsPoint(rect.Right, rect.Bottom),
				new GraphicsPoint(rect.Left, rect.Bottom)
			};
		}

		public static void Bind(this TextureBuffer texture)
		{
			Contract.Requires<ArgumentNullException>(texture != null);
			GL.BindTexture(TextureTarget.Texture2D, texture.Id);
		}

		public static void Bind(this Texture texture)
		{
			Contract.Requires<ArgumentNullException>(texture != null);
			GL.BindTexture(TextureTarget.Texture2D, texture.Buffer.Id);
		}
	}
}
