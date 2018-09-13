using System;
using System.Xml.Serialization;

namespace GRaff
{
	[Serializable]
	public class FontCharacter
	{
        public FontCharacter() { }

		public FontCharacter(char c, int x, int y, int width, int height, int xOffset, int yOffset, int xAdvance)
        {
			this.Id = c;
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.XOffset = xOffset;
            this.YOffset = yOffset;
            this.XAdvance = xAdvance;
        }

		[XmlAttribute("id")]
		public int Id { get; set; }

        public char Char
		{
			get => (char)Id;
		}

		[XmlAttribute("x")]
		public int X { get; internal set; }

		[XmlAttribute("y")]
		public int Y { get; set; }

		public IntVector Location
		{
			get => (X, Y);
		}

		[XmlAttribute("width")]
		public int Width { get; set; }

		[XmlAttribute("height")]
		public int Height { get; set; }

		public IntVector Size
		{
			get => (Width, Height);
		}

		[XmlAttribute("xoffset")]
		public int XOffset { get; set; }

		[XmlAttribute("yoffset")]
		public int YOffset { get; set; }

		public IntVector Offset
		{
			get => (XOffset, YOffset);
		}

		[XmlAttribute("xadvance")]
		public int XAdvance { get; set; }

		[XmlAttribute("page")]
		public int Page { get; set; }

		[XmlAttribute("chnl")]
		public int Channel { get; set; }

		public override string ToString()
		{
			return $"{nameof(FontCharacter)}:{Char}";
		}
	}
}