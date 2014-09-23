using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameMaker;
using OpenTK.Graphics.OpenGL;

namespace DrawingTests
{
	public class Drawer : GameObject, IKeyPressListener, IGlobalMouseListener
	{
		double zoom = 1;

		public Drawer()
			: base(300, 400)
		{
			Depth = -1;
			Background.Color = Color.ForestGreen;
			Sprite = Sprites.Xujia;
			Image.Alpha = 0.5;
			Mask.Shape = MaskShape.Ellipse(60, 40);
		}

		public override void OnStep()
		{
			Transform.Rotation += Angle.Deg(1);
		}

		public override void OnDraw()
		{
			base.OnDraw();
			Mask.DrawOutline();
			Fill.Circle(Color.Red, Color.Green, Location, 50);
			Draw.Circle(Color.Black, Location, 50);

		}


		public void OnKeyPress(Key key)
		{
			Sounds.Starlight.Play(false, 1.0, 1.0);
		}

		public void OnGlobalMouse(MouseButton button)
		{
			switch (button)
			{
				case MouseButton.Left:
					if (zoom < 4.0)
						zoom += 0.02;
					break;

				case MouseButton.Right:
					if (zoom > 1.0)
						zoom -= 0.02;
					break;

				default:
					return;
			}

			View.Width = Room.Width / zoom;
			View.Height = Room.Height / zoom;
	//		View.Center = Mouse.Location;
			
		}
	}
}
