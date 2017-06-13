using GRaff.Graphics.Text;


namespace GRaff.GraphicTest
{
	//[Test]
    class TrueTypeTest : GameElement
	{
		private static Font _fontKerning = null, _fontNonKerning = null;
		
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
				Draw.Text("AWA LTL Lorem ipsum dolor sit amet", _fontKerning, FontAlignment.Center, Colors.Black, Room.Current.Center);
			if (_fontNonKerning != null)
				Draw.Text("AWA LTL Lorem ipsum dolor sit amet", _fontNonKerning, FontAlignment.Center, Colors.Black, Room.Current.Center + new Vector(0, 36));
		}
	}
}
