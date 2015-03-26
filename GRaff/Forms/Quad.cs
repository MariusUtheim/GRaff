using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Forms
{
	public class Quad : DisplayObject
	{
		public Color Color { get; set; }

		public override void OnPaint()
		{
			Draw.FillRectangle(Color, Region);
		}
	}
}
