using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using GRaff.Graphics;
using GRaff.Graphics.Text;
using GRaff.Synchronization;


namespace GRaff
{
    public class Font : IDisposable
    {
        private static XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));

        private readonly Dictionary<char, FontCharacter> _characters = new Dictionary<char, FontCharacter>();
        private readonly Dictionary<Tuple<char, char>, int> _kerning = new Dictionary<Tuple<char, char>, int>();
        private bool _isTexturePrivate = false;

        public Font(Texture texture, FontFile fontData)
        {
            this._texture = texture;
            foreach (var c in fontData.Chars)
				_characters.Add((char)c.Id, c);

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
     
        private static FontFile _loadFontFile(string fontFilePath)
        {
            FontFile fontFile;
            using (var textReader = File.OpenRead(fontFilePath))
                fontFile = (FontFile)deserializer.Deserialize(textReader);

            if (fontFile.Pages.Count > 1)
                throw new NotSupportedException("Fonts with multiple pages are currently not supported");

            return fontFile;
        }

        public static IAsyncOperation<Font> LoadAsync(string fontFilePath)
        {
            var fontFile = _loadFontFile(fontFilePath);
            var fontTexturePath = Path.Combine(Path.GetDirectoryName(fontFilePath), fontFile.Pages[0].File);

            if (!File.Exists(fontTexturePath))
                throw new FileNotFoundException("The font texture file was not found.", fontTexturePath);

            return Texture.LoadAsync(fontTexturePath).ThenQueue(textureBuffer => new Font(textureBuffer, fontFile) { _isTexturePrivate = true });
        }

        public static Font Load(string fontFilePath)
        {
            var fontFile = _loadFontFile(fontFilePath);
            var fontTexturePath = Path.Combine(Path.GetDirectoryName(fontFilePath), fontFile.Pages[0].File);

            if (!File.Exists(fontTexturePath))
                throw new FileNotFoundException("The font texture file was not found.", fontTexturePath);

            return new Font(Texture.Load(fontTexturePath), fontFile) { _isTexturePrivate = true };
        }


        private Texture _texture;
        public Texture Texture
        {
            get { _isTexturePrivate = false; return _texture; }
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
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
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
            Contract.Requires<ObjectDisposedException>(!IsDisposed);

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

		public FontCharacter GetCharacter(char c)
		{
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            return _characters[c];
		}

        public bool HasCharacter(char c)
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
             return _characters.ContainsKey(c);
        }

        public bool TryGetCharacter(char c, out FontCharacter fontCharacter)
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
           return _characters.TryGetValue(c, out fontCharacter);
        }

		public int GetKerning(char first, char second)
		{
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            if (!HasKerning) return 0;
            if (_kerning.TryGetValue(new Tuple<char, char>(first, second), out int kerning))
                return kerning;
            else
                return 0;
        }

		public Texture RenderText(string text)
		{
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            Contract.Requires<ArgumentNullException>(text != null);
            var width = GetWidth(text);
            var height = Height;
            var buffer = new Framebuffer(width, height);
            using (buffer.Use())
            {
                Draw.Text(text, this, (0, 0), Colors.White);
                return buffer.Texture;
            }
		}


        #region IDisposable support

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
                if (_isTexturePrivate)
                    _texture.Dispose();
                _texture = null;
                _characters.Clear();
                IsDisposed = true;
            }
        }
        #endregion
    }
}
