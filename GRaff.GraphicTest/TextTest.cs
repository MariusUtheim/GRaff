using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.GraphicTest
{
	class TextTest : Test
	{
		private const string lipsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\nPhasellus rutrum nibh sed nulla dapibus, sit amet tempor nulla feugiat.";
        private Font _font;
		private TextRenderer _render;

		public TextTest()
		{
			_font = Font.Load(@"C:\test\TimesNewRoman.png", @"C:\test\TimesNewRoman.xml");
			_render = new TextRenderer(_font, width: 300);
			_render.Alignment = FontAlignment.TopLeft;
			
			var multiline = _render.MultilineFormat(lipsum);
		}


		public override void OnDraw()
		{
			Draw.Clear(Colors.White);

			Draw.Text("Hello, world!", _render, Colors.Black, 10, 10);

			Draw.Rectangle(Colors.Black, 10, 50, 300, _font.Height);
			Draw.Text(_render.Truncate(lipsum), _render, Colors.Black, 10, 50);

			Draw.Text($"Hello\nWorld{Environment.NewLine}Newline", _render, Colors.Black, 10, 90);

			Draw.Rectangle(Colors.Black, 350, 10, 300, 300);
			Draw.Text(lipsum, _render, Colors.Black, 350, 10);
		}

	}
}
