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
		}

		public override void OnStep()
		{
			Transform.Rotation += Angle.Deg(1);
			View.Rotation += Angle.Deg(0.1);

			byte v = (byte)(127.5 * (1 + GMath.Sin(Time.LoopCount / 60.0)));
			Background.Color = new Color(v, v, v);
		}

		public override void OnDraw()
		{
			Fill.Rectangle(Color.Green, -0.5, -0.5, 0.5, 0.5);
			Fill.Rectangle(Color.Red.Transparent(0), Color.Red, Color.Blue, Color.Blue.Transparent(0), 10, 10, 100, 100);
			Fill.Circle(Color.Aqua, Color.Aquamarine, 150, 150, 200);
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
