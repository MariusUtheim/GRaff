using System;
using System.Diagnostics.Contracts;
using GRaff.Graphics;
using GRaff.Graphics.Text;

namespace GRaff
{
	public static partial class Draw
	{
		private static Surface _currentSurface;
		public static Surface CurrentSurface
		{
			get
			{
				Contract.Ensures(Contract.Result<Surface>() != null);
				return _currentSurface;
			}

			set
			{
				Contract.Requires<ArgumentNullException>(value != null);
				_currentSurface = value;
			}
		}


		public static void Clear(Color color) => CurrentSurface.Clear(color);

		public static Color GetPixel(int x, int y) => CurrentSurface.GetPixel(x, y);
		public static Color GetPixel(IntVector px) => CurrentSurface.GetPixel(px.X, px.Y);
		
		public static void Pixel(Color color, double x, double y) => CurrentSurface.SetPixel(color, new GraphicsPoint(x, y));
		public static void Pixel(Color color, Point px) => CurrentSurface.SetPixel(color, (GraphicsPoint)px);

		public static void Circle(Color color, double x, double y, double radius) => CurrentSurface.DrawCircle(color, new GraphicsPoint(x, y), radius);
		public static void Circle(Color color, Point location, double radius) => CurrentSurface.DrawCircle(color, (GraphicsPoint)location, radius);
		public static void FillCircle(Color color, Point location, double radius) => Draw.CurrentSurface.FillCircle(color, color, (GraphicsPoint)location, radius);
		public static void FillCircle(Color color, double x, double y, double radius) => Draw.CurrentSurface.FillCircle(color, color, new GraphicsPoint(x, y), radius);
		public static void FillCircle(Color col1, Color col2, Point location, double radius) => Draw.CurrentSurface.FillCircle(col1, col2, (GraphicsPoint)location, radius);
		public static void FillCircle(Color col1, Color col2, double x, double y, double radius) => Draw.CurrentSurface.FillCircle(col1, col2, new GraphicsPoint(x, y), radius);

		public static void Ellipse(Color color, double x, double y, double width, double height) => CurrentSurface.DrawEllipse(color, new GraphicsPoint(x, y), width, height);
		public static void Ellipse(Color color, Point location, Vector size) => CurrentSurface.DrawEllipse(color, (GraphicsPoint)location, size.X, size.Y);
		public static void Ellipse(Color color, Rectangle rectangle) => CurrentSurface.DrawEllipse(color, (GraphicsPoint)rectangle.TopLeft, rectangle.Width, rectangle.Height);
		public static void FillEllipse(Color color, double x, double y, double width, double height) => CurrentSurface.FillEllipse(color, color, new GraphicsPoint(x, y), width, height);
		public static void FillEllipse(Color color, Point location, Vector size) => CurrentSurface.FillEllipse(color, color, (GraphicsPoint)location, size.X, size.Y);
		public static void FillEllipse(Color color, Rectangle rectangle) => CurrentSurface.FillEllipse(color, color, (GraphicsPoint)rectangle.TopLeft, rectangle.Width, rectangle.Height);
		public static void FillEllipse(Color innerColor, Color outerColor, double x, double y, double width, double height) => CurrentSurface.FillEllipse(innerColor, outerColor, new GraphicsPoint(x, y), width, height);
		public static void FillEllipse(Color innerColor, Color outerColor, Point location, Vector size) => CurrentSurface.FillEllipse(innerColor, outerColor, (GraphicsPoint)location, size.X, size.Y);
		public static void FillEllipse(Color innerColor, Color outerColor, Rectangle rectangle) => CurrentSurface.FillEllipse(innerColor, outerColor, (GraphicsPoint)rectangle.TopLeft, rectangle.Width, rectangle.Height);


