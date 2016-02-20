using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using SharpFont;
using SysColor = System.Drawing.Color;
using SysGraphics = System.Drawing.Graphics;
using SysRectangle = System.Drawing.Rectangle;

namespace GRaff.Graphics.Text
{
    static partial class FontLoader
    {
#warning Something is wrong with the alignment
		public static Font LoadTrueType(FileInfo file, int size, ISet<char> charSet)
		{
			var lib = new Library();
			var face = new Face(lib, file.FullName);
			face.SetPixelSizes((uint)size, 0);

			var glyphs = charSet.Select(c => makeFontChar(face, c)).ToArray();
			var rects = RectanglePacker.Map(glyphs.Select(glyph => new IntVector(glyph.Width, glyph.Height)).ToArray()).ToArray();

			var bmp = new Bitmap(rects.Max(r => r.Right), rects.Max(r => r.Bottom));
			var g = SysGraphics.FromImage(bmp);
			g.Clear(SysColor.Transparent);

			for (var i = 0; i < glyphs.Length; i++)
			{
				g.DrawImage(glyphs[i].Image, (float)rects[i].Left, (float)rects[i].Top);
			}

			var bmpData = bmp.LockBits(new SysRectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			var buffer = new TextureBuffer(bmpData.Width, bmpData.Height, bmpData.Scan0);
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
			return new Font(buffer, new FontFile { Chars = chars.ToList(), Common = new FontCommon { LineHeight = face.Glyph.Metrics.VerticalAdvance.ToInt32() }, Info = null, Kernings = null, Pages = null });
		}

		/*
				private static Bitmap crop(Bitmap bitmap, SysColor backColor)
				{
					int top, bot, left, right;
					bool willContinue;

					for (top = 0, willContinue = true; willContinue; top++)
					{
						for (int x = 0; x < bitmap.Width; x++)
							if (bitmap.GetPixel(x, top) != backColor)
							{
								willContinue = false;
								break;
							}
					}
					for (bot = bitmap.Height - 1, willContinue = false; willContinue; bot--)
					{
						for (int x = 0; x < bitmap.Width; x++)
							if (bitmap.GetPixel(x, bot) != backColor)
							{
								willContinue = false;
								break;
							}
					}

					for (left = 0, willContinue = true; willContinue; left++)
					{
						for (int y = 0; y < bitmap.Height; y++)
							if (bitmap.GetPixel(left, y) != backColor)
							{
								willContinue = false;
								break;
							}
					}
					for (right = bitmap.Width - 1, willContinue = false; willContinue; right--)
					{
						for (int y = 0; y < bitmap.Height; y++)
							if (bitmap.GetPixel(right, y) != backColor)
							{
								willContinue = false;
								break;
							}
					}

					var srcRect = new SysRectangle(left, top, right - left + 1, bot - top + 1);
					var dstRect = new SysRectangle(srcRect.X - 1, srcRect.Y - 1, srcRect.Width + 2, srcRect.Height + 2);
					var result = new Bitmap(dstRect.Width, dstRect.Height);
					SysGraphics.FromImage(result).DrawImage(bitmap, dstRect, srcRect, GraphicsUnit.Pixel);
					return result;
				}
		*/

		private static Glyph makeFontChar(Face face, char c)
		{
			//var gIdx = face.GetCharIndex(c);
			face.LoadChar(c, LoadFlags.Default, LoadTarget.Normal);
			face.Glyph.RenderGlyph(RenderMode.Light);
			var bmp = (face.Glyph.Bitmap.Width == 0) ? new Bitmap(1, 1) : face.Glyph.Bitmap.ToGdipBitmap();
			
			return new Glyph
			{
				Character = c,
				Image = bmp,
				XOffset = face.Glyph.BitmapLeft,
				YOffset = face.Glyph.LinearVerticalAdvance.ToInt32() - face.Glyph.BitmapTop,
				XAdvance = face.Glyph.Advance.X.ToInt32(),
				Width = bmp.Width,
				Height = bmp.Height
			};
		}

		public static IEnumerable<string> GetFontNames()
			=> new InstalledFontCollection().Families.Select(f => f.Name);



	}
}
