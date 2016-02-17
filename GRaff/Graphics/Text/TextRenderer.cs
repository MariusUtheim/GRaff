using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GRaff.Graphics.Text
{
	public sealed class TextRenderer
	{
		private static readonly Regex NewlineRegex = new Regex("\r\n|\n");
		
		public TextRenderer(Font font, FontAlignment alignment = FontAlignment.TopLeft, int width = Int32.MaxValue)
			: this(font, alignment, width, font.Height)
		{
			Contract.Requires<ArgumentNullException>(font != null);
		}

		public TextRenderer(Font font, FontAlignment alignment, int width, double lineSeparation)
		{
			Contract.Requires<ArgumentNullException>(font != null);
			this.Font = font;
			this.Alignment = alignment;
			this.Width = width;
			this.LineSeparation = lineSeparation;
		}

		[ContractInvariantMethod]
		private void objectInvariants()
		{
			Contract.Invariant(Font != null);
		}

		public Font Font { get; set; }

		public int Width { get; set; }

		public double LineSeparation { get; set; }

		public FontAlignment Alignment { get; set; } = FontAlignment.TopLeft;

		public int LengthOfSpace => GetWidth(" ");

		private string _multilineFormat(string text)
		{
			Contract.Assume(text != null);
			var words = text.Split(' ');
			var multilineFormat = new StringBuilder(text.Length);
			var currentLine = new StringBuilder(words[0]);
			var currentLineLength = GetWidth(words[0]);
			var lengthOfSpace = LengthOfSpace;

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
					multilineFormat.AppendLine(currentLine.ToString());

					currentLine = new StringBuilder(words[i]);
					currentLineLength = wordLength;
				}
			}

			multilineFormat.AppendLine(currentLine.ToString());

			var result = multilineFormat.ToString().TrimEnd(Environment.NewLine.ToCharArray());
			return result;
		}

		public string MultilineFormat(string text)
		{
			return text == null ? "" : String.Concat(Regex.Split(text, Environment.NewLine).Select(str => _multilineFormat(str)));
		}

		public string[] LineSplit(string text)
		{
			return text == null ? new string[0] : NewlineRegex.Split(text).Select(str => _multilineFormat(str)).SelectMany(str => NewlineRegex.Split(str)).ToArray();
		}

		public string Truncate(string text)
		{
			if (text == null)
				return "";

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


		internal string[] RenderCoords(string text, out GraphicsPoint[] quadCoords)
		{
			var lines = LineSplit(text);
			var length = lines.Sum(line => line.Length);

			quadCoords = new GraphicsPoint[4 * length];

			var x0 = 0.0;
			var y0 = 0.0;
			
			switch (Alignment & FontAlignment.Vertical)
			{
				case FontAlignment.Top: y0 = 0; break;
				case FontAlignment.Center: y0 = -(LineSeparation * (lines.Length - 1) + Font.Height) / 2; break;
				case FontAlignment.Bottom: y0 = -(LineSeparation * (lines.Length - 1) + Font.Height); break;
			}

			var coordIndex = 0;
			for (var l = 0; l < lines.Length; l++)
			{
				var lineWidth = Font.GetWidth(lines[l]);
				switch (Alignment & FontAlignment.Horizontal)
				{
					case FontAlignment.Left: x0 = 0; break;
					case FontAlignment.Center: x0 = -lineWidth / 2f; break;
					case FontAlignment.Right: x0 = -lineWidth; break;
				}

				var x = x0;
				var y = y0 + l * LineSeparation;
				for (var i = 0; i < lines[l].Length; i++)
				{
					var s = Font.GetSize(lines[l][i]);
					var d = Font.GetOffset(lines[l][i]);
					quadCoords[coordIndex] = new GraphicsPoint(x + d.X, y + d.Y);
					quadCoords[coordIndex + 1] = new GraphicsPoint(x + d.X + s.X, y + d.Y);
					quadCoords[coordIndex + 2] = new GraphicsPoint(x + d.X + s.X, y + d.Y + s.Y);
					quadCoords[coordIndex + 3] = new GraphicsPoint(x + d.X, y + d.Y + s.Y);
					x += s.X;
					coordIndex += 4;
				}
			}

			return lines;
		}


		public TextureBuffer Render(string text)
		{
			throw new NotImplementedException();
		}
	}
}