		public static void Triangle(Color color, double x1, double y1, double x2, double y2, double x3, double y3) => Draw.CurrentSurface.DrawTriangle(color, color, color, new GraphicsPoint(x1, y1), new GraphicsPoint(x2, y2), new GraphicsPoint(x3, y3));
		public static void Triangle(Color color, Point p1, Point p2, Point p3) => Draw.CurrentSurface.DrawTriangle(color, color, color, (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3);
		public static void Triangle(Color color, Triangle triangle) => CurrentSurface.DrawTriangle(color, color, color, (GraphicsPoint)triangle.V1, (GraphicsPoint)triangle.V2, (GraphicsPoint)triangle.V3);
		public static void Triangle(Color col1, Color col2, Color col3, double x1, double y1, double x2, double y2, double x3, double y3) => Draw.CurrentSurface.DrawTriangle(col1, col2, col3, new GraphicsPoint(x1, y1), new GraphicsPoint(x2, y2), new GraphicsPoint(x3, y3));
		public static void Triangle(Color col1, Color col2, Color col3, Point p1, Point p2, Point p3) => Draw.CurrentSurface.DrawTriangle(col1, col2, col3, (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3);
		public static void Triangle(Color col1, Color col2, Color col3, Triangle triangle) => CurrentSurface.DrawTriangle(col1, col2, col3, (GraphicsPoint)triangle.V1, (GraphicsPoint)triangle.V2, (GraphicsPoint)triangle.V3);
		public static void FillTriangle(Color color, double x1, double y1, double x2, double y2, double x3, double y3) => Draw.CurrentSurface.FillTriangle(color, color, color, new GraphicsPoint(x1, y1), new GraphicsPoint(x2, y2), new GraphicsPoint(x3, y3));
		public static void FillTriangle(Color color, Point p1, Point p2, Point p3) => Draw.CurrentSurface.FillTriangle(color, color, color, (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3);
		public static void FillTriangle(Color color, Triangle triangle) => CurrentSurface.FillTriangle(color, color, color, (GraphicsPoint)triangle.V1, (GraphicsPoint)triangle.V2, (GraphicsPoint)triangle.V3);
		public static void FillTriangle(Color col1, Color col2, Color col3, double x1, double y1, double x2, double y2, double x3, double y3) => Draw.CurrentSurface.FillTriangle(col1, col2, col3, new GraphicsPoint(x1, y1), new GraphicsPoint(x2, y2), new GraphicsPoint(x3, y3));
		public static void FillTriangle(Color col1, Color col2, Color col3, Point p1, Point p2, Point p3) => Draw.CurrentSurface.FillTriangle(col1, col2, col3, (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3);
		public static void FillTriangle(Color col1, Color col2, Color col3, Triangle triangle) => CurrentSurface.FillTriangle(col1, col2, col3, (GraphicsPoint)triangle.V1, (GraphicsPoint)triangle.V2, (GraphicsPoint)triangle.V3);
		
		public static void Rectangle(Color color, double x, double y, double width, double height) => CurrentSurface.DrawRectangle(color, color, color, color, x, y, width, height);
		public static void Rectangle(Color color, Point location, Vector size) => CurrentSurface.DrawRectangle(color, color, color, color, location.X, location.Y, size.X, size.Y);
		public static void Rectangle(Color color, Rectangle rectangle) => CurrentSurface.DrawRectangle(color, color, color, color, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);

		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height) => CurrentSurface.DrawRectangle(col1, col2, col3, col4, x, y, width, height);
		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Point location, Vector size) => CurrentSurface.DrawRectangle(col1, col2, col3, col4, location.X, location.Y, size.X, size.Y);
		public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle) => CurrentSurface.DrawRectangle(col1, col2, col3, col4, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);

		public static void FillRectangle(Color color, double x, double y, double width, double height) => CurrentSurface.FillRectangle(color, color, color, color, x, y, width, height);
		public static void FillRectangle(Color color, Point location, Vector size) => CurrentSurface.FillRectangle(color, color, color, color, location.X, location.Y, size.X, size.Y);
		public static void FillRectangle(Color color, Rectangle rectangle) => Draw.CurrentSurface.FillRectangle(color, color, color, color, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);

		public static void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double width, double height) => Draw.CurrentSurface.FillRectangle(col1, col2, col3, col4, x, y, width, height);
		public static void FillRectangle(Color col1, Color col2, Color col3, Color col4, Point location, Vector size) => CurrentSurface.FillRectangle(col1, col2, col3, col4, location.X, location.Y, size.X, size.Y);
		public static void FillRectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle) => Draw.CurrentSurface.FillRectangle(col1, col2, col3, col4, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);

