using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff;

namespace DrawingTests
{
	class Block : DraggableObject
	{
		private Color _col = Color.Black;

		public Block(double x, double y)
			: base(x, y)
		{
			Mask.Shape = MaskShape.Rectangle(30, 30);
			Depth = -5;
		}

		public override void OnDraw()
		{
			Fill.Rectangle(Color.Black, -15, -15, 30, 30);
			Mask.DrawOutline(Color.Red);
		}
	}
}
