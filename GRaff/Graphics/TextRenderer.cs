﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GRaff.Graphics
{
	public sealed class TextRenderer
	{
		private static readonly Regex NewlineRegex = new Regex("\r\n|\n");
		private int lengthOfSpace;
		
		public TextRenderer(Font font, FontAlignment alignment = FontAlignment.TopLeft, int width = Int32.MaxValue)
			: this(font, alignment, width, font.Height)
		{ }

		public TextRenderer(Font font, FontAlignment alignment, int width, double lineSeparation)
		{
			this.Font = font;
			this.Alignment = alignment;
			this.Width = width;
			this.LineSeparation = lineSeparation;
			lengthOfSpace = GetWidth(" ");
		}

		public Font Font { get; set; }

		public int Width { get; set; }

		public double LineSeparation { get; set; }

		public FontAlignment Alignment { get; set; } = FontAlignment.TopLeft;

		private string _multilineFormat(string text)
		{
			if (text == null)
				throw new ArgumentNullException();

			var words = text.Split(' ');
			var multilineFormat = new StringBuilder(text.Length);
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
			return String.Concat(Regex.Split(text, Environment.NewLine).Select(str => _multilineFormat(str)));
		}

		public string[] LineSplit(string text)
		{
			return NewlineRegex.Split(text).Select(str => _multilineFormat(str)).SelectMany(str => NewlineRegex.Split(str)).ToArray();
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

		public int GetWidth(string text) => Font.GetWidth(text);



		internal string[] RenderCoords(string text, out PointF[] quadCoords)
		{
			var lines = LineSplit(text);
			var length = lines.Sum(line => line.Length);

			quadCoords = new PointF[4 * length];

			var x0 = 0f;
			var y0 = 0f;
			
			switch (Alignment & FontAlignment.Vertical)
			{
				case FontAlignment.Top: y0 = 0; break;
				case FontAlignment.Center: y0 = -(float)(LineSeparation * (lines.Length - 1) + Font.Height) / 2f; break;
				case FontAlignment.Bottom: y0 = -(float)(LineSeparation * (lines.Length - 1) + Font.Height); break;
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
					quadCoords[coordIndex] = new PointF(x + d.X, y + d.Y);
					quadCoords[coordIndex + 1] = new PointF(x + d.X + s.X, y + d.Y);
					quadCoords[coordIndex + 2] = new PointF(x + d.X + s.X, y + d.Y + s.Y);
					quadCoords[coordIndex + 3] = new PointF(x + d.X, y + d.Y + s.Y);
					x += s.X;
					coordIndex += 4;
				}
			}

			return lines;
		}
		/*
		public void Draw(string text, Color color, Point location) => Draw(text, color, location.X, location.Y);

		public void Draw(string text, Color color, double x, double y)
		{
			var lines = LineSplit(text);
			if ((Alignment & FontAlignment.Vertical) == FontAlignment.Center)
				y -= lines.Length * LineSeparation / 2 - Font.Height / 2;
			else if ((Alignment & FontAlignment.Vertical) == FontAlignment.Bottom)
				y -= lines.Length * LineSeparation - Font.Height;

			for (var i = 0; i < lines.Length; i++)
				GRaff.Draw.Text(this, color, x, y + i * LineSeparation);
		}
		*/
		public TextureBuffer Render(string text)
		{
			throw new NotImplementedException();
		}
	}
}
