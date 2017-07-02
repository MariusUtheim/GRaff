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
			_render.Alignment = FontAlignment.Center;
		}

		public override void OnStep()
		{
			var alignment = FontAlignment.Center;
			if (Mouse.X < Room.Current.Width / 3)
				alignment = FontAlignment.Left;
			else if (Mouse.X > Room.Current.Width * 2 / 3)
				alignment = FontAlignment.Right;
			if (Mouse.Y < Room.Current.Height / 3)
				alignment |= FontAlignment.Top;
			else if (Mouse.Y > Room.Current.Height * 2 / 3)
				alignment |= FontAlignment.Bottom;
			_render.Alignment = alignment;
		}

		public override void OnDraw()
		{
			Draw.Clear(Colors.White);
			Draw.Text(_render.Alignment.ToString(), _render, Mouse.Location, Colors.DarkGray);

			Draw.Line((0, Room.Current.Center.Y), (Room.Current.Width, Room.Current.Center.Y), Colors.Black);
			Draw.Line((Room.Current.Center.X, 0), (Room.Current.Center.X, Room.Current.Height), Colors.Black);
			//_render.Draw(lipsum, Colors.White, Room.Current.Center);

			//Draw.Rectangle(Colors.Black, 10, 50, 300, _font.Height);
			//Draw.Text(_render.Truncate(lipsum), _render, Colors.Black, 10, 50);

            Draw.Text("Top Left", font, FontAlignment.TopLeft, (0, 0), Colors.Black);
            Draw.Text("Top", font, FontAlignment.Top, (Room.Current.Center.X, 0), Colors.Black);
            Draw.Text("Top Right",  font, FontAlignment.TopRight, (Room.Current.Width, 0), Colors.Black);
            Draw.Text("Left", font, FontAlignment.Left, (0, Room.Current.Center.Y), Colors.Black);
			Draw.Text("Center", font, FontAlignment.Center, Room.Current.Center, Colors.Black);
            Draw.Text("Right", font, FontAlignment.Right, (Room.Current.Width, Room.Current.Center.Y), Colors.Black);
            Draw.Text("Bottom Left", font, FontAlignment.BottomLeft, (0, Room.Current.Height), Colors.Black);
            Draw.Text("Bottom", font, FontAlignment.Bottom, (Room.Current.Center.X, Room.Current.Height), Colors.Black);
            Draw.Text("Bottom Right", font, FontAlignment.BottomRight, (Room.Current.Width, Room.Current.Height), Colors.Black);
		}

	}
}
