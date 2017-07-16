using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;
using static GRaff.GMath;

namespace GRaff.GraphicTest
{
    [Test(Order = 1)]
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
            
            Draw.Point((50, 50), Colors.Red);
            Draw.Line((120, 20), (180, 80), Colors.ForestGreen);
            Draw.Line(new Line(240, 20, 260, 80), Colors.ForestGreen);
            Draw.Line((380, 80), (320, 20), Colors.Blue);
            Draw.Line((440, 80), (460, 20), Colors.Blue);

            Draw.FillTriangle(new Point(550, 60) - new Vector(35, Angle.Deg(90)),
                              new Point(550, 60) - new Vector(35, Angle.Deg(210)),
                              new Point(550, 60) - new Vector(35, Angle.Deg(330)),
                              Color.Rgb(255, 0, 0), Color.Rgb(0, 255, 0), Color.Rgb(0, 0, 255));
            
            Draw.Primitive(PrimitiveType.TriangleFan, new[] {
                (new GraphicsPoint(650, 50), Colors.Gray),
				(new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(0)), Color.Hsv(Angle.Deg(0), 1, 0.9)),
			    (new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(60)), Color.Hsv(Angle.Deg(60), 0.75, 0.75)),
				(new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(120)), Color.Hsv(Angle.Deg(120), 1, 0.9)),
				(new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(180)), Color.Hsv(Angle.Deg(180), 0.75, 0.75)),
				(new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(240)), Color.Hsv(Angle.Deg(240), 1, 0.9)),
				(new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(300)), Color.Hsv(Angle.Deg(300), 0.75, 0.75)),
				(new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(0)), Color.Hsv(Angle.Deg(0), 1, 1)),
				});


            Draw.Triangle((40, 125), (80, 180), (10, 185), Colors.ForestGreen);
            Draw.Triangle(new Triangle((140, 125), (110, 185), (180, 180)), Colors.ForestGreen);
            Draw.Triangle((260, 125), (280, 180), (210, 145), Colors.Red, Colors.Blue, Colors.Green);
            Draw.Triangle(new Triangle((310, 145), (360, 125), (380, 180)), Colors.Red, Colors.Blue, Colors.Green);

            Draw.FillTriangle((440, 125), (480, 180), (410, 185), Colors.DarkGreen);
            Draw.FillTriangle(new Triangle((540, 125), (510, 185), (580, 180)), Colors.DarkGreen);
            Draw.FillTriangle((660, 125), (680, 180), (610, 145), Colors.Red, Colors.Blue, Colors.Invisible);
            Draw.FillTriangle(new Triangle((710, 145), (760, 125), (780, 180)), Colors.Red, Colors.Blue, Colors.Invisible);


            Draw.Rectangle((10, 225), (80, 80 / Phi), Colors.ForestGreen);
            Draw.Rectangle(new Rectangle(190, 225, -80, 80 / Phi), Colors.ForestGreen);
            Draw.Rectangle((210, 225), (80, 80 / Phi), Colors.Red, Colors.Green, Colors.Blue, Colors.Purple);
            Draw.Rectangle(new Rectangle(310 + 80, 225 + 80 / Phi, -80, -80 / Phi), Colors.Red, Colors.Green, Colors.Blue, Colors.Purple);

            Draw.FillRectangle((410, 225), (80, 80 / Phi), Colors.DarkGreen);
            Draw.FillRectangle(new Rectangle(590, 225, -80, 80 / Phi), Colors.DarkGreen);
            Draw.FillRectangle((610, 225), (80, 80 / Phi), Colors.Invisible, Colors.Red, Colors.Blue, Colors.Purple);
            Draw.FillRectangle(new Rectangle(710, 225 + 80 / Phi, 80, -80 / Phi), Colors.Invisible, Colors.Red, Colors.Blue, Colors.Purple);


            Draw.Circle((50, 350), 40, Colors.ForestGreen);
            Draw.Ellipse((150, 350), (40, 40 / Phi), Colors.ForestGreen);
            Draw.Ellipse(((250 - 40 / Phi, 310), (80 / Phi, 80)), Colors.ForestGreen);

            Draw.FillCircle((350, 350), 40, Colors.DarkGreen);
            Draw.FillCircle((450, 350), 40, Colors.Red.Transparent(0.8), Colors.Blue.Transparent(0.5));
            Draw.FillEllipse(new Rectangle(510, 350 - 40 / Phi, 80, 80 / Phi), Colors.DarkGreen);
            Draw.FillEllipse((650, 350), (40 / Phi, 40), Colors.DarkGreen);
            Draw.FillEllipse(new Rectangle(710, 350 - 40 / Phi, 80, 80 / Phi), Colors.Red.Transparent(0.8), Colors.Blue.Transparent(0.5));
            Draw.FillEllipse((850, 350), (40 / Phi, 40), Colors.Red.Transparent(0.8), Colors.Blue.Transparent(0.5));

            for (int i = 0; i < 15; i++)
            {
                var rect = new Rectangle(50 + 50 * i, 450 - i / 2.0, i, i);
                Draw.Rectangle(rect, Colors.DarkSlateGray);
                Draw.FillRectangle(rect + (0, 50), Colors.DarkSlateGray);
                Draw.Ellipse(rect + (0, 100), Colors.DarkSlateGray);
                Draw.FillEllipse(rect + (0, 150), Colors.DarkSlateGray);
            }
        }
    }
}
