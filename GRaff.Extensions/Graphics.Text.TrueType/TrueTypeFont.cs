using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpFont;


namespace GRaff.Graphics.Text.TrueType
{
	public class TrueTypeFont
	{

		public static ISet<char> ASCIICharacters { get; } = new ImmutableSet("\n !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~");
		public static ISet<char> AlphaNumeric { get; } = new ImmutableSet("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

        public TrueTypeFont(FileInfo file, ISet<char> charSet, FontOptions options)
		{
			this.FontFile = file;
			this.CharSet = charSet;
			this.FontOptions = options;
		}
        
		public static FileInfo FindFontFile(string fontFamily, FontOptions options = FontOptions.None)
		{
			if ((options & FontOptions.Bold) == FontOptions.Bold)
				fontFamily += " Bold";
			if ((options & FontOptions.Italic) == FontOptions.Italic)
				fontFamily += " Italic";

			switch (Environment.OSVersion.Platform)
			{
				case PlatformID.Win32NT:
                    var _fontsKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts");
                    var fontFileName = (string)(_fontsKey.GetValue(fontFamily)
                                               ?? _fontsKey.GetValue(fontFamily + " (TrueType)"));
                    fontFileName = Path.Combine(@"C:\Windows\Fonts\", fontFileName);
                    return File.Exists(fontFileName) ? new FileInfo(fontFileName) : null;

				case PlatformID.Unix:
				case PlatformID.MacOSX:
					var basePath = Path.Combine("/Library/Fonts/", fontFamily);
					if (File.Exists(basePath + ".ttf"))
						return new FileInfo(basePath + ".ttf");
					else if (File.Exists(basePath + ".ttc"))
						return new FileInfo(basePath + ".ttc");
					else
						return null;

				default:
					throw new NotSupportedException("TrueType loading is not supported on OS version " + Environment.OSVersion.Platform.ToString());
			}
		}

		public static TrueTypeFont LoadFamily(string fontFamily, ISet<char> charSet, FontOptions options = FontOptions.None)
		{
			var file = FindFontFile(fontFamily, options);
			if (file == null)
				throw new FileNotFoundException($"Did not find a font file corresponding to the font family {fontFamily}.");
			return new TrueTypeFont(file, charSet, options);
		}

		public static Font LoadRasterized(string fontFamily, ISet<char> charSet, int size, FontOptions options = FontOptions.None)
		{
			var ttf = LoadFamily(fontFamily, charSet, options);
			return ttf.Rasterize(size);
		}

		public FileInfo FontFile { get; }

		public ISet<char> CharSet { get; }
  
		public FontOptions FontOptions { get; }

		public Font Rasterize(int size)
		{
			var lib = new Library();
			var face = new Face(lib, FontFile.FullName);

			face.SetPixelSizes((uint)size, 0);
            
			var chars = _genCharLayout(face, CharSet);

			var dst = new Color[chars.Max(c => c.Y + c.Height), chars.Max(c => c.X + c.Width)];
			foreach (var c in chars)
				_blit(face, c, dst);

			var kernings = ((FontOptions & FontOptions.IgnoreKerning) == FontOptions.IgnoreKerning)
				            ? new List<FontKerning>() : _makeKernings(face, CharSet);
            
			var texture = new Texture(dst);

			return new Font(texture, new FontFile
			{
				Chars = chars.ToList(),
				Common = new FontCommon { LineHeight = face.Glyph.Metrics.VerticalAdvance.ToInt32() },
				Info = null,
				Kernings = kernings,
				Pages = new List<FontPage>()
			});

		}

		#region Rasterizing helper functions

		private static List<FontKerning> _makeKernings(Face face, ISet<char> charSet)
        {
            if (!face.HasKerning)
                return new List<FontKerning>();

            var kernings =
                from left in charSet
                from right in charSet
                let k = face.GetKerning(face.GetCharIndex(left), face.GetCharIndex(right), KerningMode.Default).X.ToInt32()
                where k != 0
                select new FontKerning { Left = left, Right = right, Amount = k };

            return kernings.ToList();
        }
        
		private static FontCharacter[] _genCharLayout(Face face, IEnumerable<char> chars)
        {
			//TODO// Pack a bit more efficiently
			var x = 0;
			return chars.Select(c =>
            {
				face.LoadChar(c, LoadFlags.Default, LoadTarget.Normal);
				face.Glyph.RenderGlyph(RenderMode.Normal);
				var m = face.Glyph.Metrics;
                
				var fc = new FontCharacter(c, x, 0, face.Glyph.Bitmap.Width, face.Glyph.Bitmap.Rows,
										   face.Glyph.BitmapLeft, face.Glyph.LinearVerticalAdvance.ToInt32() - face.Glyph.BitmapTop, face.Glyph.Advance.X.ToInt32());
				x += fc.Width + 1;
                
				return fc;
            }).ToArray();
        }


		private void _blit(Face face, FontCharacter character, Color[,] dst)
		{
			face.LoadChar(character.Char, LoadFlags.Default, LoadTarget.Normal);
			face.Glyph.RenderGlyph(RenderMode.Normal);

			if (face.Glyph.Bitmap.Buffer == IntPtr.Zero)
				return;

			var data = face.Glyph.Bitmap.BufferData;
			var c = 0;

			for (var y = 0; y < character.Height; y++)
                for (var x = 0; x < character.Width; x++)
					dst[character.Y + y, character.X + x] = new Color(255, 255, 255, data[c++]);
		}

		#endregion

	}


	internal class ImmutableSet : ISet<char>
    {
		private HashSet<char> _underlyingSet;

        public ImmutableSet(IEnumerable<char> chars) => _underlyingSet = new HashSet<char>(chars);

        public int Count => _underlyingSet.Count;
        public bool IsReadOnly => true;
        public bool Add(char item) => throw new NotSupportedException();
        public void Clear() => throw new NotSupportedException();
        public bool Contains(char item) => _underlyingSet.Contains(item);
        public void CopyTo(char[] array, int arrayIndex) => _underlyingSet.CopyTo(array, arrayIndex);
        public void ExceptWith(IEnumerable<char> other) => throw new NotSupportedException();
        public IEnumerator<char> GetEnumerator() => _underlyingSet.GetEnumerator();
        public void IntersectWith(IEnumerable<char> other) => throw new NotSupportedException();
        public bool IsProperSubsetOf(IEnumerable<char> other) => _underlyingSet.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<char> other) => _underlyingSet.IsProperSupersetOf(other);
        public bool IsSubsetOf(IEnumerable<char> other) => _underlyingSet.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<char> other) => _underlyingSet.IsSupersetOf(other);
        public bool Overlaps(IEnumerable<char> other) => _underlyingSet.Overlaps(other);
        public bool Remove(char item) => throw new NotSupportedException();
        public bool SetEquals(IEnumerable<char> other) => _underlyingSet.SetEquals(other);
        public void SymmetricExceptWith(IEnumerable<char> other) => throw new NotSupportedException();
        public void UnionWith(IEnumerable<char> other) => throw new NotSupportedException();
        
		void ICollection<char>.Add(char item) => throw new NotSupportedException();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
	}
}
