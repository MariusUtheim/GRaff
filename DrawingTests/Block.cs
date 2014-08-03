using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker;

namespace DrawingTests
{
	class Block : DraggableObject
	{
		private Color _col = Color.Black;

		public Block(double x, double y)
			: base(x, y)
		{
			Mask.Shape = MaskShape.Rectangle(30, 30);
		}

		public override void OnDraw()
		{
			Mask.DrawOutline(Intersects(Instance<Drawer>.First) ? Color.Red : Color.Black);
		}
	}
}
