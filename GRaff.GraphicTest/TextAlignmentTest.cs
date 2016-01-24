using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.GraphicTest
{
	[Test]
	class TextAlignmentTest : GameElement
	{
		private const string lipsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus rutrum nibh sed nulla dapibus, sit amet tempor nulla feugiat.";
        private Font _font;
		private TextRenderer _render;
		

		public TextAlignmentTest()
		{
			_font = Font.Load(@"C:\test\TimesNewRoman.png", @"C:\test\TimesNewRoman.xml");
			_render = new TextRenderer(_font, width: 300);
			_render.Alignment = FontAlignment.Center;
			
			var multiline = _render.MultilineFormat(lipsum);
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
			Draw.Text(_render.Alignment.ToString(), _render, Colors.DarkGray, Mouse.Location);

			Draw.Line(Colors.Black, 0, Room.Current.Center.Y, Room.Current.Width, Room.Current.Center.Y);
			Draw.Line(Colors.Black, Room.Current.Center.X, 0, Room.Current.Center.X, Room.Current.Height);
			//_render.Draw(lipsum, Colors.White, Room.Current.Center);

			//Draw.Rectangle(Colors.Black, 10, 50, 300, _font.Height);
			Draw.Text(_render.Truncate(lipsum), _render, Colors.Black, 10, 50);

			Draw.Text("Top Left", _font, FontAlignment.TopLeft, Colors.Black, 0, 0);
			Draw.Text("Top", _font, FontAlignment.Top, Colors.Black, Room.Current.Center.X, 0);
			Draw.Text("Top Right",  _font, FontAlignment.TopRight, Colors.Black, Room.Current.Width, 0);
			Draw.Text("Left", _font, FontAlignment.Left, Colors.Black, 0, Room.Current.Center.Y);
			Draw.Text("Center", _font, FontAlignment.Center, Colors.Black, Room.Current.Center);
			Draw.Text("Right", _font, FontAlignment.Right, Colors.Black, Room.Current.Width, Room.Current.Center.Y);
			Draw.Text("Bottom Left", _font, FontAlignment.BottomLeft, Colors.Black, 0, Room.Current.Height);
			Draw.Text("Bottom", _font, FontAlignment.Bottom, Colors.Black, Room.Current.Center.X, Room.Current.Height);
			Draw.Text("Bottom Right", _font, FontAlignment.BottomRight, Colors.Black, Room.Current.Width, Room.Current.Height);
		}

	}
}
