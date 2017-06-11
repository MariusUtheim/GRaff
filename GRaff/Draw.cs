using System;
using System.Diagnostics.Contracts;
using GRaff.Graphics;
using GRaff.Graphics.Text;
using System.Linq;

namespace GRaff
{
	public static partial class Draw
	{

        public static IRenderDevice Device { get; set; }

		public static void Clear(Color color) => Device.Clear(color);

		
		public static void Point(Color color, Point px) => Device.Draw(new[] { (GraphicsPoint)px }, color, PrimitiveType.Points);


        public static void Line(Color color, Point p1, Point p2) => Device.Draw(new[] { (GraphicsPoint)p1, (GraphicsPoint)p2 }, color, PrimitiveType.Lines);
        public static void Line(Color color, Line line) => Line(color, line.Origin, line.Destination);

        public static void Line(Color col1, Color col2, Point p1, Point p2) => Device.Draw(new[] { (GraphicsPoint)p1, (GraphicsPoint)p2 }, new[] { col1, col2 }, PrimitiveType.Lines);
        public static void Line(Color col1, Color col2, Line line) => Line(col1, col2, line.Origin, line.Destination);


		public static void Triangle(Color color, Point p1, Point p2, Point p3) => Device.Draw(new[] { (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3 }, color, PrimitiveType.LineLoop);
        public static void Triangle(Color color, Triangle triangle) => Triangle(color, triangle.V1, triangle.V2, triangle.V3);

        public static void Triangle(Color col1, Color col2, Color col3, Point p1, Point p2, Point p3) => Device.Draw(new[] { (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3 }, new[] { col1, col2, col3 }, PrimitiveType.LineLoop);
        public static void Triangle(Color col1, Color col2, Color col3, Triangle triangle) => Triangle(col1, col2, col3, triangle.V1, triangle.V2, triangle.V3);

        public static void FillTriangle(Color color, Point p1, Point p2, Point p3) => Device.Draw(new[] { (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3 }, color, PrimitiveType.Triangles);
        public static void FillTriangle(Color color, Triangle triangle) => FillTriangle(color, triangle.V1, triangle.V2, triangle.V3);

        public static void FillTriangle(Color col1, Color col2, Color col3, Point p1, Point p2, Point p3) => Device.Draw(new[] { (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3 }, new[] { col1, col2, col3 }, PrimitiveType.Triangles);
        public static void FillTriangle(Color col1, Color col2, Color col3, Triangle triangle) => FillTriangle(col1, col2, col3, triangle.V1, triangle.V2, triangle.V3);


		public static void Rectangle(Color color, Point location, Vector size)
        {
            (var x, var y) = location;
            (var w, var h) = size;
            Device.Draw(new[] { new GraphicsPoint(x, y), new GraphicsPoint(x + w, y), new GraphicsPoint(x + w, y + h), new GraphicsPoint(x, y + h), new GraphicsPoint(x, y + h) }, color, PrimitiveType.LineLoop);
        }
        public static void Rectangle(Color color, Rectangle rectangle) => Rectangle(color, rectangle.TopLeft, rectangle.Size);

        public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Point location, Vector size)
        {
            (var x, var y) = location;
            (var w, var h) = size;
            Device.Draw(new[] { new GraphicsPoint(x, y), new GraphicsPoint(x + w, y), new GraphicsPoint(x + w, y + h), new GraphicsPoint(x, y + h) }, 
                        new[] { col1, col2, col3, col4 }, PrimitiveType.LineLoop);
        }
        public static void Rectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle) => Rectangle(col1, col2, col3, col4, rectangle.TopLeft, rectangle.Size);
        
        public static void FillRectangle(Color color, Point location, Vector size)
        {
            (var x, var y) = location;
            (var w, var h) = size;
            Device.Draw(new[] { new GraphicsPoint(x, y), new GraphicsPoint(x + w, y), new GraphicsPoint(x + w, y + h), new GraphicsPoint(x, y + h), new GraphicsPoint(x, y + h) }, color, PrimitiveType.Quads);
        }
        public static void FillRectangle(Color color, Rectangle rectangle) => FillRectangle(color, rectangle.TopLeft, rectangle.Size);

        public static void FillRectangle(Color col1, Color col2, Color col3, Color col4, Point location, Vector size)
        {
            var (x, y) = location;
            var (w, h) = size;
            Device.Draw(new[] { new GraphicsPoint(x, y), new GraphicsPoint(x + w, y), new GraphicsPoint(x + w, y + h), new GraphicsPoint(x, y + h) },
                        new[] { col1, col2, col3, col4 }, PrimitiveType.Quads);
        }
        public static void FillRectangle(Color col1, Color col2, Color col3, Color col4, Rectangle rectangle) => FillRectangle(col1, col2, col3, col4, rectangle.TopLeft, rectangle.Size);

        
        public static void Circle(Color color, Point location, double radius) => Device.Draw(GRaff.Polygon.Circle(location, radius).Outline(), color, PrimitiveType.LineLoop);

        public static void FillCircle(Color color, Point location, double radius) => Device.Draw(GRaff.Polygon.Circle(location, radius).Tesselate(), color, PrimitiveType.TriangleStrip);

        public static void FillCircle(Color innerColor, Color outerColor, Point location, double radius) => _drawFan((GraphicsPoint)location, GRaff.Polygon.Circle(location, radius).Outline(), innerColor, outerColor);


        public static void Ellipse(Color color, Point location, Vector radii) 
            => Device.Draw(GRaff.Polygon.Ellipse(location, radii.X, radii.Y).Outline(), color, PrimitiveType.LineLoop);

        public static void Ellipse(Color color, Rectangle rectangle) => Device.Draw(GRaff.Polygon.Ellipse(rectangle).Outline(), color, PrimitiveType.LineLoop);

        public static void FillEllipse(Color color, Point location, Vector radii) => Device.Draw(GRaff.Polygon.Ellipse(location, radii.X, radii.Y).Tesselate(), color, PrimitiveType.TriangleStrip);

        public static void FillEllipse(Color color, Rectangle rectangle) => Device.Draw(GRaff.Polygon.Ellipse(rectangle).Tesselate(), color, PrimitiveType.TriangleStrip);

        public static void FillEllipse(Color innerColor, Color outerColor, Point location, Vector radii) => _drawFan((GraphicsPoint)location, GRaff.Polygon.Ellipse(location, radii.X, radii.Y).Outline(), innerColor, outerColor);

        public static void FillEllipse(Color innerColor, Color outerColor, Rectangle rectangle) => _drawFan((GraphicsPoint)rectangle.Center, GRaff.Polygon.Ellipse(rectangle).Outline(), innerColor, outerColor);

        public static void Texture(Texture texture, Color blend, Point location)
        {
            if (texture != null)
                Device.DrawTexture(texture, 0, 0, blend, Matrix.Translation(location));
        }
        public static void Texture(Texture texture, Point location) => Texture(texture, Colors.White, location);

        public static void Sprite(Sprite sprite, double imageIndex, Color blend, Matrix transform)
        {
            Contract.Requires<ArgumentNullException>(transform != null);
            if (sprite != null)
                Device.DrawTexture(sprite.SubImage(imageIndex), sprite.XOrigin, sprite.YOrigin, blend, transform);
        }
        public static void Sprite(Sprite sprite, double imageIndex, Matrix transform) => Sprite(sprite, imageIndex, Colors.White, transform);
        public static void Sprite(Sprite sprite, double imageIndex, Point location) => Sprite(sprite, imageIndex, Colors.White, Matrix.Translation(location));
        public static void Sprite(Sprite sprite, int imageIndex, Transform transform) => Sprite(sprite, imageIndex, Colors.White, transform.GetMatrix());
        public static void Sprite(Sprite sprite, int imageIndex, Color blend, Transform transform) => Sprite(sprite, imageIndex, blend, transform.GetMatrix());
        public static void Image(Image image)
        {
            if (image != null)
                Sprite(image.Sprite, image.Index, image.Blend, image.Transform.GetMatrix());
        }

        public static void Polygon(Color color, Polygon polygon)
		{
			if (polygon != null)
				Device.Draw(polygon.Vertices.Select(v => (GraphicsPoint)v).ToArray(), color, PrimitiveType.LineStrip);
		}

		public static void FillPolygon(Color color, Polygon polygon)
		{
			if (polygon != null)
                Device.Draw(polygon.Vertices.Select(v => (GraphicsPoint)v).ToArray(), color, PrimitiveType.TriangleFan);
        }


        public static void Text(string text, TextRenderer renderer, Color color, Matrix transform)
        {
            Contract.Requires<ArgumentNullException>(renderer != null && transform != null);
            Device.DrawText(renderer, color, text, transform);
        }
        public static void Text(string text, TextRenderer renderer, Color color, Point location) => Text(text, renderer, color, Matrix.Translation(location));
        public static void Text(string text, TextRenderer renderer, Color color, Transform transform)
		{
			Contract.Requires<ArgumentNullException>(renderer != null && transform != null);
            Text(text, renderer, color, transform.GetMatrix());
		}
        public static void Text(string text, Font font, Color color, Point location) => Text(text, new TextRenderer(font), color, Matrix.Translation(location));
		public static void Text(string text, Font font, FontAlignment alignment, Color color, double x, double y) => Text(text, new TextRenderer(font, alignment), color, Matrix.Translation(x, y));
		public static void Text(string text, Font font, FontAlignment alignment, Color color, Point location) => Text(text, new TextRenderer(font, alignment), color, Matrix.Translation(location.X, location.Y));

        //public static void Text(string text, Font font, Color color, double x, double y) => CurrentSurface.DrawText(font, )

        /*
		public static void Text(string text, Font font, Color color, Transform transform) => CurrentSurface.DrawText(font, FontAlignment.TopLeft, color, text, transform);
		
		public static void Text(string text, Font font, FontAlignment alignment, Color color, double x, double y) => CurrentSurface.DrawText(font, alignment, color, text, new PointF(x, y));

		public static void Text(string text, Font font, FontAlignment alignment, Color color, Point location) => CurrentSurface.DrawText(font, alignment, color, text, (PointF)location);

		public static void Text(string text, Font font, FontAlignment alignment, Color color, Transform transform) => CurrentSurface.DrawText(font, alignment, color, text, transform);
		*/



        private static void _drawFan(GraphicsPoint origin, GraphicsPoint[] boundary, Color innerColor, Color outerColor)
        {
            var vertices = new GraphicsPoint[boundary.Length + 2];
            vertices[0] = origin;
            Array.Copy(boundary, 0, vertices, 1, boundary.Length);
            vertices[vertices.Length - 1] = boundary[0];
            
            var colors = new Color[vertices.Length];
            colors[0] = innerColor;
            for (int j = 1; j < colors.Length; j++)
                colors[j] = outerColor;

            Device.Draw(vertices, colors, PrimitiveType.TriangleFan);
        }

    }
}
