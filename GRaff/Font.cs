using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Graphics.Text;
using GRaff.Synchronization;

namespace GRaff
{
	public class Font : IAsset
	{
		private static readonly Regex NewlinePattern = new Regex("\r\n|\n");

		private readonly Dictionary<char, FontCharacter> _characters = new Dictionary<char, FontCharacter>();
		private readonly string _bitmapFile, _dataFile;
		private int _height;
		private TextureBuffer _textureBuffer;
		private IAsyncOperation _loadingOperation;

		public Font(string bitmapFile, string dataFile)
		{
			_bitmapFile = bitmapFile;
			_dataFile = dataFile;
		}

		[ContractInvariantMethod]
		private void objectInvariants()
		{
			Contract.Invariant(!IsLoaded || _textureBuffer != null);
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
			Contract.Requires<InvalidOperationException>(IsLoaded);
			if (str == null)
				return 0;
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
				Contract.Requires<InvalidOperationException>(IsLoaded);
				return _textureBuffer;
			}
		}

		public IAsyncOperation LoadAsync()
		{
			return _loadingOperation = TextureBuffer.LoadAsync(_bitmapFile)
				.Then(textureBuffer => {
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

					_textureBuffer = textureBuffer;
					IsLoaded = true;
				});
		}

		public void Unload()
		{
			if (!IsLoaded)
				return;

			_textureBuffer.Unload();
			_textureBuffer = null;
			IsLoaded = false;
			_characters.Clear();
		}


		internal void RenderTexCoords(string str, int offset, ref GraphicsPoint[] texCoords)
		{
			double tXScale = 1.0 / TextureBuffer.Width, tYScale = 1.0 / TextureBuffer.Height;
			
			for (var i = 0; i < str.Length; i++)
			{
				var index = 4 * (i + offset);

				FontCharacter character;
				if (_characters.TryGetValue(str[i], out character))
				{
					texCoords[index] = new GraphicsPoint(tXScale * character.X, tYScale * character.Y);
					texCoords[index + 1] = new GraphicsPoint(tXScale * (character.X + character.Width), tYScale * character.Y);
					texCoords[index + 2] = new GraphicsPoint(tXScale * (character.X + character.Width), tYScale * (character.Y + character.Height));
					texCoords[index + 3] = new GraphicsPoint(tXScale * character.X, tYScale * (character.Y + character.Height));
				}
				else
				{
					texCoords[index] = GraphicsPoint.Zero;
					texCoords[index + 1] = GraphicsPoint.Zero;
					texCoords[index + 2] = GraphicsPoint.Zero;
					texCoords[index + 3] = GraphicsPoint.Zero;
				}
			}
		}

	}
}
