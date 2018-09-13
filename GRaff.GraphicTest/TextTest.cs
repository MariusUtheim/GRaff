using System;
using GRaff.Graphics.Text;
using GRaff.Graphics;
using GRaff.Graphics.Text.TrueType;

namespace GRaff.GraphicTest
{
	[Test]
	class TextTest : GameElement
	{
        private static Font _font = Font.Load("Assets/Times New Roman.fnt");
		private TextRenderer _render;
		private Texture _renderedText;

		public TextTest()
		{
            Instance.Create(new Background { Color = Colors.LightGray });
			_render = new TextRenderer(_font, Alignment.TopLeft, 300);
            _renderedText = _render.Render("This text is prerendered. It has been turned into a texture, which makes it more efficent to draw and enables blending.");
		}


		public override void OnDraw()
		{
			Draw.Text($"Line one\nLine two{Environment.NewLine}Line three", _render, (10, 50), Colors.Black);
			
			Draw.Rectangle(new Rectangle((10, 150), (_render.LineWidth.Value, _font.Height)), Colors.Black);
			Draw.Text(_render.Truncate("Truncated and completely contained in the textbox"), _render, (10, 150), Colors.Black);

            var largeText = "This is a large text.<br>\nThe textbox should automatically fit the width specified to the renderer, and the height of the produced text.";
			Draw.Rectangle(new Rectangle((10, 200), (_render.LineWidth.Value, _render.GetHeight(largeText))), Colors.Black);
			Draw.Text(largeText, _render, (10, 200), Colors.Black);

            Draw.Texture(_renderedText.SubTexture(), (10, 350), Colors.Red, Colors.Aqua, Colors.Green, Colors.Black);

            Draw.Rectangle(new Rectangle((350, 10), (500, 500)), Colors.Black);

            using (View.DrawTo((350, 10)).Use())
            {
                Draw.Line((0, 250), (500, 250), Colors.DarkGray);
                Draw.Line((250, 0), (250, 500), Colors.DarkGray);

                Draw.Text("Top Left", _font, Alignment.TopLeft, (0, 0), Colors.Black);
                Draw.Text("Top", _font, Alignment.Top, (250, 0), Colors.Black);
                Draw.Text("Top Right", _font, Alignment.TopRight, (500, 0), Colors.Black);
                Draw.Text("Left", _font, Alignment.Left, (0, 250), Colors.Black);
                Draw.Text("Center", _font, Alignment.Center, (250, 250), Colors.Black);
                Draw.Text("Right", _font, Alignment.Right, (500, 250), Colors.Black);
                Draw.Text("Bottom Left", _font, Alignment.BottomLeft, (0, 500), Colors.Black);
                Draw.Text("Bottom", _font, Alignment.Bottom, (250, 500), Colors.Black);
                Draw.Text("Bottom Right", _font, Alignment.BottomRight, (500, 500), Colors.Black);
            }

            

        }
	}
}
