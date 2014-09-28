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
			View.Rotation += Angle.Deg(0.1);
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
