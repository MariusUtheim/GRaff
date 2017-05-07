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
		private Font _font;

		public TrueTypeTest()
		{
			_font = Font.LoadTruetype("arial", 24, Font.ASCIICharacters, FontOptions.Italic | FontOptions.Bold);
		}

		public override void OnDraw()
		{
			Draw.Text("Lorem Ipsum Dolor Sit Amet", _font, FontAlignment.Center, Colors.Black, Room.Current.Center);
		}
	}
}
