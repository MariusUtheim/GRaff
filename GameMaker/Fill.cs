using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class Fill
	{

		public static void Circle(Color color, Point location, double radius)
		{
			throw new NotImplementedException();
		}

		public static void Rectangle(Color color, double x, double y, double width, double height)
		{
			Draw.CurrentSurface.FillRectangle(color, x, y, width, height);
		}

		public static void Rectangle(Color color, Rectangle rectangle)
		{
			Draw.CurrentSurface.FillRectangle(color, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			throw new NotImplementedException();
		}

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle)
		{
			throw new NotImplementedException();
		}

	}
}
