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
		private readonly Dictionary<Tuple<char, char>, int> _kerning = new Dictionary<Tuple<char, char>, int>();

		public IReadOnlyCollection<char> ASCIICharacters { get; } = Array.AsReadOnly("\n !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~".ToArray());

		public Font(TextureBuffer buffer, FontFile fontData)
		{
			Buffer = buffer;
			foreach (var c in fontData.Chars)
				_characters.Add((char)c.Id, new FontCharacter(c));

			Height = fontData.Common.LineHeight;

			foreach (var kerning in fontData.Kernings)
				_kerning.Add(new Tuple<char, char>((char)kerning.First, (char)kerning.Second), kerning.Amount);
		}

		public bool IsDisposed { get; private set; }

		~Font()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				if (disposing)
				{
					if (!Buffer.IsDisposed)
						Buffer.Dispose();
					Buffer = null;
				}
			}
		}

		public TextureBuffer Buffer { get; private set; }

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

		public bool EnableKerning { get; set; } = true;

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

		public Vector GetOffset(char character)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			FontCharacter c;
			if (_characters.TryGetValue(character, out c))
				return new Vector(c.XOffset, c.YOffset);
			else
				return Vector.Zero;
		}
		
		private int GetAdvance(char character)
		{
			FontCharacter c;
			if (_characters.TryGetValue(character, out c))
				return c.XAdvance;
			else
				return 0;
		}

		/// <summary>
		/// Returns the offset between the position of the character at the specified position in the string and the end of that character.
		/// If the font uses kerning, this will be taken into consideration.
		/// </summary>
		/// <param name="str">A string.</param>
		/// <param name="i">The position of the character.</param>
		/// <returns>The horizontal advance of the character, taking kerning into consideration.</returns>
		public int GetAdvance(string str, int i)
		{
			Contract.Requires<ArgumentNullException>(str != null);
			Contract.Requires<IndexOutOfRangeException>(i >= 0 && i < str.Length);

			if (i == str.Length - 1)
				return GetAdvance(str[i]);
			else
				return GetAdvance(str[i]) + GetKerning(str[i], str[i + 1]);
		}
		
		public int GetWidth(string str)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			if (str == null)
				return 0;
			var width = 0;
			for (var i = 0; i < str.Length; i++)
				width += GetAdvance(str, i);
			return width;
		}


		internal TextureBuffer TextureBuffer
		{
			get
			{
				Contract.Requires<ObjectDisposedException>(!IsDisposed);
				return Buffer;
			}
		}

		public FontCharacter GetCharacter(char c)
		{
			return _characters[c];
		}

		public bool TryGetCharacter(char c, out FontCharacter fontCharacter)
		{
			return _characters.TryGetValue(c, out fontCharacter);
		}

		public int GetKerning(char first, char second)
		{
			if (!EnableKerning) return 0;
			int kerning;
			if (_kerning.TryGetValue(new Tuple<char, char>(first, second), out kerning))
				return kerning;
			else
				return 0;
		}
	}
}
