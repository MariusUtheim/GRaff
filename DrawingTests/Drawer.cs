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
			Image.Alpha = 1;
			Mask.Shape = MaskShape.Ellipse(60, 40);
			Background.Color = Color.LightGray;
			Sprite = Sprites.Xujia;
		}

		public override void OnStep()
		{
			//Transform.Rotation += Angle.Deg(1);
		}

		public override void OnDraw()
		{
			base.OnDraw();
			Vector offset = new Vector(50, Angle.Deg(45));
			Fill.Circle(Color.PeachPuff, Color.HotPink, View.Center + offset, 20);
			//Fill.Rectangle(Draw.GetPixel(Mouse.Location), 0, 0, 10, 10);
			for (int x = 0; x < 25; x++)
				for (int y = 0; y < 25; y++)
				{
					Point p = new Point(Mouse.X + x, Mouse.Y - 10 + y);
					Draw.Pixel(Draw.GetPixel(p), x, y);
				}
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
