using GRaff.Graphics;
using System.Diagnostics.Contracts;


namespace GRaff
{
#warning Move to GRaff.Graphics?
	public sealed class Texture
	{
		public Texture(TextureBuffer buffer, float left, float bottom, float right, float top)
		{
			Contract.Requires(buffer != null);
			Buffer = buffer;
			TopLeft = new PointF(left, top);
			TopRight = new PointF(right, top);
			BottomLeft = new PointF(left, bottom);
			BottomRight = new PointF(right, bottom);
		}

		public int Id { get { return Buffer.Id; } }

		public TextureBuffer Buffer { get; private set; }

		public PointF TopLeft { get; private set; }
		
		public PointF TopRight { get; private set; }

		public PointF BottomLeft { get; private set; }

		public PointF BottomRight { get; private set; }

		public float PixelWidth
			=> Buffer.Width * (BottomRight.X - TopLeft.X);

		public float PixelHeight
			=> Buffer.Height * (BottomRight.Y + TopLeft.Y);
	}
}
