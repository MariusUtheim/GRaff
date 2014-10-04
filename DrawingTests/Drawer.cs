using GameMaker;


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
		}

		public override void OnStep()
		{
			//Transform.Rotation += Angle.Deg(1);
		}

		public override void OnDraw()
		{
			Fill.Rectangle(0xFFFFFFFF, 0xFFFF00FF, 0xFFFFFF00, 0xFF00FFFF, 10, 10, 200, 120);
			Fill.Circle(0x80FFFF00, 0xFF00FFFF, 105, 250, 100);

			Draw.Line(Color.Black, Color.Red, 10, 400, 210, 410);
			Draw.Sprite(300, 20, Sprites.Xujia, 0);
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
