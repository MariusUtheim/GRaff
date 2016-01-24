using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GRaff.GMath;

namespace GRaff.GraphicTest
{
	[Test]
	class VectorGraphicsTest : GameElement
	{
		public override void OnDraw()
		{
			Draw.Clear(Colors.LightGray);

			Draw.Circle(Colors.Red, new Point(50, 50), 40);
			Draw.Circle(Colors.Red, 150, 50, -40);
			Draw.FillCircle(Colors.Blue, new Point(250, 50), 40);
			Draw.FillCircle(Colors.Blue.Transparent(0.5), 350, 50, 40);
			Draw.FillCircle(Colors.Red, Colors.Blue, new Point(450, 50), 40);
			Draw.FillCircle(Colors.Red.Transparent(0), Colors.Blue, 550, 50, 40);
			for (int i = 0; i < 12; i++)
			{
				Draw.Circle(Colors.Black, 650 + 30 * i, 25, i);
				Draw.FillCircle(Colors.Black, 650 + 30 * i, 70, i);
			}


			Draw.Rectangle(Colors.Red, 10, 150, 80, 80 / Phi);
			Draw.Rectangle(Colors.Red, Colors.Green, Colors.Blue, Colors.Purple, new Rectangle(110 + 80, 150 + 80 / Phi, -80, -80 / Phi));
			Draw.FillRectangle(Colors.Blue, new Point(210, 150), new Vector(80, 80 / Phi));
			Draw.FillRectangle(Colors.Blue.Transparent(0.5), new Rectangle(310, 150, 80, 80 / Phi));
			Draw.FillRectangle(Colors.Red.Transparent(0), Colors.Blue, Colors.Red.Transparent(0), Colors.Blue, new Rectangle(410, 150, 80, 80 / Phi));
			Draw.FillRectangle(Colors.Red, Colors.Green, Colors.Blue, Colors.Black.Transparent(0), 510, 150, 80, 80 / Phi);
			for (int i = 0; i < 12; i++)
			{
				var size = new Vector(i * Phi, i);
				Draw.Rectangle(Colors.Black, new Point(650 + 30 * i, 165 - i), size);
				Draw.FillRectangle(Colors.Black, new Point(650 + 30 * i, 190 - i), size);
			}


			Draw.Ellipse(Colors.Red, 10, 250, 80, 80 / Phi);
			Draw.Ellipse(Colors.Red, new Rectangle(110 + 80, 250 + 80 / Phi, -80, -80 / Phi));
			Draw.FillEllipse(Colors.Blue, new Point(210, 250), new Vector(80, 80 / Phi));
			Draw.FillEllipse(Colors.Blue.Transparent(0.5), new Rectangle(310, 250, 80, 80 / Phi));
			Draw.FillEllipse(Colors.Red, Colors.Blue, new Rectangle(410, 250, 80, 80 / Phi));
			Draw.FillEllipse(Colors.Red.Transparent(0), Colors.Blue, 510, 250, 80, 80 / Phi);
			for (int i = 0; i < 12; i++)
			{
				var size = new Vector(i * Phi, i);
				Draw.Ellipse(Colors.Black, new Point(650 + 30 * i, 265 - i), size);
				Draw.FillEllipse(Colors.Black, new Point(650 + 30 * i, 290 - i), size);
			}


		}
	}
}
