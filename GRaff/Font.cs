using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff
{
	public class Font
	{
		private Dictionary<char, FontCharacter> _characters = new Dictionary<char, FontCharacter>();


		public static Font Load(string bitmapFile, string characterFile)
		{
			var fontData = BmFont.FontLoader.Load(characterFile);
			var font = new Font();

			font.TextureBuffer = TextureBuffer.Load(bitmapFile);

			foreach (var c in fontData.Chars)
			{
				var character = new FontCharacter
				{
					X = c.X,
					Y = c.Y,
					Width = c.Width,
					Height = c.Height,
					XAdvance = c.XAdvance,
					XOffset = c.XOffset,
					YOffset = c.YOffset
				};

				font._characters.Add((char)c.ID, character);
			}

			font.Height = fontData.Common.LineHeight;

			return font;
		}

		public int Height { get; private set; }

		public double GetWidth(string str)
		{
			double width = 0;
			foreach (var c in str)
				width += _characters[c].XAdvance;
			return width;
		}

		public TextureBuffer TextureBuffer { get; private set; }

		internal void Render(string str, FontAlignment alignment, out PointF[] rectCoords, out PointF[] texCoords)
		{
			rectCoords = new PointF[str.Length * 4];
			texCoords = new PointF[str.Length * 4];

			float tXScale = 1.0f / TextureBuffer.Width, tYScale = 1.0f / TextureBuffer.Height;
			float xOffset = 0, yOffset = 0;

			if ((alignment & FontAlignment.HorizontalCenter) == FontAlignment.HorizontalCenter)
				xOffset = -(float)GetWidth(str) / 2;
			else if ((alignment & FontAlignment.Right) == FontAlignment.Right)
				xOffset = -(float)GetWidth(str);

			if ((alignment & FontAlignment.VerticalCenter) == FontAlignment.VerticalCenter)
				yOffset = -Height / 2;
			else if ((alignment & FontAlignment.Bottom) == FontAlignment.Bottom)
				yOffset = -Height;

			for (int index = 0; index < str.Length; index++)
			{
				var character = _characters[str[index]];
				var coordIndex = index * 4;
				rectCoords[coordIndex] = new PointF(xOffset + character.XOffset, yOffset + character.YOffset);
				rectCoords[coordIndex + 1] = new PointF(xOffset + character.XOffset + character.Width, yOffset + character.YOffset);
				rectCoords[coordIndex + 2] = new PointF(xOffset + character.XOffset + character.Width, yOffset + character.YOffset + character.Height);
				rectCoords[coordIndex + 3] = new PointF(xOffset + character.XOffset, yOffset + character.YOffset + character.Height);

				texCoords[coordIndex] = new PointF(tXScale * character.X, tYScale * character.Y);
				texCoords[coordIndex + 1] = new PointF(tXScale * (character.X + character.Width), tYScale * character.Y);
				texCoords[coordIndex + 2] = new PointF(tXScale * (character.X + character.Width), tYScale * (character.Y + character.Height));
				texCoords[coordIndex + 3] = new PointF(tXScale * character.X, tYScale * (character.Y + character.Height));

				xOffset += character.XAdvance;
			}

		}
	}
}
