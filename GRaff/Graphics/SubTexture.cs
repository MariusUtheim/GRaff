using System.Diagnostics.Contracts;
using System;
using coord = System.Double;


namespace GRaff.Graphics
{
	public sealed class SubTexture
	{
        private static readonly GraphicsPoint defaultTL = new GraphicsPoint(0, 0);
        private static readonly GraphicsPoint defaultTR = new GraphicsPoint(1, 0);
        private static readonly GraphicsPoint defaultBL = new GraphicsPoint(0, 1);
        private static readonly GraphicsPoint defaultBR = new GraphicsPoint(1, 1);

		internal SubTexture(Texture buffer, GraphicsPoint topLeft, GraphicsPoint topRight, GraphicsPoint bottomLeft, GraphicsPoint bottomRight)
		{
			Contract.Requires<ArgumentNullException>(buffer != null);
			TriangleStripCoords = new[] { topLeft, topRight, bottomLeft, bottomRight };
			Texture = buffer;
		}

        public SubTexture(Texture buffer)
            : this(buffer, defaultTL, defaultTR, defaultBL, defaultBR)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
        }

		public SubTexture(Texture buffer, Rectangle region)
			: this(buffer,
				  new GraphicsPoint(region.Left / buffer.Width, region.Top / buffer.Height), 
				  new GraphicsPoint(region.Right / buffer.Width, region.Top / buffer.Height),
				  new GraphicsPoint(region.Left / buffer.Width, region.Bottom / buffer.Height),
				  new GraphicsPoint(region.Right / buffer.Width, region.Bottom / buffer.Height))
		{
			Contract.Requires<ArgumentNullException>(buffer != null);
		}

		public static SubTexture FromTexCoords(Texture buffer, Rectangle texCoords)
		{
			Contract.Requires<ArgumentNullException>(buffer != null);
			Contract.Ensures(Contract.Result<SubTexture>() != null);
			return new SubTexture(buffer, (GraphicsPoint)texCoords.TopLeft, (GraphicsPoint)texCoords.TopRight, (GraphicsPoint)texCoords.BottomLeft, (GraphicsPoint)texCoords.BottomRight);
		}

		[ContractInvariantMethod]
		private void invariants()
		{
			Contract.Invariant(TriangleStripCoords != null);
			Contract.Invariant(TriangleStripCoords.Length == 4);
			Contract.Invariant(Texture != null);
		}

        internal GraphicsPoint[] TriangleStripCoords { get; private set; }
		
		public Texture Texture { get; }

		public GraphicsPoint TopLeft => TriangleStripCoords[0];
		
		public GraphicsPoint TopRight => TriangleStripCoords[1];

		public GraphicsPoint BottomRight => TriangleStripCoords[3];

		public GraphicsPoint BottomLeft => TriangleStripCoords[2];

		public double Width => (0.5 * (TopLeft + BottomLeft) - 0.5 * (TopRight + BottomRight)).Magnitude * Texture.Width;

		public double Height => (0.5 * (Point)(TopLeft + TopRight) - 0.5 * (Point)(BottomLeft + BottomRight)).Magnitude * Texture.Height;

		public Vector Size => new Vector(Width, Height);

		internal coord PixelWidth => Texture.Width * (BottomRight.Xt - TopLeft.Xt);

		internal coord PixelHeight => Texture.Height * (BottomRight.Yt - TopLeft.Yt);
	}
}
