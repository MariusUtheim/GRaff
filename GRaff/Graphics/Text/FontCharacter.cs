using System;
using GRaff.Graphics.Text;

namespace GRaff
{
	public class FontCharacter
	{
		public FontCharacter(int x, int y, int width, int height, int xOffset, int yOffset, int xAdvance)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
			this.XOffset = xOffset;
			this.YOffset = yOffset;
			this.XAdvance = xAdvance;
		}

		public FontCharacter(FontChar c)
		{
			this.X = c.X;
			this.Y = c.Y;
			this.Width = c.Width;
			this.Height = c.Height;
			this.XOffset = c.XOffset;
			this.YOffset = c.YOffset;
			this.XAdvance = c.XAdvance;
		}

		public int X { get; private set; }
		public int Y { get; private set; }
		public IntVector Location => new IntVector(X, Y);
		public int Width { get; private set; }
		public int Height { get; private set; }
		public IntVector Size => new IntVector(Width, Height);
		public int XOffset { get; private set; }
		public int YOffset { get; private set; }
		public IntVector Offset => new IntVector(XOffset, YOffset);
		public int XAdvance { get; private set; }
	}
}