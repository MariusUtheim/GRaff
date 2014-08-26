using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker;

namespace DrawingTests
{
	public class TestPoint : GameObject
	{

		public TestPoint(double x, double y)
			: base(x, y)
		{
			Depth = -1;
			Mask.Rectangle(1, 1);
		}

		public override void OnDraw()
		{
			if (Intersects(Instance<Drawer>.First))
				Draw.Point(Color.Red, X, Y);
			//Mask.DrawOutline(Intersects(Instance<Drawer>.First) ? Color.Red : Color.Black);
		}
	}
}
