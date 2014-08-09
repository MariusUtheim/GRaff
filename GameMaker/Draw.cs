using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class Draw
	{
		public static Surface CurrentSurface
		{
			get;
			set;
		}

		public static void Clear(Color color)
		{
			CurrentSurface.Clear(color);
		}

		public static void Point(Color color, double x, double y)
		{
			CurrentSurface.SetPixel(x, y, color);
		}

		public static void Point(Color color, Point p)
		{
			Draw.Point(color, (int)p.X, (int)p.Y);
		}

		public static void Circle(Color color, Point location, double radius)
		{
			CurrentSurface.DrawCircle(color, location.X, location.Y, radius);
		}

		public static void Circle(Color color, double x, double y, double radius)
		{
			CurrentSurface.DrawCircle(color, x, y, radius);
		}

		public static void Rectangle(Color color, double x, double y, double width, double height)
		{
			CurrentSurface.DrawRectangle(color, x, y, width, height);
		}

		public static void Rectangle(Color color, Rectangle rectangle)
		{
			CurrentSurface.DrawRectangle(color, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			CurrentSurface.DrawRectangle(col1, col2, col3, col4, x, y, width, height);
		}

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle)
		{
			CurrentSurface.DrawRectangle(col1, col2, col3, col4, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static void Line(Color color, double x1, double y1, double x2, double y2)
		{
			CurrentSurface.DrawLine(color, x1, y1, x2, y2);
		}

		public static void Line(Color color, Point p1, Point p2)
		{
			CurrentSurface.DrawLine(color, p1.X, p1.Y, p2.X, p2.Y);
		}

		public static void Line(Color color, Line line)
		{
			CurrentSurface.DrawLine(color, line.Origin.X, line.Origin.Y, line.Origin.X + line.Direction.X, line.Origin.Y + line.Direction.Y);
		}

		public static void Line(Color col1, Color col2, double x1, double y1, double x2, double y2)
		{
			CurrentSurface.DrawLine(col1, col2, x1, y1, x2, y2);
		}

		public static void Line(Color col1, Color col2, Point p1, Point p2)
		{
			CurrentSurface.DrawLine(col1, col2, p1.X, p1.Y, p2.X, p2.Y);
		}

		public static void Image(double x, double y, Transform transform, Image image)
		{
			CurrentSurface.DrawImage(x, y, transform, image);
		}

		public static void Sprite(Point location, Sprite sprite, int imageIndex)
		{
			CurrentSurface.DrawTexture(location.X - sprite.XOrigin, location.Y - sprite.YOrigin, sprite.GetTexture(imageIndex));
		}

		public static void Sprite(double x, double y, Sprite sprite, int imageIndex)
		{
			CurrentSurface.DrawTexture(x - sprite.XOrigin, y - sprite.YOrigin, sprite.GetTexture(imageIndex));
		}
	}
}
