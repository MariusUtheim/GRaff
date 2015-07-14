using System;
using System.Diagnostics.Contracts;
using GRaff.Graphics;


namespace GRaff
{
	public static partial class Draw
	{
		public static Surface CurrentSurface
		{
			get;
			set;
		}

		public static void Clear(Color color) => CurrentSurface.Clear(color);

		public static Color GetPixel(int x, int y) => CurrentSurface.GetPixel(x, y);
		public static Color GetPixel(IntVector px) => CurrentSurface.GetPixel(px.X, px.Y);
		
		public static void Pixel(Color color, double x, double y) => CurrentSurface.SetPixel(color, new PointF(x, y));
		public static void Pixel(Color color, Point px) => CurrentSurface.SetPixel(color, (PointF)px);

		public static void Circle(Color color, double x, double y, double radius) => CurrentSurface.DrawCircle(color, new PointF(x, y), radius);
		public static void Circle(Color color, Point location, double radius) => CurrentSurface.DrawCircle(color, (PointF)location, radius);
		public static void FillCircle(Color color, Point location, double radius) => Draw.CurrentSurface.FillCircle(color, color, (PointF)location, radius);
		public static void FillCircle(Color color, double x, double y, double radius) => Draw.CurrentSurface.FillCircle(color, color, new PointF(x, y), radius);
		public static void FillCircle(Color col1, Color col2, Point location, double radius) => Draw.CurrentSurface.FillCircle(col1, col2, (PointF)location, radius);
		public static void FillCircle(Color col1, Color col2, double x, double y, double radius) => Draw.CurrentSurface.FillCircle(col1, col2, new PointF(x, y), radius);

		public static void Triangle(Color color, double x1, double y1, double x2, double y2, double x3, double y3) => Draw.CurrentSurface.DrawTriangle(color, color, color, new PointF((float)x1, (float)y1), new PointF((float)x2, (float)y2), new PointF((float)x3, (float)y3));
		public static void Triangle(Color color, Point p1, Point p2, Point p3) => Draw.CurrentSurface.DrawTriangle(color, color, color, (PointF)p1, (PointF)p2, (PointF)p3);
		public static void Triangle(Color color, Triangle triangle) => CurrentSurface.DrawTriangle(color, color, color, (PointF)triangle.V1, (PointF)triangle.V2, (PointF)triangle.V3);
		public static void Triangle(Color col1, Color col2, Color col3, double x1, double y1, double x2, double y2, double x3, double y3) => Draw.CurrentSurface.DrawTriangle(col1, col2, col3, new PointF((float)x1, (float)y1), new PointF((float)x2, (float)y2), new PointF((float)x3, (float)y3));
		public static void Triangle(Color col1, Color col2, Color col3, Point p1, Point p2, Point p3) => Draw.CurrentSurface.DrawTriangle(col1, col2, col3, (PointF)p1, (PointF)p2, (PointF)p3);
		public static void Triangle(Color col1, Color col2, Color col3, Triangle triangle) => CurrentSurface.DrawTriangle(col1, col2, col3, (PointF)triangle.V1, (PointF)triangle.V2, (PointF)triangle.V3);
		public static void FillTriangle(Color color, double x1, double y1, double x2, double y2, double x3, double y3) => Draw.CurrentSurface.FillTriangle(color, color, color, new PointF((float)x1, (float)y1), new PointF((float)x2, (float)y2), new PointF((float)x3, (float)y3));
		public static void FillTriangle(Color color, Point p1, Point p2, Point p3) => Draw.CurrentSurface.FillTriangle(color, color, color, (PointF)p1, (PointF)p2, (PointF)p3);
		public static void FillTriangle(Color color, Triangle triangle) => CurrentSurface.FillTriangle(color, color, color, (PointF)triangle.V1, (PointF)triangle.V2, (PointF)triangle.V3);
		public static void FillTriangle(Color col1, Color col2, Color col3, double x1, double y1, double x2, double y2, double x3, double y3) => Draw.CurrentSurface.FillTriangle(col1, col2, col3, new PointF((float)x1, (float)y1), new PointF((float)x2, (float)y2), new PointF((float)x3, (float)y3));
		public static void FillTriangle(Color col1, Color col2, Color col3, Point p1, Point p2, Point p3) => Draw.CurrentSurface.FillTriangle(col1, col2, col3, (PointF)p1, (PointF)p2, (PointF)p3);
		public static void FillTriangle(Color col1, Color col2, Color col3, Triangle triangle) => CurrentSurface.FillTriangle(col1, col2, col3, (PointF)triangle.V1, (PointF)triangle.V2, (PointF)triangle.V3);

