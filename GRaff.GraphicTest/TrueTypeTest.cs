#if false
using GRaff.Graphics;
using GRaff.Graphics.Text;


namespace GRaff.GraphicTest
{
    [Test]
    class TrueTypeTest : GameElement
	{
        private Font _fontKerning, _fontNonKerning, _fontItalic, _fontBold, _fontBoldItalic;
        private Texture _rendered;
		
		public TrueTypeTest()
		{
			//_font = Font.LoadTrueType("Papyrus", 24, Font.ASCIICharacters);
			Font.LoadTrueTypeAsync("Times New Roman", 36, Font.ASCIICharacters, FontOptions.IgnoreKerning)
			    .ThenQueue(font => _fontNonKerning = font);
            _fontKerning = Font.LoadTrueType("/Library/Fonts/Times New Roman.ttf", 36, Font.ASCIICharacters);
            _fontBold = Font.LoadTrueType("Times New Roman", 36, Font.ASCIICharacters, FontOptions.Bold);
            _fontItalic = Font.LoadTrueType("TIMES NEW ROMAN", 36, Font.ASCIICharacters, FontOptions.Italic);
            _fontBoldItalic = Font.LoadTrueType("times new roman", 36, Font.ASCIICharacters, FontOptions.Bold | FontOptions.Italic);
            _rendered = _fontKerning.RenderText("This is rendered text");
		}

		public override void OnDraw()
		{
            Draw.Clear(Colors.ForestGreen);
            Draw.Texture(_rendered.SubTexture(), (0, 0), Colors.Red, Colors.Purple, Colors.Blue, Colors.Lime);

			if (_fontKerning != null)
				Draw.Text("AW This text uses kerning WA", _fontKerning, Alignment.Center, Window.Center);
			if (_fontNonKerning != null)
				Draw.Text("AW This text doesn't use kerning WA", _fontNonKerning, Alignment.Center, Window.Center + new Vector(0, 36), Colors.Black);

            Draw.Text("Bold text", _fontBold, Alignment.Center, Window.Center + new Vector(0, 72), Colors.Red);
            Draw.Text("Italic text", _fontItalic, Alignment.Center, Window.Center + new Vector(0, 108), Colors.Blue);
            Draw.Text("Bold and italic text", _fontBoldItalic, Alignment.Center, Window.Center + new Vector(0, 144), Colors.Purple);

		}
	}
}
#endif