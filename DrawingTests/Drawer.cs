﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameMaker;

namespace DrawingTests
{
	public class Drawer : GameObject, IKeyListener
	{
		int idx = 0;

		public Drawer()
		{
			Mask.Shape = MaskShape.Diamond(-25, -25, 50, 50);
			X = Y = 300;
			Depth = -1;
			Sprite = Sprites.Xujia;
//			for (int x = -24; x <= 24; x += 2)
//				for (int y = -24; y <= 24; y += 2)
//					Instance<TestPoint>.Create(X + x, Y + y);
		}

		public override void OnStep()
		{
			Transform.Rotation += Angle.Deg(1);
			Window.Title = Game.FPS.ToString();
		}

		public void OnKey(Key key)
		{
			Transform.Rotation += Angle.Deg(3);
		}
	}
}