		public static void Rectangle(Color color, double x, double y, double width, double height) => CurrentSurface.DrawRectangle(color, color, color, color, (float)x, (float)y, (float)width, (float)height);
		public static void Rectangle(Color color, Point location, Vector size) => CurrentSurface.DrawRectangle(color, color, color, color, (float)location.X, (float)location.Y, (float)size.X, (float)size.Y);
		public static void Rectangle(Color color, Rectangle rectangle) => CurrentSurface.DrawRectangle(color, color, color, color, (float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height) => CurrentSurface.DrawRectangle(col1, col2, col3, col4, (float)x, (float)y, (float)width, (float)height);
		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Point location, Vector size) => CurrentSurface.DrawRectangle(col1, col2, col3, col4, (float)location.X, (float)location.Y, (float)size.X, (float)size.Y);
		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle) => CurrentSurface.DrawRectangle(col1, col2, col3, col4, (float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);

		public static void FillRectangle(Color color, double x, double y, double width, double height) => CurrentSurface.FillRectangle(color, color, color, color, (float)x, (float)y, (float)width, (float)height);
		public static void FillRectangle(Color color, Point location, Vector size) => CurrentSurface.FillRectangle(color, color, color, color, (float)location.X, (float)location.Y, (float)size.X, (float)size.Y);
		public static void FillRectangle(Color color, Rectangle rectangle) => Draw.CurrentSurface.FillRectangle(color, color, color, color, (float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);

		public static void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height) => Draw.CurrentSurface.FillRectangle(col1, col2, col3, col4, (float)x, (float)y, (float)width, (float)height);
		public static void FillRectangle(Color col1, Color col2, Color col3, Color col4, Point location, Vector size) => CurrentSurface.FillRectangle(col1, col2, col3, col4, (float)location.X, (float)location.Y, (float)size.X, (float)size.Y);
		public static void FillRectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle) => Draw.CurrentSurface.FillRectangle(col1, col2, col3, col4, (float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);

		public static void Line(Color color, double x1, double y1, double x2, double y2) => CurrentSurface.DrawLine(color, color, new PointF((float)x1, (float)y1), new PointF((float)x2, (float)y2));
		public static void Line(Color color, Point p1, Point p2) => CurrentSurface.DrawLine(color, color, (PointF)p1, (PointF)p2);
		public static void Line(Color color, Line line) => CurrentSurface.DrawLine(color, color, (PointF)line.Origin, (PointF)line.Destination);
		public static void Line(Color col1, Color col2, double x1, double y1, double x2, double y2) => CurrentSurface.DrawLine(col1, col2, new PointF((float)x1, (float)y1), new PointF((float)x2, (float)y2));
		public static void Line(Color col1, Color col2, Point p1, Point p2) => CurrentSurface.DrawLine(col1, col2, (PointF)p1, (PointF)p2);
#warning DESIGN: ArgumentNullExceptions?
		public static void Texture(Texture texture, double x, double y)
		{
			Contract.Requires<ArgumentNullException>(texture != null);
			if (!texture.Buffer.IsLoaded)
				throw new InvalidOperationException("Trying to draw a texture that hasn't been loaded");
			if (texture != null)
				CurrentSurface.DrawTexture(texture, new PointF(x, y));
		}


		public static void Sprite(Sprite sprite, int imageIndex, double x, double y)
		{
			if (sprite != null)
				CurrentSurface.DrawSprite(sprite, imageIndex, (float)x, (float)y);
		}

		public static void Sprite(Sprite sprite, int imageIndex, Point p)
		{
			if (sprite != null)
				CurrentSurface.DrawSprite(sprite, imageIndex, (float)p.X, (float)p.Y);
		}

		public static void Sprite(Sprite sprite, int imageIndex, AffineMatrix transform)
		{
			Contract.Requires(transform != null);
			if (sprite != null)
				CurrentSurface.DrawSprite(sprite, imageIndex, Colors.White, transform);
		}

		public static void Sprite(Sprite sprite, int imageIndex, Color blend, AffineMatrix transform)
		{
			Contract.Requires<ArgumentNullException>(transform != null);
			if (sprite != null)
				CurrentSurface.DrawSprite(sprite, imageIndex, blend, transform);
		}

		public static void Sprite(Sprite sprite, int imageIndex, Transform transform)
		{
			Contract.Requires(transform != null);
			if (sprite != null)
				CurrentSurface.DrawSprite(sprite, imageIndex, Colors.White, transform.GetMatrix());
		}

		public static void Sprite(Sprite sprite, int imageIndex, Color blend, Transform transform)
		{
			if (sprite != null)
				CurrentSurface.DrawSprite(sprite, imageIndex, blend, transform.GetMatrix());
		}

		public static void Polygon(Color color, Polygon polygon)
		{
			if (polygon != null)
				CurrentSurface.DrawPolygon(color, polygon);
		}

		public static void FillPolygon(Color color, Polygon polygon)
		{
			if (polygon != null)
				CurrentSurface.FillPolygon(color, polygon);
		}
		
		public static void Image(Image image)
		{
			if (image != null)
				CurrentSurface.DrawImage(image);
		}

		public static void Text(Font font, Color color, string text, double x, double y) => CurrentSurface.DrawText(font, FontAlignment.TopLeft, color, text, new PointF(x, y));

		public static void Text(Font font, Color color, string text, Point location) => CurrentSurface.DrawText(font, FontAlignment.TopLeft, color, text, (PointF)location);

		public static void Text(Font font, Color color, string text, Transform transform) => CurrentSurface.DrawText(font, FontAlignment.TopLeft, color, text, transform);

		public static void Text(Font font, FontAlignment alignment, Color color, string text, double x, double y) => CurrentSurface.DrawText(font, alignment, color, text, new PointF(x, y));

		public static void Text(Font font, FontAlignment alignment, Color color, string text, Point location) => CurrentSurface.DrawText(font, alignment, color, text, (PointF)location);

		public static void Text(Font font, FontAlignment alignment, Color color, string text, Transform transform) => CurrentSurface.DrawText(font, alignment, color, text, transform);
	}
}
