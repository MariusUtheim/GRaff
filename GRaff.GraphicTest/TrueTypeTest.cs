using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics.Text;

namespace GRaff.GraphicTest
{
	[Test(Order = -1)]
    class TrueTypeTest : GameElement
	{
		private Font _fontKerning = null, _fontNonKerning = null;

		public TrueTypeTest()
		{
			//_font = Font.LoadTrueType("Papyrus", 24, Font.ASCIICharacters);
			Font.LoadTrueTypeAsync("Times New Roman", 24, Font.ASCIICharacters)
				.ThenQueue(font => _fontKerning = font);
			Font.LoadTrueTypeAsync("Times New Roman", 24, Font.ASCIICharacters, FontOptions.IgnoreKerning)
				.ThenQueue(font => _fontNonKerning = font);
		}

		public override void OnDraw()
		{
			if (_fontKerning != null)
				Draw.Text("This text is with kerning AW WA AWA", _fontKerning, FontAlignment.Center, Colors.Black, Room.Current.Center);
			if (_fontNonKerning != null)
				Draw.Text("This text is without kerning AW WA AWA", _fontNonKerning, FontAlignment.Center, Colors.Black, Room.Current.Center + new Vector(0, 36));
		}
	}
}
