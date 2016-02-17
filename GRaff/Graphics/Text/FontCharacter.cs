using System;
using GRaff.Graphics.Text;

namespace GRaff
{
	public class FontCharacter
	{
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

		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int XOffset { get; set; }
		public int YOffset { get; set; }
		public int XAdvance { get; set; }
	}
}