using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameMaker;

namespace DrawingTests
{
	public class Drawer : DraggableObject, IKeyListener
	{
		public Drawer()
		{
			Mask.Shape = MaskShape.Rectangle(50, 50);
			X = Y = 300;
			Depth = -1;
		}

		public override void OnDraw()
		{
			for (int x = -24; x <= 24; x += 4)
				for (int y = -24; y <= 24; y += 4)
					Draw.Point(Color.Black, Transform.Point(x, y));

			Mask.DrawOutline(Color.Blue);
		}

		public void OnKey(Key key)
		{
			Transform.Rotation += Angle.Deg(3);
		}
	}
}
