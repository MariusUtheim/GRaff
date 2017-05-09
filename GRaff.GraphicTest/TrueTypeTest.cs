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
		private Font _font = null;

		public TrueTypeTest()
		{
			//_font = Font.LoadTrueType("Papyrus", 24, Font.ASCIICharacters);
			Font.LoadTrueTypeAsync("Papyrus", 24, Font.ASCIICharacters)
				.ThenQueue(font => _font = font);
		}

		public override void OnDraw()
		{
			if (_font != null)
				Draw.Text("Lorem Ipsum dolor sit amet", _font, FontAlignment.Center, Colors.Black, Room.Current.Center);
		}
	}
}
