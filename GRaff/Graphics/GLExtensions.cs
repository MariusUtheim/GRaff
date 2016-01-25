using OpenTK.Graphics.ES30;

namespace GRaff.Graphics
{
	public static class GLExtensions
	{
		public static PointF[] QuadCoordinates(this Rectangle rect)
		{
			return new[] {
				new PointF((float)rect.Left, (float)rect.Top),
				new PointF((float)rect.Right, (float)rect.Top),
				new PointF((float)rect.Right, (float)rect.Bottom),
				new PointF((float)rect.Left, (float)rect.Bottom)
			};
		}

		public static void Bind(this TextureBuffer texture)
		{
			GL.BindTexture(TextureTarget.Texture2D, texture.Id);
		}

		public static void Bind(this Texture texture)
		{
			GL.BindTexture(TextureTarget.Texture2D, texture.Buffer.Id);
		}
	}
}
