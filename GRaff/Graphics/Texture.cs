using System.Diagnostics.Contracts;
using System;
#if OpenGL4
using coord = System.Double;
#else
using coord = System.Single;
#endif


namespace GRaff.Graphics
{
#warning Move to GRaff.Graphics?
	public sealed class Texture
	{
		internal Texture(TextureBuffer buffer, GraphicsPoint topLeft, GraphicsPoint topRight, GraphicsPoint bottomLeft, GraphicsPoint bottomRight)
		{
			Contract.Requires<ArgumentNullException>(buffer != null);
			Buffer = buffer;
			TexCoords = new[] { topLeft, topRight, bottomLeft, bottomRight };
		}

		public Texture(TextureBuffer buffer, Rectangle region)
			: this(buffer,
				  new GraphicsPoint(region.Left / buffer.Width, region.Top / buffer.Height), 
				  new GraphicsPoint(region.Right / buffer.Width, region.Top / buffer.Height),
				  new GraphicsPoint(region.Left / buffer.Width, region.Bottom / buffer.Height),
				  new GraphicsPoint(region.Right / buffer.Width, region.Bottom / buffer.Height))
		{
			Contract.Requires<ArgumentNullException>(buffer != null);
			Contract.Requires<ArgumentException>(buffer.IsLoaded);
		}

		[ContractInvariantMethod]
		private void objectInvariants()
		{
			Contract.Invariant(TexCoords != null);
			Contract.Invariant(TexCoords.Length == 4);
		}

		internal GraphicsPoint[] TexCoords { get; private set; }

		public TextureBuffer Buffer { get; private set; }

		public GraphicsPoint TopLeft => TexCoords[0];
		
		public GraphicsPoint TopRight => TexCoords[1];

		public GraphicsPoint BottomRight => TexCoords[3];

		public GraphicsPoint BottomLeft => TexCoords[2];

		public double Width => (0.5 * (TopLeft + BottomLeft) - 0.5 * (TopRight + BottomRight)).Magnitude * Buffer.Width;

		public double Height => (0.5 * (Point)(TopLeft + TopRight) - 0.5 * (Point)(BottomLeft + BottomRight)).Magnitude * Buffer.Height;

		public Vector Size => new Vector(Width, Height);

		internal coord PixelWidth => Buffer.Width * (BottomRight.Xt - TopLeft.Xt);

		internal coord PixelHeight => Buffer.Height * (BottomRight.Yt - TopLeft.Yt);
	}
}