		public static void Line(Color color, double x1, double y1, double x2, double y2) => CurrentSurface.DrawLine(color, color, new GraphicsPoint(x1, y1), new GraphicsPoint(x2, y2));
		public static void Line(Color color, Point p1, Point p2) => CurrentSurface.DrawLine(color, color, (GraphicsPoint)p1, (GraphicsPoint)p2);
		public static void Line(Color color, Line line) => CurrentSurface.DrawLine(color, color, (GraphicsPoint)line.Origin, (GraphicsPoint)line.Destination);
		public static void Line(Color col1, Color col2, double x1, double y1, double x2, double y2) => CurrentSurface.DrawLine(col1, col2, new GraphicsPoint(x1, y1), new GraphicsPoint(x2, y2));
		public static void Line(Color col1, Color col2, Point p1, Point p2) => CurrentSurface.DrawLine(col1, col2, (GraphicsPoint)p1, (GraphicsPoint)p2);

		public static void Texture(Texture texture, double x, double y)
		{
			if (texture != null)
				CurrentSurface.DrawTexture(texture, new GraphicsPoint(x, y));
		}


		public static void Sprite(Sprite sprite, int imageIndex, double x, double y)
		{
			if (sprite != null)
				CurrentSurface.DrawSprite(sprite, imageIndex, x, y);
		}

		public static void Sprite(Sprite sprite, int imageIndex, Point p)
		{
			if (sprite != null)
				CurrentSurface.DrawSprite(sprite, imageIndex, p.X, p.Y);
		}

		public static void Sprite(Sprite sprite, int imageIndex, AffineMatrix transform)
		{
			Contract.Requires<ArgumentNullException>(transform != null);
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
			Contract.Requires<ArgumentNullException>(transform != null);
			if (sprite != null)
				CurrentSurface.DrawSprite(sprite, imageIndex, Colors.White, transform.GetMatrix());
		}

		public static void Sprite(Sprite sprite, int imageIndex, Color blend, Transform transform)
		{
			Contract.Requires<ArgumentNullException>(transform != null);
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

		public static void Text(string text, TextRenderer renderer, Color color, double x, double y)
		{
			Contract.Requires<ArgumentNullException>(renderer != null);
			CurrentSurface.DrawText(renderer, color, text, AffineMatrix.Translation(x, y));
		}

		public static void Text(string text, TextRenderer renderer, Color color, Point location)
		{
			Contract.Requires<ArgumentNullException>(renderer != null);
			CurrentSurface.DrawText(renderer, color, text, AffineMatrix.Translation(location.X, location.Y));
		}

		public static void Text(string text, TextRenderer renderer, Color color, Transform transform)
		{
			Contract.Requires<ArgumentNullException>(renderer != null && transform != null);
			CurrentSurface.DrawText(renderer, color, text, transform.GetMatrix());
		}

		public static void Text(string text, TextRenderer renderer, Color color, AffineMatrix transform)
		{
			Contract.Requires<ArgumentNullException>(renderer != null && transform != null);
			CurrentSurface.DrawText(renderer, color, text, transform);
		}

		public static void Text(string text, Font font, Color color, double x, double y) => Text(text, new TextRenderer(font), color, AffineMatrix.Translation(x, y));
		public static void Text(string text, Font font, Color color, Point location) => Text(text, new TextRenderer(font), color, AffineMatrix.Translation(location.X, location.Y));

		public static void Text(string text, Font font, FontAlignment alignment, Color color, double x, double y) => Text(text, new TextRenderer(font, alignment), color, AffineMatrix.Translation(x, y));

		public static void Text(string text, Font font, FontAlignment alignment, Color color, Point location) => Text(text, new TextRenderer(font, alignment), color, AffineMatrix.Translation(location.X, location.Y));

		//public static void Text(string text, Font font, Color color, double x, double y) => CurrentSurface.DrawText(font, )

		/*
		public static void Text(string text, Font font, Color color, Transform transform) => CurrentSurface.DrawText(font, FontAlignment.TopLeft, color, text, transform);
		
		public static void Text(string text, Font font, FontAlignment alignment, Color color, double x, double y) => CurrentSurface.DrawText(font, alignment, color, text, new PointF(x, y));

		public static void Text(string text, Font font, FontAlignment alignment, Color color, Point location) => CurrentSurface.DrawText(font, alignment, color, text, (PointF)location);

		public static void Text(string text, Font font, FontAlignment alignment, Color color, Transform transform) => CurrentSurface.DrawText(font, alignment, color, text, transform);
		*/
	}
}
