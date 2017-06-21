using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GRaff.Graphics.Text;
using GRaff.Synchronization;


namespace GRaff
{
    public class Font : IDisposable
	{
        private class ImmutableSet : ISet<char>
        {
            private HashSet<char> _backingSet;

            public ImmutableSet(IEnumerable<char> chars) => _backingSet = new HashSet<char>(chars);

            public int Count => _backingSet.Count;
            public bool IsReadOnly => true;
            public bool Add(char item) => throw new NotSupportedException();
            public void Clear() => throw new NotSupportedException();
            public bool Contains(char item) => _backingSet.Contains(item);
            public void CopyTo(char[] array, int arrayIndex) => _backingSet.CopyTo(array, arrayIndex);
            public void ExceptWith(IEnumerable<char> other) => throw new NotSupportedException();
            public IEnumerator<char> GetEnumerator() => _backingSet.GetEnumerator();
            public void IntersectWith(IEnumerable<char> other) => throw new NotSupportedException();
            public bool IsProperSubsetOf(IEnumerable<char> other) => _backingSet.IsProperSubsetOf(other);
            public bool IsProperSupersetOf(IEnumerable<char> other) => _backingSet.IsProperSupersetOf(other);
            public bool IsSubsetOf(IEnumerable<char> other) => _backingSet.IsSubsetOf(other);
            public bool IsSupersetOf(IEnumerable<char> other) => _backingSet.IsSupersetOf(other);
            public bool Overlaps(IEnumerable<char> other) => _backingSet.Overlaps(other);
            public bool Remove(char item) => throw new NotSupportedException();
            public bool SetEquals(IEnumerable<char> other) => _backingSet.SetEquals(other);
            public void SymmetricExceptWith(IEnumerable<char> other) => throw new NotSupportedException();
            public void UnionWith(IEnumerable<char> other) => throw new NotSupportedException();
            void ICollection<char>.Add(char item) => throw new NotSupportedException();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private static readonly Regex NewlinePattern = new Regex("\r\n|\n");

		private readonly Dictionary<char, FontCharacter> _characters = new Dictionary<char, FontCharacter>();
		private readonly Dictionary<Tuple<char, char>, int> _kerning = new Dictionary<Tuple<char, char>, int>();

		public static ISet<char> ASCIICharacters { get; } = new ImmutableSet("\n !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~");
		public static IEnumerable<string> GetFontFamilies() => new InstalledFontCollection().Families.Select(f => f.Name);


		public Font(Texture texture, FontFile fontData)
		{
			Texture = texture;
			foreach (var c in fontData.Chars)
				_characters.Add((char)c.Id, new FontCharacter(c));

			Height = fontData.Common.LineHeight;

			if (fontData.Kernings?.Any() ?? false)
			{
				HasKerning = true;
				foreach (var kerning in fontData.Kernings)
					_kerning.Add(new Tuple<char, char>((char)kerning.Left, (char)kerning.Right), kerning.Amount);
			}
			else
				HasKerning = false;
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
					if (!Texture.IsDisposed)
                        Texture.Dispose();
					Texture = null;
				}
			}
		}

        public Texture Texture { get; private set; }

		public static IAsyncOperation<Font> LoadAsync(string bitmapFile, string fontDataFile)
		{
			return Texture.LoadAsync(bitmapFile)
				.ThenQueue(textureBuffer =>
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


        private static FileInfo _getFontFileName(string fontFamily, FontOptions options)
        {
            if ((options & FontOptions.Bold) == FontOptions.Bold)
                fontFamily += " Bold";
            if ((options & FontOptions.Italic) == FontOptions.Italic)
                fontFamily += " Italic";

            fontFamily += " (TrueType)";

            var fontFileName = Path.Combine(@"C:\Windows\Fonts\", TrueTypeLoader.GetTrueTypeFile(fontFamily));

            if (!File.Exists(fontFileName))
                throw new FileNotFoundException();

            return new FileInfo(fontFileName);
        }

        public static Font LoadTrueType(string fontFamily, int size, ISet<char> charSet, FontOptions options = FontOptions.None)
		{
            return TrueTypeLoader.LoadTrueType(_getFontFileName(fontFamily, options), size, charSet, (options & FontOptions.IgnoreKerning) == FontOptions.IgnoreKerning);
		}

		public static IAsyncOperation<Font> LoadTrueTypeAsync(string fontFamily, int size, ISet<char> charSet, FontOptions options = FontOptions.None)
		{
			return TrueTypeLoader.LoadTrueTypeAsync(_getFontFileName(fontFamily, options), size, charSet, (options & FontOptions.IgnoreKerning) == FontOptions.IgnoreKerning);
		}

		public bool HasKerning { get; }

		public int Height { get; }
		
		public int GetWidth(char character)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
            if (_characters.TryGetValue(character, out FontCharacter c))
                return c.XAdvance;
            else
                return 0;
        }

		public IntVector GetSize(char character)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
            if (_characters.TryGetValue(character, out FontCharacter c))
                return new IntVector(c.Width, c.Height);
            else
                return IntVector.Zero;
        }

		public Vector GetOffset(char character)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
            if (_characters.TryGetValue(character, out FontCharacter c))
                return new Vector(c.XOffset, c.YOffset);
            else
                return Vector.Zero;
        }
		
		private int GetAdvance(char character)
		{
            if (_characters.TryGetValue(character, out FontCharacter c))
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


		internal Texture TextureBuffer
		{
			get
			{
				Contract.Requires<ObjectDisposedException>(!IsDisposed);
				return Texture;
			}
		}

		public FontCharacter GetCharacter(char c)
		{
			return _characters[c];
		}

        public bool HasCharacter(char c) => _characters.ContainsKey(c);

		public bool TryGetCharacter(char c, out FontCharacter fontCharacter) => _characters.TryGetValue(c, out fontCharacter);

		public int GetKerning(char first, char second)
		{
			if (!HasKerning) return 0;
            if (_kerning.TryGetValue(new Tuple<char, char>(first, second), out int kerning))
                return kerning;
            else
                return 0;
        }

		public Texture RenderText(string text)
		{
			throw new NotImplementedException();
		}
	}
}
