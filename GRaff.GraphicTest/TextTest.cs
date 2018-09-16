using System;
using GRaff.Graphics.Text;
using GRaff.Graphics;
using GRaff.Graphics.Text.TrueType;

namespace GRaff.GraphicTest
{
	[Test]
	class TextTest : GameElement, IKeyPressListener
	{
		private static Font _font;
		private static Font _preRenderedFont = Font.Load("Assets/Times New Roman.fnt");
		private static Font _trueTypeFont = TrueTypeFont.LoadRasterized("Times New Roman", TrueTypeFont.ASCIICharacters, 22);
		private TextRenderer _render;
		private Texture _renderedText;

		public TextTest()
		{
            Instance.Create(new Background { Color = Colors.LightGray });
			_font = _preRenderedFont;
			Window.Title = "TextTest - Pre-rendered";
			_render = new TextRenderer(_font, Alignment.TopLeft, 300);
            _renderedText = _render.Render("This text is prerendered. It has been turned into a texture, which makes it more efficent to draw and enables blending.");
		}

		public void OnKeyPress(Key key)
		{
			if (key == Key.Enter)
			{
				if (_font == _preRenderedFont)
				{
					_font = _trueTypeFont;
					Window.Title = "TextTest - TrueType";
				}
				else
				{
					_font = _preRenderedFont;
					Window.Title = "TextTest - Pre-rendered";
				}
				_render.Font = _font;
			}
		}

		public override void OnDraw()
		{
            Draw.Text(null, _render, (0, 0));
            Draw.Text("", _render, (0, 0));

			Draw.Text($"Line one\nLine two{Environment.NewLine}Line three\n\nSpace single\nSpace  double\nSpace   triple", _render, (10, 10), Colors.Black);
			
			Draw.Rectangle(new Rectangle((10, 200), (_render.LineWidth.Value, _font.Height)), Colors.Black);
			Draw.Text(_render.Truncate("@Truncated and completely contained in the textbox"), _render, (10, 200), Colors.Black);

            var largeText = "This is a large text.<br>\nThe textbox should automatically fit the width specified to the renderer, and the height of the produced text.";
			Draw.Rectangle(new Rectangle((10, 250), (_render.LineWidth.Value, _render.GetHeight(largeText))), Colors.Black);
			Draw.Text(largeText, _render, (10, 250), Colors.Black);

            Draw.Texture(_renderedText, (10, 400), Colors.Red, Colors.Aqua, Colors.Green, Colors.Black);

            Draw.Rectangle(new Rectangle((350, 10), (500, 500)), Colors.Black);

			using (View.TranslateTo((350, 10)).Use())
            {
                Draw.Line((0, 250), (500, 250), Colors.DarkGray);
                Draw.Line((250, 0), (250, 500), Colors.DarkGray);

                Draw.Text("Top Left", _font, (0, 0), Colors.Black, Alignment.TopLeft);
                Draw.Text("Top", _font, (250, 0), Colors.Black, Alignment.Top);
                Draw.Text("Top Right", _font, (500, 0), Colors.Black, Alignment.TopRight);
                Draw.Text("Left", _font, (0, 250), Colors.Black, Alignment.Left);
                Draw.Text("Center", _font, (250, 250), Colors.Black, Alignment.Center);
                Draw.Text("Right", _font, (500, 250), Colors.Black, Alignment.Right);
                Draw.Text("Bottom Left", _font, (0, 500), Colors.Black, Alignment.BottomLeft);
                Draw.Text("Bottom", _font, (250, 500), Colors.Black, Alignment.Bottom);
                Draw.Text("Bottom Right", _font, (500, 500), Colors.Black, Alignment.BottomRight);
            }

            

        }
	}
}
