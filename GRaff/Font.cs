using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff
{
	public class Font : IAsset
	{
		private Dictionary<char, FontCharacter> _characters = new Dictionary<char, FontCharacter>();
		private string _bitmapFile, _dataFile;
		private int _height;
		private TextureBuffer _textureBuffer;

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

		public AssetState AssetState { get; private set; }

		public int Height
		{
			get
			{
				if (AssetState != AssetState.Loaded) throw new InvalidOperationException("The font is not loaded.");
				return _height;
			}
		}

		public double GetWidth(string str)
		{
			if (AssetState != AssetState.Loaded) throw new InvalidOperationException("The font is not loaded.");

			double width = 0;
			foreach (var c in str)
				width += _characters[c].XAdvance;
			return width;
		}

		internal TextureBuffer TextureBuffer
		{
			get
			{
				if (AssetState != AssetState.Loaded) throw new InvalidOperationException("The font is not loaded.");
				return _textureBuffer;
			}
		}

		private void _load(TextureBuffer textureBuffer, FontFile fontData)
		{
			_textureBuffer = textureBuffer;

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

				_characters.Add((char)c.Id, character);
			}

			_height = fontData.Common.LineHeight;

			AssetState = AssetState.Loaded;
		}

		public void Load()
		{
			TextureBuffer textureBuffer;
			FontFile fontData;

			lock (this)
			{
				if (AssetState == AssetState.Loaded)
					return;
				AssetState = AssetState.Loaded;

				textureBuffer = TextureBuffer.Load(_bitmapFile);
				fontData = FontLoader.Load(_dataFile);
			}

			_load(textureBuffer, fontData);
		}

		public async Task LoadAsync()
		{
			if (AssetState != AssetState.NotLoaded)
				return;

			AssetState = AssetState.LoadingAsync;

			var textureBuffer = await TextureBuffer.LoadAsync(_bitmapFile);
			var fontData = FontLoader.Load(_dataFile);

			lock (this)
			{
				if (AssetState == AssetState.LoadingAsync)
					_load(textureBuffer, fontData);
			}
		}

		public void Unload()
		{
			lock (this)
			{
				if (AssetState == AssetState.NotLoaded)
					return;

				if (_textureBuffer != null)
				{
					_textureBuffer.Dispose();
					_textureBuffer = null;
				}

				_characters.Clear();

				AssetState = AssetState.NotLoaded;
			}
		}

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
