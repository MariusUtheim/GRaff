/*using System;
using System.Collections.Generic;
using System.IO;
using GRaff.Synchronization;

namespace GRaff.Graphics.Text.TrueType
{
    public class TrueTypeFont
    {
        public TrueTypeFont()
        {
        }


        public static IEnumerable<string> GetFontFamilies()
            => new InstalledFontCollection().Families.Select(f => f.Name);


		#warning Do this better
        private static FileInfo _getFontFileName(string fontFamily, FontOptions options)
        {
            if ((options & FontOptions.Bold) == FontOptions.Bold)
                fontFamily += " Bold";
            if ((options & FontOptions.Italic) == FontOptions.Italic)
                fontFamily += " Italic";

            var fontFileName = TrueTypeLoader.GetTrueTypeFile(fontFamily);

            if (fontFileName == null)
                throw new IOException("The specified font name is not a valid font family, or the font family does not support the specified font options.");

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    fontFileName = Path.Combine(@"C:\Windows\Fonts\", fontFileName);
                    break;

                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    var basePath = Path.Combine("/Library/Fonts/", fontFamily);
                    if (File.Exists(basePath + ".ttf"))
                        fontFileName = basePath + ".ttf";
                    else
                        fontFileName = basePath + ".ttc";
                    break;

                default:
                    throw new NotSupportedException("TrueType loading is not supported on OS version " + Environment.OSVersion.Platform.ToString());
            }


            if (!File.Exists(fontFileName))
                throw new FileNotFoundException();

            return new FileInfo(fontFileName);
        }
       

        public static TrueTypeFont LoadFromSystem(string fontFamily, ISet<char> charSet, FontOptions options = FontOptions.None)
        {
            return TrueTypeLoader.LoadTrueType(_getFontFileName(fontFamily, options), charSet, (options & FontOptions.IgnoreKerning) == FontOptions.IgnoreKerning);
        }

        
        public static IAsyncOperation<TrueTypeFont> LoadFromSystemAsync(string fontFamily, ISet<char> charSet, FontOptions options = FontOptions.None)
        {
            return TrueTypeLoader.LoadTrueTypeAsync(_getFontFileName(fontFamily, options), size, charSet, (options & FontOptions.IgnoreKerning) == FontOptions.IgnoreKerning);
        }

        /// <summary>
        /// Loads a TrueType font from the specified file.
        /// </summary>
        /// <param name="fileName">The TrueType font file to load.</param>
        /// <param name="size">The size of the loaded font.</param>
        /// <param name="charSet">The character set to load.</param>
        /// <returns></returns>
        public static Font LoadFromFile(string fileName, int size, ISet<char> charSet)
        {
            return TrueTypeLoader.LoadTrueType(new FileInfo(fileName), size, charSet, true);
        }

		public static IAsyncOperation<TrueTypeFont> LoadFromFileAsync(string fileName, int size, ISet<char> charSet)
		{
#warning Not implemented
			throw new NotImplementedException();
		}

		public Font Render(int size)
		{
			throw new NotImplementedException();
		}
    }
}
*/