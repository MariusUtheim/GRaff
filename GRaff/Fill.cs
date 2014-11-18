using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public static class Fill
	{
		public static void Circle(Color color, Point location, double radius)
		{
			Draw.CurrentSurface.FillCircle(color, color, location, radius);
		}

		public static void Circle(Color color, double x, double y, double radius)
		{
			Draw.CurrentSurface.FillCircle(color, color, new Point(x, y), radius);
		}

		public static void Circle(Color col1, Color col2, Point location, double radius)
		{
			Draw.CurrentSurface.FillCircle(col1, col2, location, radius);
		}

		public static void Circle(Color col1, Color col2, double x, double y, double radius)
		{
			Draw.CurrentSurface.FillCircle(col1, col2, new Point(x, y), radius);
		}

		public static void Triangle(Color color, double x1, double y1, double x2, double y2, double x3, double y3)
		{
			Draw.CurrentSurface.FillTriangle(color, color, color, new Point(x1, y1), new Point(x2, y2), new Point(x3, y3));
		}

		public static void Triangle(Color color, Point p1, Point p2, Point p3)
		{
			Draw.CurrentSurface.FillTriangle(color, color, color, p1, p2, p3);
		}

		public static void Triangle(Color col1, Color col2, Color col3, double x1, double y1, double x2, double y2, double x3, double y3)
		{
			Draw.CurrentSurface.FillTriangle(col1, col2, col3, new Point(x1, y1), new Point(x2, y2), new Point(x3, y3));
		}

		public static void Triangle(Color col1, Color col2, Color col3, Point p1, Point p2, Point p3)
		{
			Draw.CurrentSurface.FillTriangle(col1, col2, col3, p1, p2, p3);
		}

		public static void Rectangle(Color color, double x, double y, double width, double height)
		{
			Draw.CurrentSurface.FillRectangle(color, color, color, color, x, y, width, height);
		}

		public static void Rectangle(Color color, Rectangle rectangle)
		{
			Draw.CurrentSurface.FillRectangle(color, color, color, color, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			Draw.CurrentSurface.FillRectangle(col1, col2, col3, col4, x, y, width, height);
		}

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle)
		{
			Draw.CurrentSurface.FillRectangle(col1, col2, col3, col4, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}


	}
}
