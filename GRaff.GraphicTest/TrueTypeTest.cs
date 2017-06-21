using GRaff.Graphics.Text;


namespace GRaff.GraphicTest
{
    [Test(Order = -1)]
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
            Draw.Clear(Colors.ForestGreen);
			if (_fontKerning != null)
				Draw.Text("AW This text uses kerning WA", _fontKerning, FontAlignment.Center, Room.Current.Center, Colors.Black);
			if (_fontNonKerning != null)
				Draw.Text("AW This text doesn't use kerning WA", _fontNonKerning, FontAlignment.Center, Room.Current.Center + new Vector(0, 36), Colors.Black);
		}
	}
}
