using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff
{
	public static class Fill
	{
		public static void Circle(Color color, Point location, double radius)
		{
			Draw.CurrentSurface.FillCircle(color, color, (PointF)location, radius);
		}

		public static void Circle(Color color, double x, double y, double radius)
		{
			Draw.CurrentSurface.FillCircle(color, color, new PointF((float)x, (float)y), radius);
		}

		public static void Circle(Color col1, Color col2, Point location, double radius)
		{
			Draw.CurrentSurface.FillCircle(col1, col2, (PointF)location, radius);
		}

		public static void Circle(Color col1, Color col2, double x, double y, double radius)
		{
			Draw.CurrentSurface.FillCircle(col1, col2, new PointF((float)x, (float)y), radius);
		}

		public static void Triangle(Color color, double x1, double y1, double x2, double y2, double x3, double y3)
		{
			Draw.CurrentSurface.FillTriangle(color, color, color, new PointF((float)x1, (float)y1), new PointF((float)x2, (float)y2), new PointF((float)x3, (float)y3));
		}

		public static void Triangle(Color color, Point p1, Point p2, Point p3)
		{
			Draw.CurrentSurface.FillTriangle(color, color, color, (PointF)p1, (PointF)p2, (PointF)p3);
		}

		public static void Triangle(Color col1, Color col2, Color col3, double x1, double y1, double x2, double y2, double x3, double y3)
		{
			Draw.CurrentSurface.FillTriangle(col1, col2, col3, new PointF((float)x1, (float)y1), new PointF((float)x2, (float)y2), new PointF((float)x3, (float)y3));
		}

		public static void Triangle(Color col1, Color col2, Color col3, Point p1, Point p2, Point p3)
		{
			Draw.CurrentSurface.FillTriangle(col1, col2, col3, (PointF)p1, (PointF)p2, (PointF)p3);
		}

		public static void Rectangle(Color color, double x, double y, double width, double height)
		{
			Draw.CurrentSurface.FillRectangle(color, color, color, color, (float)x, (float)y, (float)width, (float)height);
		}

		public static void Rectangle(Color color, Rectangle rectangle)
		{
			Draw.CurrentSurface.FillRectangle(color, color, color, color, (float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);
		}

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			Draw.CurrentSurface.FillRectangle(col1, col2, col3, col4, (float)x, (float)y, (float)width, (float)height);
		}

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle)
		{
			Draw.CurrentSurface.FillRectangle(col1, col2, col3, col4, (float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);
		}


	}
}
