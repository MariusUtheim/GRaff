using System.Diagnostics.Contracts;
using System;
#if OpenGL4
using coord = System.Double;
#else
using coord = System.Single;
#endif


namespace GRaff.Graphics
{
	public sealed class Texture
	{
		internal Texture(TextureBuffer buffer, GraphicsPoint topLeft, GraphicsPoint topRight, GraphicsPoint bottomLeft, GraphicsPoint bottomRight)
		{
			Contract.Requires<ArgumentNullException>(buffer != null);
			QuadCoords = new[] { topLeft, topRight, bottomRight, bottomLeft };
			Buffer = buffer;
		}

		public Texture(TextureBuffer buffer, Rectangle region)
			: this(buffer,
				  new GraphicsPoint(region.Left / buffer.Width, region.Top / buffer.Height), 
				  new GraphicsPoint(region.Right / buffer.Width, region.Top / buffer.Height),
				  new GraphicsPoint(region.Left / buffer.Width, region.Bottom / buffer.Height),
				  new GraphicsPoint(region.Right / buffer.Width, region.Bottom / buffer.Height))
		{
			Contract.Requires<ArgumentNullException>(buffer != null);
		}

		public static Texture FromTexCoords(TextureBuffer buffer, Rectangle texCoords)
		{
			Contract.Requires<ArgumentNullException>(buffer != null);
			Contract.Ensures(Contract.Result<Texture>() != null);
			return new Texture(buffer, (GraphicsPoint)texCoords.TopLeft, (GraphicsPoint)texCoords.TopRight, (GraphicsPoint)texCoords.BottomLeft, (GraphicsPoint)texCoords.BottomRight);
		}

		[ContractInvariantMethod]
		private void invariants()
		{
			Contract.Invariant(QuadCoords != null);
			Contract.Invariant(QuadCoords.Length == 4);
			Contract.Invariant(Buffer != null);
		}

		internal GraphicsPoint[] QuadCoords { get; }

        internal GraphicsPoint[] StripCoords => new[] { TopLeft, TopRight, BottomLeft, BottomRight };
		
		public TextureBuffer Buffer { get; }

		public GraphicsPoint TopLeft => QuadCoords[0];
		
		public GraphicsPoint TopRight => QuadCoords[1];

		public GraphicsPoint BottomRight => QuadCoords[2];

		public GraphicsPoint BottomLeft => QuadCoords[3];

		public double Width => (0.5 * (TopLeft + BottomLeft) - 0.5 * (TopRight + BottomRight)).Magnitude * Buffer.Width;

		public double Height => (0.5 * (Point)(TopLeft + TopRight) - 0.5 * (Point)(BottomLeft + BottomRight)).Magnitude * Buffer.Height;

		public Vector Size => new Vector(Width, Height);

		internal coord PixelWidth => Buffer.Width * (BottomRight.Xt - TopLeft.Xt);

		internal coord PixelHeight => Buffer.Height * (BottomRight.Yt - TopLeft.Yt);
	}
}
