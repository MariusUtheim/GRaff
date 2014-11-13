using GRaff.OpenGL;


namespace GRaff
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

		public static Color GetPixel(double x, double y)
		{
			return CurrentSurface.GetPixel(x, y);
		}
		public static Color GetPixel(Point p)
		{
			return CurrentSurface.GetPixel(p.X, p.Y);
		}
		
		public static void Pixel(Color color, double x, double y)
		{
			CurrentSurface.SetPixel(color, new Point(x, y));
		}
		public static void Pixel(Color color, Point p)
		{
			CurrentSurface.SetPixel(color, p);
		}

		public static void Circle(Color color, double x, double y, double radius)
		{
			CurrentSurface.DrawCircle(color, new Point(x, y), radius);
		}
		public static void Circle(Color color, Point location, double radius)
		{
			CurrentSurface.DrawCircle(color, location, radius);
		}

		public static void Rectangle(Color color, double x, double y, double width, double height)
		{
			CurrentSurface.DrawRectangle(color, color, color, color, x, y, width, height);
		}
		public static void Rectangle(Color color, Point location, Vector size)
		{
			CurrentSurface.DrawRectangle(color, color, color, color, location.X, location.Y, size.X, size.Y);
		}
		public static void Rectangle(Color color, Rectangle rectangle)
		{
			CurrentSurface.DrawRectangle(color, color, color, color, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height)
		{
			CurrentSurface.DrawRectangle(col1, col2, col3, col4, x, y, width, height);
		}
		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Point location, Vector size)
		{
			CurrentSurface.DrawRectangle(col1, col2, col3, col4, location.X, location.Y, size.X, size.Y);
		}
		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle)
		{
			CurrentSurface.DrawRectangle(col1, col2, col3, col4, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static void Line(Color color, double x1, double y1, double x2, double y2)
		{
			CurrentSurface.DrawLine(color, color, new Point(x1, y1), new Point(x2, y2));
		}
		public static void Line(Color color, Point p1, Point p2)
		{
			CurrentSurface.DrawLine(color, color, p1, p2);
		}
		public static void Line(Color color, Line line)
		{
			CurrentSurface.DrawLine(color, color, line.Origin, line.Destination);
		}

		public static void Line(Color col1, Color col2, double x1, double y1, double x2, double y2)
		{
			CurrentSurface.DrawLine(col1, col2, new Point(x1, y1), new Point(x2, y2));
		}
		public static void Line(Color col1, Color col2, Point p1, Point p2)
		{
			CurrentSurface.DrawLine(col1, col2, p1, p2);
		}

		public static void Sprite(Sprite sprite, int imageIndex, double x, double y)
		{
			CurrentSurface.DrawSprite(sprite, imageIndex, x, y);
		}

		public static void Polygon(Color color, Polygon polygon)
		{
			CurrentSurface.DrawPolygon(color, polygon);
		}

		public static void Image(Image image)
		{
			CurrentSurface.DrawImage(image);
		}

		public static void Text(Font font, string text, double x, double y)
		{
			CurrentSurface.DrawText(font, Color.Black, text, x, y);
		}
	}
}
