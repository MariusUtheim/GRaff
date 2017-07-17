using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;
using GRaff.Graphics.Text;

namespace GRaff.GraphicTest
{
	//[Test]
	class TextAlignmentTest : GameElement
	{
		private const string lipsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus rutrum nibh sed nulla dapibus, sit amet tempor nulla feugiat.";
        private static Font font = Font.Load(@"C:\test\TimesNewRoman.png", @"C:\test\TimesNewRoman.xml");
		private TextRenderer _render;
		

		public TextAlignmentTest()
		{
			_render = new TextRenderer(font, lineWidth: 300);
			_render.Alignment = Alignment.Center;
		}

		public override void OnStep()
		{
			var alignment = Alignment.Center;
			if (Mouse.X < Window.Width / 3)
				alignment = Alignment.Left;
			else if (Mouse.X > Window.Width * 2 / 3)
				alignment = Alignment.Right;
			if (Mouse.Y < Window.Height / 3)
				alignment |= Alignment.Top;
			else if (Mouse.Y > Window.Height * 2 / 3)
				alignment |= Alignment.Bottom;
			_render.Alignment = alignment;
		}

		public override void OnDraw()
		{
			Draw.Clear(Colors.White);
			Draw.Text(_render.Alignment.ToString(), _render, Mouse.Location, Colors.DarkGray);

			Draw.Line((0, Window.Center.Y), (Window.Width, Window.Center.Y), Colors.Black);
			Draw.Line((Window.Center.X, 0), (Window.Center.X, Window.Height), Colors.Black);
			//_render.Draw(lipsum, Colors.White, Room.Current.Center);

			//Draw.Rectangle(Colors.Black, 10, 50, 300, _font.Height);
			//Draw.Text(_render.Truncate(lipsum), _render, Colors.Black, 10, 50);

            Draw.Text("Top Left", font, Alignment.TopLeft, (0, 0), Colors.Black);
            Draw.Text("Top", font, Alignment.Top, (Window.Center.X, 0), Colors.Black);
            Draw.Text("Top Right",  font, Alignment.TopRight, (Window.Width, 0), Colors.Black);
            Draw.Text("Left", font, Alignment.Left, (0, Window.Center.Y), Colors.Black);
			Draw.Text("Center", font, Alignment.Center, Window.Center, Colors.Black);
            Draw.Text("Right", font, Alignment.Right, (Window.Width, Window.Center.Y), Colors.Black);
            Draw.Text("Bottom Left", font, Alignment.BottomLeft, (0, Window.Height), Colors.Black);
            Draw.Text("Bottom", font, Alignment.Bottom, (Window.Center.X, Window.Height), Colors.Black);
            Draw.Text("Bottom Right", font, Alignment.BottomRight, (Window.Width, Window.Height), Colors.Black);
		}

	}
}
