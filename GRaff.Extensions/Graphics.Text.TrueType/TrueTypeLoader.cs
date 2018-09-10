/*using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using GRaff.Synchronization;
using Microsoft.Win32;
using SharpFont;
using GdiColor = System.Drawing.Color;
using GdiGraphics = System.Drawing.Graphics;
using GdiRectangle = System.Drawing.Rectangle;

namespace GRaff.Graphics.Text
{
    internal static class TrueTypeLoader
    {
        public static Font LoadTrueType(FileInfo file, int size, ISet<char> charSet, bool suppressKerning)
        {
            var lib = new Library();
            var face = new Face(lib, file.FullName);

            face.SetPixelSizes((uint)size, 0);
            
            var glyphs = charSet.Select(c => _makeFontChar(face, c)).ToArray();
            var rects = _genRects(glyphs);
            
            var bmp = new Bitmap(rects.Max(r => r.Right), rects.Max(r => r.Bottom));
            var g = GdiGraphics.FromImage(bmp);
            g.Clear(GdiColor.Transparent);

            for (var i = 0; i < glyphs.Length; i++)
                g.DrawImage(glyphs[i].Image, new GdiRectangle(rects[i].Left, rects[i].Top, rects[i].Width, rects[i].Height), new GdiRectangle(0, 0, glyphs[i].Width, glyphs[i].Height), GraphicsUnit.Pixel);

            var bmpData = bmp.LockBits(new GdiRectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var buffer = new Texture(bmpData.Width, bmpData.Height, bmpData.Scan0);
            bmp.UnlockBits(bmpData);

            var b = buffer.ToColorArray();
            for (var x = 0; x < buffer.Width; x++)
                for (var y = 0; y < buffer.Height; y++)
                    b[y, x] = new Color(255, 255, 255, b[y, x].A);
            buffer = new Texture(b);



            //var output = (Image)bmp.Clone();
            //g = GdiGraphics.FromImage(output);
            //for (var i = 0; i < glyphs.Length; i++)
            //    g.DrawRectangle(Pens.Red, new GdiRectangle(rects[i].Left, rects[i].Top, rects[i].Width, rects[i].Height));
            //output.Save("C:/test/chars/chars.png");


            var chars = new FontChar[glyphs.Length];
            for (var i = 0; i < glyphs.Length; i++)
            {
                chars[i] = new FontChar
                {
                    Id = glyphs[i].Character,
                    Channel = 1,
                    Page = 0,
                    X = rects[i].Left,
                    Y = rects[i].Top,
                    Width = glyphs[i].Width,
                    Height = glyphs[i].Height,
                    XOffset = glyphs[i].XOffset,
                    YOffset = glyphs[i].YOffset,
                    XAdvance = glyphs[i].XAdvance
                };
            }

            face.LoadChar('\n', LoadFlags.Default, LoadTarget.Normal);

            return new Font(buffer, new FontFile
            {
                Chars = chars.ToList(),
                Common = new FontCommon { LineHeight = face.Glyph.Metrics.VerticalAdvance.ToInt32() },
                Info = null,
                Kernings = suppressKerning ? new List<FontKerning>() : _makeKernings(face, charSet),
                Pages = new List<FontPage>()
            });

        }

        public static IAsyncOperation<Font> LoadTrueTypeAsync(FileInfo file, int size, ISet<char> charSet, bool suppressKerning)
		{
			Glyph[] glyphs = null;
			Bitmap bmp = null;
			BitmapData bmpData = null;
			IntRectangle[] rects = null;
			Face face = null;

			return Async.RunParallel(() =>
			{
				var lib = new Library();
				face = new Face(lib, file.FullName);

				face.SetPixelSizes((uint)size, 0);

				glyphs = charSet.Select(c => _makeFontChar(face, c)).ToArray();

				rects = _genRects(glyphs);

				bmp = new Bitmap(rects.Max(r => r.Right), rects.Max(r => r.Bottom));
				var g = GdiGraphics.FromImage(bmp);
				g.Clear(GdiColor.Transparent);

				for (var i = 0; i < glyphs.Length; i++)
					g.DrawImage(glyphs[i].Image, rects[i].Left, rects[i].Top);
				
			}).ThenQueue(() =>
			{
				bmpData = bmp.LockBits(new GdiRectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
				var buffer = new Texture(bmpData.Width, bmpData.Height, bmpData.Scan0);
				bmp.UnlockBits(bmpData);

				var chars = new FontChar[glyphs.Length];
				for (var i = 0; i < glyphs.Length; i++)
				{
					chars[i] = new FontChar
					{
						Id = glyphs[i].Character,
						Channel = 1,
						Page = 0,
						X = rects[i].Left,
						Y = rects[i].Top,
						Width = glyphs[i].Width,
						Height = glyphs[i].Height,
						XOffset = glyphs[i].XOffset,
						YOffset = glyphs[i].YOffset,
						XAdvance = glyphs[i].XAdvance
					};
				}

				face.LoadChar('\n', LoadFlags.Default, LoadTarget.Normal);

				return new Font(buffer, new FontFile
				{
					Chars = chars.ToList(),
					Common = new FontCommon { LineHeight = face.Glyph.Metrics.VerticalAdvance.ToInt32() },
					Info = null,
					Kernings = suppressKerning ? new List<FontKerning>() : _makeKernings(face, charSet),
					Pages = new List<FontPage>()
				});
			});
		}



        private static Glyph _makeFontChar(Face face, char c)
        {
            face.LoadChar(c, LoadFlags.Default, LoadTarget.Normal);

            face.Glyph.RenderGlyph(RenderMode.Normal);


            //TODO// Render everything manually instead of first converting to Bitmap
#warning Does this work with italics? 
            var bmp = (face.Glyph.Bitmap.Width == 0) ? new Bitmap(1, 1) : face.Glyph.Bitmap.ToGdipBitmap();


            return new Glyph
			{
				Character = c,
				Image = bmp,
				XOffset = face.Glyph.BitmapLeft ,
				YOffset = face.Glyph.LinearVerticalAdvance.ToInt32() - face.Glyph.BitmapTop ,
				XAdvance = face.Glyph.Advance.X.ToInt32(),
				Width = bmp.Width + 1,
				Height = bmp.Height + 1
			};
		}

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

        private static IntRectangle[] _genRects(IEnumerable<Glyph> glyphs)
        {
            //TODO// Back a bit more efficiently
            var x = 0;
            return glyphs.Select(glyph =>
            {
                var r = new IntRectangle(x, 0, glyph.Width + 1, glyph.Height + 1);
                x += glyph.Width + 1;
                return r;
            }).ToArray();
        }

#warning Clean up
        private static RegistryKey _fontsKey => Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts");
		

		internal static string GetTrueTypeFile(string fontFamilyName)
		{
			return (string)(_fontsKey.GetValue(fontFamilyName)
						 ?? _fontsKey.GetValue(fontFamilyName + " (TrueType)"));
		}


	}
}
*/