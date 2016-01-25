using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Synchronization;

namespace GRaff
{
	public class Font : IAsset
	{
		private static readonly Regex NewlinePattern = new Regex("\r\n|\n");

		private Dictionary<char, FontCharacter> _characters = new Dictionary<char, FontCharacter>();
		private string _bitmapFile, _dataFile;
		private int _height;
		private TextureBuffer _textureBuffer;
		private IAsyncOperation _loadingOperation;

		public Font(string bitmapFile, string dataFile)
		{
			_bitmapFile = bitmapFile;
			_dataFile = dataFile;
		}

		public static Font Load(string bitmapFile, string characterFile)
		{
			var font = new Font(bitmapFile, characterFile);
			font.Load();
			return font;
		}

		public bool IsLoaded { get; private set; }

		public int Height
		{
			get
			{
				if (!IsLoaded) throw new InvalidOperationException("The font is not loaded.");
				return _height;
			}
		}

		public int GetWidth(char character)
		{
			FontCharacter c;
			if (_characters.TryGetValue(character, out c))
				return c.XAdvance;
			else
				return 0;
		}

		public IntVector GetSize(char character)
		{
			FontCharacter c;
			if (_characters.TryGetValue(character, out c))
				return new IntVector(c.Width, c.Height);
			else
				return IntVector.Zero;
		}

		public int GetWidth(string str)
		{
			if (!IsLoaded) throw new InvalidOperationException("The font is not loaded.");

			var width = 0;
			foreach (var c in str)
				width += GetWidth(c);
			return width;
		}
		

		public Vector GetOffset(char character)
		{
			FontCharacter c;
			if (_characters.TryGetValue(character, out c))
				return new Vector(c.XOffset, c.YOffset);
			else
				return Vector.Zero;
		}

		internal TextureBuffer TextureBuffer
		{
			get
			{
				if (!IsLoaded) throw new InvalidOperationException("The font is not loaded.");
				return _textureBuffer;
			}
		}

		public IAsyncOperation LoadAsync()
		{
			return TextureBuffer.LoadAsync(_bitmapFile)
				.Then(textureBuffer => {
					_textureBuffer = textureBuffer;
					var fontData = FontLoader.Load(_dataFile);

					foreach (var c in fontData.Chars)
					{
						var character = new FontCharacter {
							X = c.X,
							Y = c.Y,
							Width = c.Width,
							Height = c.Height,
							XAdvance = c.XAdvance,
							XOffset = c.XOffset,
							YOffset = c.YOffset
						};

						_characters.Add((char)c.Id, character);
					}

					_height = fontData.Common.LineHeight;

					IsLoaded = true;
				});
		}

		public void Unload()
		{
			if (!IsLoaded)
				return;

			if (_textureBuffer != null)
			{
				_textureBuffer.Unload();
				_textureBuffer = null;
			}

			_characters.Clear();
			IsLoaded = false;
		}


		internal void RenderTexCoords(string str, int offset, ref PointF[] texCoords)
		{
			float tXScale = 1.0f / TextureBuffer.Width, tYScale = 1.0f / TextureBuffer.Height;
			
			for (var i = 0; i < str.Length; i++)
			{
				var index = 4 * (i + offset);

				FontCharacter character;
				if (_characters.TryGetValue(str[i], out character))
				{
					texCoords[index] = new PointF(tXScale * character.X, tYScale * character.Y);
					texCoords[index + 1] = new PointF(tXScale * (character.X + character.Width), tYScale * character.Y);
					texCoords[index + 2] = new PointF(tXScale * (character.X + character.Width), tYScale * (character.Y + character.Height));
					texCoords[index + 3] = new PointF(tXScale * character.X, tYScale * (character.Y + character.Height));
				}
				else
				{
					texCoords[index] = PointF.Zero;
					texCoords[index + 1] = PointF.Zero;
					texCoords[index + 2] = PointF.Zero;
					texCoords[index + 3] = PointF.Zero;
				}
			}
		}

	}
}
