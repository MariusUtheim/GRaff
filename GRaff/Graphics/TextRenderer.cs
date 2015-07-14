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
			: this(font, Int32.MaxValue, font.Height)
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

		public int MaxLines { get; set; }

		public double LineSeparation { get; set; }

		public FontAlignment Alignment { get; set; }

		public string[] LineSplit(string text)
		{
			if (text == null)
				throw new ArgumentNullException();

			var words = text.Split(' ');
			var lines = new List<string>();
			var currentLine = new StringBuilder(words[0]);
			var currentLineLength = GetWidth(words[0]);

			var lengths = words.Select(word => GetWidth(word));

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

					Debug.Assert(GetWidth(currentLine.ToString()) <= Width);

					currentLine = new StringBuilder(words[i]);
					currentLineLength = wordLength;
				}
			}

			lines.Add(currentLine.ToString());

			return lines.ToArray();
		}

		public string Truncate(string text)
		{
			var lowerBound = Width - 3 * Font.GetWidth(".");
			var currentWidth = 0;

			for (var i = 0; i < text.Length; i++)
			{
				var nextWidth = Font.GetWidth(text[i]);
				currentWidth += nextWidth;
				if (currentWidth > lowerBound)
				{
					for (var j = i + 1; j < text.Length; j++)
					{
						currentWidth += Font.GetWidth(text[j]);
						if (currentWidth > Width)
							return text.Substring(0, i) + "...";
                    }
					break;
				}
			}

			return text;
		}

		public int GetWidth(string text)
		{
			return Font.GetWidth(text);
		}
		

		public void Draw(string text, Color color, Point location)
		{
			Draw(text, color, location.X, location.Y);
		}

		public void Draw(string text, Color color, double x, double y)
		{
			var lines = LineSplit(text);

			if ((Alignment & FontAlignment.Vertical) == FontAlignment.Center)
				y -= (lines.Length * LineSeparation - Font.Height) / 2;
			else if ((Alignment & FontAlignment.Vertical) == FontAlignment.Bottom)
				y -= lines.Length * LineSeparation - Font.Height;
			for (var i = 0; i < lines.Length; i++)
				GRaff.Draw.Text(Font, Alignment, color, lines[i], x, y + i * LineSeparation);
		}

		public TextureBuffer Render(string text)
		{
			throw new NotImplementedException();
		}
	}
}
