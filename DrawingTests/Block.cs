using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker;

namespace DrawingTests
{
	class Block : DraggableObject, ICollisionListener<Drawer>
	{
		private Color _col = Color.Black;

		public Block(double x, double y)
			: base(x, y)
		{
			Mask.Rectangle(30, 30);
		}

		public override void OnStep()
		{
			base.OnStep();
			Location = Mouse.Location;
		}

		public override void OnDraw()
		{
			Mask.DrawOutline(_col);
		}

		public void OnCollision(Drawer other)
		{
			_col = Color.Red;
		}
	}
}
