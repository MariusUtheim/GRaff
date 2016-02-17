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
	public class Font : IDisposable
	{
		private static readonly Regex NewlinePattern = new Regex("\r\n|\n");

		private readonly Dictionary<char, FontCharacter> _characters = new Dictionary<char, FontCharacter>();
		private TextureBuffer _buffer;

		public Font(TextureBuffer buffer, FontFile fontData)
		{
			_buffer = buffer;
			foreach (var c in fontData.Chars)
				_characters.Add((char)c.Id, new FontCharacter(c));

			Height = fontData.Common.LineHeight;

		}

		public bool IsDisposed { get; private set; }

		~Font()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				if (disposing)
				{
					if (!_buffer.IsDisposed)
						_buffer.Dispose();
					_buffer = null;
				}
			}
		}

		public static IAsyncOperation<Font> LoadAsync(string bitmapFile, string fontDataFile)
		{
			return TextureBuffer.LoadAsync(bitmapFile)
				.Then(textureBuffer =>
				{
					var fontData = FontLoader.Load(fontDataFile);
					return new Font(textureBuffer, fontData);
				});
		}


		public static Font Load(string bitmapFile, string fontDataFile)
		{
			Contract.Ensures(Contract.Result<Font>() != null);
			return LoadAsync(bitmapFile, fontDataFile).Wait();
		}



		public int Height { get; }

		public int GetWidth(char character)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			FontCharacter c;
			if (_characters.TryGetValue(character, out c))
				return c.XAdvance;
			else
				return 0;
		}

		public IntVector GetSize(char character)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			FontCharacter c;
			if (_characters.TryGetValue(character, out c))
				return new IntVector(c.Width, c.Height);
			else
				return IntVector.Zero;
		}

		public int GetWidth(string str)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			if (str == null)
				return 0;
			var width = 0;
			foreach (var c in str)
				width += GetWidth(c);
			return width;
		}
		

		public Vector GetOffset(char character)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
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
				Contract.Requires<ObjectDisposedException>(!IsDisposed);
				return _buffer;
			}
		}



		internal void RenderTexCoords(string str, int offset, ref GraphicsPoint[] texCoords)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
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
