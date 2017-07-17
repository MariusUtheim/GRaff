using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GRaff.Graphics.Text
{
#warning Needs cleaning
	public sealed class TextRenderer
	{
		private static readonly Regex NewlineRegex = new Regex("\r\n|\n");
		private static readonly Regex WhitespaceRegex = new Regex(" ");
		
		public TextRenderer(Font font, Alignment alignment = Alignment.TopLeft, int? lineWidth = null, double lineSeparation = 0)
		{
			Contract.Requires<ArgumentNullException>(font != null);
			this.Font = font;
			this.Alignment = alignment;
			this.LineWidth = lineWidth;
			this.LineSeparation = lineSeparation;
		}

		[ContractInvariantMethod]
		private void objectInvariants()
		{
			Contract.Invariant(Font != null);
		}

		private static bool _isNewline(char c)
		{
			return c == '\r' || c == '\n';
		}

		public Font Font { get; set; }

		public int? LineWidth { get; set; }

		public double LineSeparation { get; set; }

		public Alignment Alignment { get; set; } = Alignment.TopLeft;

		private IEnumerable<string> _breakString(string text)
		{
			Contract.Assume(text != null);
			if (LineWidth == null)
			{
				yield return text;
				yield break;
			}

			var words = WhitespaceRegex.Split(text);
			var lengthOfSpace = Font.GetWidth(" ");

			var currentLine = new StringBuilder(words[0]);
			var currentLineLength = Font.GetWidth(words[0]);

			var lengths = words.Select(word => Font.GetWidth(word));

			for (var i = 1; i < words.Length; i++)
			{
				var wordLength = Font.GetWidth(words[i]);
				if (LineWidth == null || currentLineLength + wordLength < LineWidth)
				{
					currentLine.Append(" " + words[i]);
					currentLineLength += lengthOfSpace + wordLength;
				}
				else
				{
					yield return currentLine.ToString();

					currentLine = new StringBuilder(words[i]);
					currentLineLength = wordLength;
				}
			}

			yield return currentLine.ToString().TrimEnd(Environment.NewLine.ToCharArray());

//			multilineFormat.AppendLine(currentLine.ToString());
//
//			var result = multilineFormat.ToString().TrimEnd(Environment.NewLine.ToCharArray());
//			return result;
		}

		//public string MultilineFormat(string text)
		//{
		//	if (text == null)
		//		return "";
		//	else
		//	{
		//		var split = NewlineRegex.Split(text).Select(_multilineFormat);
		//		return String.Join("\n", split);
		//	}
		//}

		public string[] LineSplit(string text)
		{
			return text == null ? new string[0] : NewlineRegex.Split(text).SelectMany(_breakString).ToArray();
		}

		public string Truncate(string text)
		{
			if (text == null)
				return "";
			if (LineWidth == null)
				return text;

			var ellipsisWidth = Font.GetWidth("...");
			var lowerBound = LineWidth - ellipsisWidth;
			var offset = 0;

			for (var i = 0; i < text.Length; i++)
			{
				if (_isNewline(text[i]))
					return text.Substring(0, i) + "...";

				var nextWidth = Font.GetWidth(text[i]);
				var advance = Font.GetAdvance(text, i);

				if (offset + nextWidth > LineWidth)
					return text.Substring(0, i) + "...";

				if (offset + advance >= lowerBound)
				{
					offset += advance;
					for (var j = i + 1; j < text.Length; j++)
					{
						nextWidth = Font.GetWidth(text[j]);
						if (offset + nextWidth > LineWidth)
							return text.Substring(0, i) + "...";
						offset += Font.GetAdvance(text, j);
                    }
					break;
				}

				offset += advance;
			}

			return text;
		}

		public int GetWidth(string text)
		{
			return LineSplit(text).Select(Font.GetWidth).Max();
		}

		public double GetHeight(string text)
		{
			var n = LineSplit(text).Length;
			return n * Font.Height + (n - 1) * LineSeparation;
		}

		private static Point _getOrigin(Alignment alignment, double width, double height)
		{
			double x, y;

			switch (alignment & Alignment.Horizontal)
			{
				case Alignment.Left: x = 0; break;
				case Alignment.Center: x = width / 2; break;
				case Alignment.Right: x = width; break;
				default: x = 0; break;
			}

			switch (alignment & Alignment.Vertical)
			{
				case Alignment.Top: y = 0; break;
				case Alignment.Center: y = height / 2; break;
				case Alignment.Bottom: y = height; break;
				default: y = 0; break;
			}

			return new Point(x, y);
		}

		public Texture Render(string text)
		{
			var width = GetWidth(text);
			var height = (int)GMath.Ceiling(GetHeight(text));

			using (var buffer = new Framebuffer(width, height))
			using (buffer.Use())
			{
				Draw.Text(text, this, _getOrigin(Alignment, width, height), Colors.White);
				return buffer.Texture;
			}
		}


		internal (GraphicsPoint[] vertices, GraphicsPoint[] texCoords) RenderCoords(string text)
		{
			var lines = LineSplit(text);
			var length = lines.SelectMany(str => str.ToCharArray())
                              .Where(c => Font.HasCharacter(c))
                              .Count();
            double tXScale = 1.0 / Font.Texture.Width, tYScale = 1.0 / Font.Texture.Height;

            var vertices = new GraphicsPoint[6 * length];
            var texCoords = new GraphicsPoint[6 * length];

			var x0 = 0.0;
			var y0 = 0.0;
			
			switch (Alignment & Alignment.Vertical)
			{
				case Alignment.Top: y0 = 0; break;
				case Alignment.Center: y0 = -((Font.Height + LineSeparation) * (lines.Length - 1) + Font.Height) / 2; break;
				case Alignment.Bottom: y0 = -((Font.Height + LineSeparation) * (lines.Length - 1) + Font.Height); break;
			}

			var idx = 0;
			for (var l = 0; l < lines.Length; l++)
			{
				var lineWidth = Font.GetWidth(lines[l]);
				switch (Alignment & Alignment.Horizontal)
				{
					case Alignment.Left: x0 = 0; break;
					case Alignment.Center: x0 = -lineWidth / 2f; break;
					case Alignment.Right: x0 = -lineWidth; break;
				}

#warning What happens when the character is not found?

                var x = x0;
				var y = y0 + l * (Font.Height + LineSeparation);
				for (var i = 0; i < lines[l].Length; i++)
				{
                    if (Font.TryGetCharacter(lines[l][i], out FontCharacter c))
                    {
                        vertices[idx] = new GraphicsPoint(x + c.XOffset, y + c.YOffset);
                        vertices[idx + 1] = vertices[idx + 3] = new GraphicsPoint(x + c.XOffset + c.Width, y + c.YOffset);
                        vertices[idx + 2] = vertices[idx + 4] = new GraphicsPoint(x + c.XOffset, y + c.YOffset + c.Height);
                        vertices[idx + 5] = new GraphicsPoint(x + c.XOffset + c.Width, y + c.YOffset + c.Height);
                        x += c.XAdvance;
                        if (i < lines[l].Length - 1)
                            x += Font.GetKerning(lines[l][i], lines[l][i + 1]);

                        texCoords[idx] = new GraphicsPoint(tXScale * c.X, tYScale * c.Y);
                        texCoords[idx + 1] = texCoords[idx + 3] = new GraphicsPoint(tXScale * (c.X + c.Width), tYScale * c.Y);
                        texCoords[idx + 2] = texCoords[idx + 4] = new GraphicsPoint(tXScale * c.X, tYScale * (c.Y + c.Height));
                        texCoords[idx + 5] = new GraphicsPoint(tXScale * (c.X + c.Width), tYScale * (c.Y + c.Height));
                    }
                    idx += 6;
				}
			}

            return (vertices, texCoords);
		}

	}
}
