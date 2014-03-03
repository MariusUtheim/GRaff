using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class Draw
	{
		internal static Texture LoadTexture(string _path)
		{
			return GraphicsEngine.Current.LoadTexture(_path);
		}

		public static void Clear(Color color)
		{
			GraphicsEngine.Current.Clear(color);
		}

		public static void Rectangle(Color color, double x, double y, double width, double height)
		{
			GraphicsEngine.Current.DrawRectangle(color, x, y, width, height);
		}

		public static void Rectangle(Color color, Rectangle rectangle)
		{
			GraphicsEngine.Current.DrawRectangle(color, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static void Line(Color color, double x1, double y1, double x2, double y2)
		{
			GraphicsEngine.Current.DrawLine(color, x1, y1, x2, y2);
		}

		public static void Line(Color color, Point p1, Point p2)
		{
			GraphicsEngine.Current.DrawLine(color, p1.X, p1.Y, p2.X, p2.Y);
		}

		public static void Line(Color col1, Color col2, double x1, double y1, double x2, double y2)
		{
			GraphicsEngine.Current.DrawLine(col1, col2, x1, y1, x2, y2);
		}

		public static void Line(Color col1, Color col2, Point p1, Point p2)
		{
			GraphicsEngine.Current.DrawLine(col1, col2, p1.X, p1.Y, p2.X, p2.Y);
		}

		public static void Image(double x, double y, Image image)
		{
			GraphicsEngine.Current.DrawImage(x - image.Sprite.Origin.X, y - image.Sprite.Origin.Y, image);
		}
	}
}
