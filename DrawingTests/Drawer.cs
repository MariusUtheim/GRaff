using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameMaker;
using OpenTK.Graphics.OpenGL;

namespace DrawingTests
{
	public class Drawer : GameObject, IKeyListener, IGlobalMouseListener
	{
		int idx = 0;
		double zoom = 1;

		public Drawer()
			: base(300, 400)
		{
			Mask.Diamond(-25, -25, 50, 50);
			Depth = -1;
			Background.Color = Color.AliceBlue;
			Sprite = Sprites.Xujia;
		}

		public override void OnStep()
		{
		}

		public override void OnDraw()
		{
			base.OnDraw();
		}


		public void OnKey(Key key)
		{
			Transform.Rotation += Angle.Deg(3);
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
