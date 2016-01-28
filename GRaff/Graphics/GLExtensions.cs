using System;
using System.Diagnostics.Contracts;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
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

		public static GraphicsPoint[] ToTriangleStrip(this Polygon polygon)
		{
			Contract.Requires<ArgumentNullException>(polygon != null);
			var result = new GraphicsPoint[polygon.Length];
			for (int i = 0, sign = 1; i < polygon.Length; i++, sign = -sign)
				result[i] = (GraphicsPoint)polygon.Vertex(i * sign);
			return result;
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
