using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics.Text;

namespace GRaff.GraphicTest
{
	[Test]
    class TrueTypeTest : GameElement
	{
		private static Font _fontKerning = null, _fontNonKerning = null;
		private static Font _pngFont = Font.Load(@"C:\test\TimesNewRoman.png", @"C:\test\TimesNewRoman.xml");


		public TrueTypeTest()
		{
			//_font = Font.LoadTrueType("Papyrus", 24, Font.ASCIICharacters);
			Font.LoadTrueTypeAsync("Times New Roman", 36, Font.ASCIICharacters)
				.ThenQueue(font => _fontKerning = font);
			Font.LoadTrueTypeAsync("Times New Roman", 36, Font.ASCIICharacters, FontOptions.IgnoreKerning)
				.ThenQueue(font => _fontNonKerning = font);
		}

		public override void OnDraw()
		{
			if (_fontKerning != null)
				Draw.Text("AWA LTL Lorem ipsum dolor sit amet", _fontKerning, FontAlignment.TopLeft, Colors.Black, Room.Current.Center);
			if (_fontNonKerning != null)
				Draw.Text("AWA LTL Lorem ipsum dolor sit amet", _fontNonKerning, FontAlignment.TopLeft, Colors.Black, Room.Current.Center + new Vector(0, 36));
			
			Draw.Text("Lorem ipsum dolor sit amet", _pngFont, FontAlignment.TopLeft, Colors.Black, Room.Current.Center + new Vector(0, -36));
		}
	}
}
