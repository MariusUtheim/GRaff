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

		public static void Circle(Color color, Point location, double radius)
		{
			GraphicsEngine.Current.DrawCircle(color, location, radius);
		}

		public static void Rectangle(Color color, double x, double y, double width, double height)
		{
			GraphicsEngine.Current.DrawRectangle(color, x, y, width, height);
		}

		public static void Rectangle(Color color, Rectangle rectangle)
		{
			GraphicsEngine.Current.DrawRectangle(color, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			GraphicsEngine.Current.DrawRectangle(col1, col2, col3, col4, x, y, width, height);
		}

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle)
		{
			GraphicsEngine.Current.DrawRectangle(col1, col2, col3, col4, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static void Line(Color color, double x1, double y1, double x2, double y2)
		{
			GraphicsEngine.Current.DrawLine(color, x1, y1, x2, y2);
		}

		public static void Line(Color color, Point p1, Point p2)
		{
			GraphicsEngine.Current.DrawLine(color, p1.X, p1.Y, p2.X, p2.Y);
		}

		public static void Line(Color color, Line line)
		{
			GraphicsEngine.Current.DrawLine(color, line.Origin.X, line.Origin.Y, line.Origin.X + line.Direction.X, line.Origin.Y + line.Direction.Y);
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
			GraphicsEngine.Current.DrawImage(x, y, image);
		}

		public static void Sprite(Point location, Sprite sprite, int imageIndex)
		{
			GraphicsEngine.Current.DrawTexture(location.X - sprite.XOrigin, location.Y - sprite.YOrigin, sprite.GetTexture(imageIndex));
		}

		public static Rectangle GetVisibleRegion()
		{
			if (View.IsEnabled)
				return View.ActualRoomView;
			else
				return new Rectangle(0, 0, Room.Width, Room.Height);
		}
	}
}
