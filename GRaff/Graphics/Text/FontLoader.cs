// ---- AngelCode BmFont XML serializer ----------------------
// ---- By DeadlyDan @ deadlydan@gmail.com -------------------
// ---- There's no license restrictions, use as you will. ----
// ---- Credits to http://www.angelcode.com/ -----------------

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using SysFont = System.Drawing.Font;


namespace GRaff.Graphics.Text
{

	internal static class FontLoader
	{
		private static XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));

		public static FontFile Load(string filename)
		{
			using (var textReader = new FileStream(filename, FileMode.Open, FileAccess.Read))
				return (FontFile)deserializer.Deserialize(textReader);
		}


		[StructLayout(LayoutKind.Sequential)]
		struct TEXTMETRIC
		{
			long tmHeight, tmAscent, tmDescent, tmInternalLeading, tmExternalLeading,
				tmAveCharWidth, tmMaxCharWidth, tmWeight, tmOverhang, tmDigitizedAspectX, tmDigitizedAspectY;
			char tmFirstChar, tmLastChar, tmDefaultChar, tmBreakChar;
			byte tmItalic, tmUnderlined, tmStruckOut, tmPitchAndFamily, tmCharSet;
		}
		[DllImport("Gdi32.dll")]
		private static extern bool GetTextMetrics(IntPtr hdc, out TEXTMETRIC tm);

		public static void TextMetrics()
		{
			var f = new SysFont("Arial", 12);
			TEXTMETRIC tm;
			GetTextMetrics(f.ToHfont(), out tm);

			Console.WriteLine(tm);
		}




	}
}