using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GRaff.GMath;

namespace GRaff.GraphicTest
{
	[Test(Order = -1)]
	class VectorGraphicsTest : GameElement
	{
        private const double dx = 100;

        public override void OnStep()
        {
            Window.Title = "VectorGraphicsTest - Fps: " + Time.Fps.ToString();
        }

        public override void OnDraw()
		{
            Draw.Clear(Colors.Black);
            
            Draw.Point(Colors.Red, (50, 50));
            Draw.Line(Colors.ForestGreen, (120, 20), (180, 80));
            Draw.Line(Colors.ForestGreen, new Line(240, 20, 260, 80));
            Draw.Line(Colors.Red, Colors.Blue, (380, 80), (320, 20));
            Draw.Line(Colors.Red, Colors.Blue, (440, 80), (460, 20));


            Draw.Triangle(Colors.ForestGreen, (40, 125), (80, 180), (10, 185));
            Draw.Triangle(Colors.ForestGreen, new Triangle((140, 125), (110, 185), (180, 180)));
            Draw.Triangle(Colors.Red, Colors.Blue, Colors.Green, (260, 125), (280, 180), (210, 145));
            Draw.Triangle(Colors.Red, Colors.Blue, Colors.Green, new Triangle((310, 145), (360, 125), (380, 180)));

            Draw.FillTriangle(Colors.DarkGreen, (440, 125), (480, 180), (410, 185));
            Draw.FillTriangle(Colors.DarkGreen, new Triangle((540, 125), (510, 185), (580, 180)));
            Draw.FillTriangle(Colors.Red, Colors.Blue, Colors.Invisible, (660, 125), (680, 180), (610, 145));
            Draw.FillTriangle(Colors.Red, Colors.Blue, Colors.Invisible, new Triangle((710, 145), (760, 125), (780, 180)));


            Draw.Rectangle(Colors.ForestGreen, (10, 225), (80, 80 / Phi));
            Draw.Rectangle(Colors.ForestGreen, new Rectangle(190, 225, -80, 80 / Phi));
            Draw.Rectangle(Colors.Red, Colors.Green, Colors.Blue, Colors.Purple, (210, 225), (80, 80 / Phi));
            Draw.Rectangle(Colors.Red, Colors.Green, Colors.Blue, Colors.Purple, new Rectangle(310 + 80, 225 + 80 / Phi, -80, -80 / Phi));

            Draw.FillRectangle(Colors.DarkGreen, (410, 225), (80, 80 / Phi));
            Draw.FillRectangle(Colors.DarkGreen, new Rectangle(590, 225, -80, 80 / Phi));
            Draw.FillRectangle(Colors.Invisible, Colors.Red, Colors.Blue, Colors.Purple, (610, 225), (80, 80 / Phi));
            Draw.FillRectangle(Colors.Invisible, Colors.Red, Colors.Blue, Colors.Purple, new Rectangle(710, 225 + 80 / Phi, 80, -80 / Phi));


            Draw.Circle(Colors.ForestGreen, (50, 350), 40);
            Draw.Ellipse(Colors.ForestGreen, (150, 350), (40, 40 / Phi));
            Draw.Ellipse(Colors.ForestGreen, ((250 - 40 / Phi, 310), (80 / Phi, 80)));

            Draw.FillCircle(Colors.DarkGreen, (350, 350), 40);
            Draw.FillCircle(Colors.Red.Transparent(0.8), Colors.Blue.Transparent(0.5), (450, 350), 40);
            Draw.FillEllipse(Colors.DarkGreen, new Rectangle(510, 350 - 40 / Phi, 80, 80 / Phi));
            Draw.FillEllipse(Colors.DarkGreen, (650, 350), (40 / Phi, 40));
            Draw.FillEllipse(Colors.Red.Transparent(0.8), Colors.Blue.Transparent(0.5), new Rectangle(710, 350 - 40 / Phi, 80, 80 / Phi));
            Draw.FillEllipse(Colors.Red.Transparent(0.8), Colors.Blue.Transparent(0.5), (850, 350), (40 / Phi, 40));

            for (int i = 0; i < 15; i++)
            {
                var rect = new Rectangle(50 + 50 * i, 450 - i / 2.0, i, i);
                Draw.Rectangle(Colors.DarkSlateGray, rect);
                Draw.FillRectangle(Colors.DarkSlateGray, rect + (0, 50));
                Draw.Ellipse(Colors.DarkSlateGray, rect + (0, 100));
                Draw.FillEllipse(Colors.DarkSlateGray, rect + (0, 150));
            }
        }
    }
}
