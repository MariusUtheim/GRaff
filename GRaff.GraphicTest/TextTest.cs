using System;
using GRaff.Graphics.Text;


namespace GRaff.GraphicTest
{
	[Test]
	class TextTest : GameElement
	{
		private const string lipsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\nPhasellus rutrum nibh sed nulla dapibus, sit amet tempor nulla feugiat.";
        private static Font _font = Font.Load(@"C:\test\TimesNewRoman.png", @"C:\test\TimesNewRoman.xml");
		private TextRenderer _render;
		private Sprite _renderedSprite;

		public TextTest()
		{
			_render = new TextRenderer(_font, lineWidth: 300);
			_render.Alignment = FontAlignment.TopLeft;
			
			_renderedSprite = new Sprite(_render.Render(lipsum).Texture);
		}


		public override void OnDraw()
		{
			Draw.Clear(Colors.LightGray);

			Draw.Text("Hello, world!", _render, Colors.Black, 10, 10);

			Draw.Rectangle(Colors.Black, 10, 50, 300, _font.Height);
			Draw.Text(_render.Truncate(lipsum), _render, Colors.Black, 10, 50);

			Draw.Text($"Line one\nLine two{Environment.NewLine}Line three", _render, Colors.Black, 10, 90);

			Draw.Rectangle(Colors.Black, 350, 10, 300, 300);
			Draw.Text(lipsum, _render, Colors.Black, 350, 10);

			if (_renderedSprite != null)
				Draw.Sprite(_renderedSprite, 0, Colors.Blue, new Transform
				{
					X = Room.Current.Center.X,
					Y = Room.Current.Center.Y + 100,
					Rotation = Angle.Deg(GMath.Sin(Time.LoopCount / 100.0)),
					XShear = 0.1 * GMath.Sin(Time.LoopCount / 10.0 / GMath.E),
					YShear = 0.2 * GMath.Cos(Time.LoopCount / 11.0 / GMath.Pi),
					XScale = 1 + 0.2 * GMath.Sin(Time.LoopCount / 10.0 / GMath.Phi),
					YScale = 1 + 0.2 * GMath.Sin(Time.LoopCount / 7.0 / GMath.Phi)
				});

		}
	}
}
