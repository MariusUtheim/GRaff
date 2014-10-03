using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameMaker;
using OpenTK.Graphics.OpenGL4;

namespace DrawingTests
{
	public class Drawer : GameObject, IKeyPressListener, IGlobalMouseListener
	{

		public Drawer()
			: base(300, 400)
		{
			Depth = -1;
			Image.Alpha = 0.5;
			Mask.Shape = MaskShape.Ellipse(60, 40);
			Background.Color = Color.LightGray;
		}

		public override void OnStep()
		{
			Transform.Rotation += Angle.Deg(1);
			View.Rotation += Angle.Deg(0.1);
		}

		public override void OnDraw()
		{
			Fill.Circle(Color.Green, Color.Red, 150, 150, 100);
			Draw.Rectangle(Color.Black, Color.Red, Color.Blue, Color.Blue.Transparent(0), 10, 10, 100, 100);
			Draw.Line(Color.Red, Color.Blue, 20, 250, 120, 270);
			Draw.Circle(Color.Red, 500, 500, 100);
			//Draw.Polygon(Color.Red, new Polygon(new Point(500, 300), new Point(600, 400), new Point(600, 500), new Point(500, 600), new Point(400, 600), new Point(300, 500), new Point(300, 400), new Point(400, 300)));
		}


		public void OnKeyPress(Key key)
		{
		//	Sounds.Starlight.Play(false, 1.0, 1.0);
		}

		public void OnGlobalMouse(MouseButton button)
		{
			if (button == MouseButton.Left && Sprite == null)
			{
				Sprite = Sprites.Xujia;
				Sprite.Load();
			}
			else if (button == MouseButton.Right && Sprite != null)
			{
				Sprite.Unload();
				Sprite = null;
			}
		}
	}
}
