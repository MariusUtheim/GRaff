using GRaff.Graphics;
using System.Diagnostics.Contracts;


namespace GRaff
{
#warning Move to GRaff.Graphics?
	public sealed class Texture
	{
		internal Texture(TextureBuffer buffer, PointF topLeft, PointF topRight, PointF bottomLeft, PointF bottomRight)
		{
			Contract.Requires(buffer != null);
			Buffer = buffer;
			TexCoords = new[] { topLeft, topRight, bottomLeft, bottomRight };
		}

		public Texture(TextureBuffer buffer, Rectangle region)
			: this(buffer,
				  new PointF(region.Left / buffer.Width, region.Top / buffer.Height), 
				  new PointF(region.Right / buffer.Width, region.Top / buffer.Height),
				  new PointF(region.Left / buffer.Width, region.Bottom / buffer.Height),
				  new PointF(region.Right / buffer.Width, region.Bottom / buffer.Height))
		{
			Contract.Requires(buffer.IsLoaded);
		}

		internal PointF[] TexCoords { get; private set; }

		public TextureBuffer Buffer { get; private set; }

		public PointF TopLeft => TexCoords[0];
		
		public PointF TopRight => TexCoords[1];

		public PointF BottomRight => TexCoords[3];

		public PointF BottomLeft => TexCoords[2];

		public double Width => (0.5f * (TopLeft + BottomLeft) - 0.5f * (TopRight + BottomRight)).Magnitude * Buffer.Width;

		public double Height => (0.5 * (Point)(TopLeft + TopRight) - 0.5 * (Point)(BottomLeft + BottomRight)).Magnitude * Buffer.Height;

		public Vector Size => new Vector(Width, Height);

		internal float PixelWidth => Buffer.Width * (BottomRight.X - TopLeft.X);

		internal float PixelHeight => Buffer.Height * (BottomRight.Y - TopLeft.Y);
	}
}
