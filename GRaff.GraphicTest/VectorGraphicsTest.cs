using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;
using static GRaff.GMath;

namespace GRaff.GraphicTest
{
    [Test(Order = -1)]
    class VectorGraphicsTest : GameElement, IKeyPressListener
	{
        private const double dx = 100;
        private static Texture reference = Texture.Load("Assets/VectorGraphicsReference.png");
        private bool _referenceMode;

        public VectorGraphicsTest()
        {
            _setMode("Draw mode", false);
        }

        public override void OnDraw()
		{
            if (_referenceMode)
                Draw.Texture(reference, (0, 0));
            else
            {
                Draw.Clear(Colors.Black);

                Draw.Point((50, 50), Colors.Red);
                Draw.Line((120, 20), (180, 80), Colors.ForestGreen);
                Draw.Line(new Line(240, 20, 260, 80), Colors.ForestGreen);
                Draw.Line((380, 80), (320, 20), Colors.Blue, Colors.Red);
                Draw.Line((440, 80), (460, 20), Colors.Blue, Colors.Red);

                Draw.FillTriangle(new Point(550, 60) - new Vector(35, Angle.Deg(90)),
                                  new Point(550, 60) - new Vector(35, Angle.Deg(210)),
                                  new Point(550, 60) - new Vector(35, Angle.Deg(330)),
                                  Color.FromRgb(255, 0, 0), Color.FromRgb(0, 255, 0), Color.FromRgb(0, 0, 255));
                
                Draw.Primitive(PrimitiveType.TriangleFan, new[] {
                (new GraphicsPoint(650, 50), Colors.Gray),
                (new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(0)), Color.FromHsv(Angle.Deg(0), 1, 0.9)),
                (new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(60)), Color.FromHsv(Angle.Deg(60), 0.75, 0.75)),
                (new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(120)), Color.FromHsv(Angle.Deg(120), 1, 0.9)),
                (new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(180)), Color.FromHsv(Angle.Deg(180), 0.75, 0.75)),
                (new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(240)), Color.FromHsv(Angle.Deg(240), 1, 0.9)),
                (new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(300)), Color.FromHsv(Angle.Deg(300), 0.75, 0.75)),
                (new GraphicsPoint(650, 50) + new Vector(30, Angle.Deg(0)), Color.FromHsv(Angle.Deg(0), 1, 1)),
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


                Draw.Quadrilateral((10, 325), (85, 330), (90, 380), (5, 375), Colors.ForestGreen);
                Draw.Quadrilateral(new Quadrilateral((110, 325), (185, 330), (190, 380), (105, 375)), Colors.ForestGreen);
                Draw.Quadrilateral((210, 325), (285, 330), (290, 380), (205, 375), Colors.Red, Colors.Green, Colors.Blue, Colors.Purple);
                Draw.Quadrilateral(new Quadrilateral((310, 325), (385, 330), (305, 375), (390, 380)), Colors.Red, Colors.Green, Colors.Blue, Colors.Purple);

                Draw.FillQuadrilateral((410, 325), (485, 330), (490, 380), (405, 375), Colors.ForestGreen);
                Draw.FillQuadrilateral(new Quadrilateral((510, 325), (585, 330), (590, 380), (505, 375)), Colors.ForestGreen);
                Draw.FillQuadrilateral((610, 325), (685, 330), (690, 380), (605, 375), Colors.Red, Colors.Green, Colors.Blue, Colors.Purple);
                Draw.FillQuadrilateral(new Quadrilateral((710, 325), (785, 330), (790, 380), (705, 375)), Colors.Red, Colors.Green, Colors.Blue, Colors.Purple);

                Draw.FillQuadrilateral(new Quadrilateral((810, 325), (885, 330), (890, 380), (865, 345)), Colors.Red, Colors.Green, Colors.Blue, Colors.Purple);
                Draw.FillQuadrilateral(new Quadrilateral((910, 325), (985, 330), (905, 375), (990, 380)), Colors.Red, Colors.Green, Colors.Blue, Colors.Purple);


                Draw.Circle((50, 450), 40, Colors.ForestGreen);
                Draw.Ellipse((150, 450), (40, 40 / Phi), Colors.ForestGreen);
                Draw.Ellipse(((250 - 40 / Phi, 410), (80 / Phi, 80)), Colors.ForestGreen);

                Draw.FillCircle((350, 450), 40, Colors.DarkGreen);
                Draw.FillCircle((450, 450), 40, Colors.Red.Transparent(0.8), Colors.Blue.Transparent(0.5));
                Draw.FillEllipse(new Rectangle(510, 450 - 40 / Phi, 80, 80 / Phi), Colors.DarkGreen);
                Draw.FillEllipse((650, 450), (40 / Phi, 40), Colors.DarkGreen);
                Draw.FillEllipse(new Rectangle(710, 450 - 40 / Phi, 80, 80 / Phi), Colors.Red.Transparent(0.8), Colors.Blue.Transparent(0.5));
                Draw.FillEllipse((850, 450), (40 / Phi, 40), Colors.Red.Transparent(0.8), Colors.Blue.Transparent(0.5));

                for (int i = 0; i < 15; i++)
                {
                    var rect = new Rectangle(50 + 50 * i, 550 - i / 2.0, i, i);
                    Draw.Rectangle(rect, Colors.DarkSlateGray);
                    Draw.FillRectangle(rect + (0, 50), Colors.DarkSlateGray);
                    Draw.Ellipse(rect + (0, 100), Colors.DarkSlateGray);
                    Draw.FillEllipse(rect + (0, 150), Colors.DarkSlateGray);
                }
            }
        }

        private void _setMode(string name, bool referenceMode)
        {
            Window.Title = "Vector graphics test - " + name;
            this._referenceMode = referenceMode;
        }

        public void OnKeyPress(Key key)
        {
            switch (key)
            {
                case Key.Number1: _setMode("Draw mode", false); break;
                case Key.Number2: _setMode("Reference mode", true); break;
            }
        }
    }
}
