using System;
using GRaff.Graphics.Text;

namespace GRaff
{
	public class FontCharacter
	{
		public FontCharacter(char c, int x, int y, int width, int height, int xOffset, int yOffset, int xAdvance)
		{
            this.Character = c;
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
			this.XOffset = xOffset;
			this.YOffset = yOffset;
			this.XAdvance = xAdvance;
		}

		public FontCharacter(FontChar fontChar)
		{
            this.Character = (char)fontChar.Id;
            this.X = fontChar.X;
			this.Y = fontChar.Y;
			this.Width = fontChar.Width;
			this.Height = fontChar.Height;
			this.XOffset = fontChar.XOffset;
			this.YOffset = fontChar.YOffset;
			this.XAdvance = fontChar.XAdvance;
		}

        public char Character { get; }
		public int X { get; }
		public int Y { get; }
		public IntVector Location => new IntVector(X, Y);
		public int Width { get; }
		public int Height { get; }
		public IntVector Size => new IntVector(Width, Height);
		public int XOffset { get; }
		public int YOffset { get; }
		public IntVector Offset => new IntVector(XOffset, YOffset);
		public int XAdvance { get; }

        public override string ToString()
        {
            return $"FontChar {Character}";
        }
    }
}