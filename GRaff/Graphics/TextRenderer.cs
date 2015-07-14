using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Graphics
{
	public class TextRenderer
	{
		private int lengthOfSpace;
		public TextRenderer(Font font)
			: this(font, -1, font.Height)
		{ }

		public TextRenderer(Font font, int width)
			: this(font, width, font.Height)
		{ }

		public TextRenderer(Font font, int width, double lineSeparation)
		{
			this.Font = font;
			this.Width = width;
			this.LineSeparation = lineSeparation;
			lengthOfSpace = GetWidth(" ");
		}

		public Font Font { get; set; }

		public int Width { get; set; }

		public double LineSeparation { get; set; }

		public FontAlignment Alignment { get; set; }

		public string[] LineSplit(string text)
		{
			if (text == null)
				throw new ArgumentNullException();
			if (Width < 0)
				return new[] { text };

			var words = text.Split(' ');
			var lines = new List<string>();
			var currentLine = new StringBuilder(words[0]);
			var currentLineLength = 0;

			for (var i = 1; i < words.Length; i++)
			{
				var wordLength = GetWidth(words[i]);
				if (currentLineLength + wordLength + lengthOfSpace < Width)
				{
					currentLine.Append(" " + words[i]);
					currentLineLength += lengthOfSpace + wordLength;
				}
				else
				{
					lines.Add(currentLine.ToString());

					currentLine = new StringBuilder(words[i]);
					currentLineLength = wordLength;
				}
			}

			lines.Add(currentLine.ToString());

			return lines.ToArray();
		}

		public int GetWidth(string text)
		{
			return Font.GetWidth(text);
		}
		
		public void Draw(string text, Color color, double x, double y)
		{
			var lines = LineSplit(text);
			for (var i = 0; i < lines.Length; i++)
				GRaff.Draw.Text(Font, color, lines[i], x, y + i * LineSeparation);
		}

		public TextureBuffer Render(string text)
		{
			throw new NotImplementedException();
		}
	}
}
