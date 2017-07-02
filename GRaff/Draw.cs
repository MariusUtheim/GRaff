using System;
using System.Diagnostics.Contracts;
using GRaff.Graphics;
using GRaff.Graphics.Text;
using System.Linq;
using System.Collections.Generic;

namespace GRaff
{
	public static partial class Draw
	{

        public static IRenderDevice Device { get; set; }

		public static void Clear(Color color) => Device.Clear(color);

		
		public static void Point(Point p, Color color) => Device.Draw(PrimitiveType.Points, new[] { (GraphicsPoint)p }, color);


        public static void Line(Point p1, Point p2, Color color) => Device.Draw(PrimitiveType.Lines, new[] { (GraphicsPoint)p1, (GraphicsPoint)p2 }, color);
        public static void Line(Line line, Color color) => Line(line.Origin, line.Destination, color);

        public static void Line(Point p1, Point p2, Color col1, Color col2) => Device.Draw(PrimitiveType.Lines, new[] { (GraphicsPoint)p1, (GraphicsPoint)p2 }, new[] { col1, col2 });
        public static void Line(Line line, Color col1, Color col2) => Line(line.Origin, line.Destination, col1, col2);


		public static void Triangle(Point p1, Point p2, Point p3, Color color) => Device.Draw(PrimitiveType.LineLoop, new[] { (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3 }, color);
        public static void Triangle(Triangle triangle, Color color) => Triangle(triangle.V1, triangle.V2, triangle.V3, color);

        public static void Triangle(Point p1, Point p2, Point p3, Color col1, Color col2, Color col3) => Device.Draw(PrimitiveType.LineLoop, new[] { (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3 }, new[] { col1, col2, col3 });
        public static void Triangle(Triangle triangle, Color col1, Color col2, Color col3) => Triangle(triangle.V1, triangle.V2, triangle.V3, col1, col2, col3);

        public static void FillTriangle(Point p1, Point p2, Point p3, Color color) => Device.Draw(PrimitiveType.Triangles, new[] { (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3 }, color);
        public static void FillTriangle(Triangle triangle, Color color) => FillTriangle(triangle.V1, triangle.V2, triangle.V3, color);

        public static void FillTriangle(Point p1, Point p2, Point p3, Color col1, Color col2, Color col3) => Device.Draw(PrimitiveType.Triangles, new[] { (GraphicsPoint)p1, (GraphicsPoint)p2, (GraphicsPoint)p3 }, new[] { col1, col2, col3 });
        public static void FillTriangle(Triangle triangle, Color col1, Color col2, Color col3) => FillTriangle(triangle.V1, triangle.V2, triangle.V3, col1, col2, col3);


		public static void Rectangle(Point location, Vector size, Color color)
        {
            (var x, var y) = location;
            (var w, var h) = size;
            Device.Draw(PrimitiveType.LineLoop, new[] { new GraphicsPoint(x, y), new GraphicsPoint(x + w, y), new GraphicsPoint(x + w, y + h), new GraphicsPoint(x, y + h), new GraphicsPoint(x, y + h) }, color);
        }
        public static void Rectangle(Rectangle rectangle, Color color) => Rectangle(rectangle.TopLeft, rectangle.Size, color);

        public static void Rectangle(Point location, Vector size, Color col1, Color col2, Color col3, Color col4)
        {
            (var x, var y) = location;
            (var w, var h) = size;
            Device.Draw(PrimitiveType.LineLoop, new[] { new GraphicsPoint(x, y), new GraphicsPoint(x + w, y), new GraphicsPoint(x + w, y + h), new GraphicsPoint(x, y + h) }, 
                        new[] { col1, col2, col3, col4 });
        }
        public static void Rectangle(Rectangle rectangle, Color col1, Color col2, Color col3, Color col4) => Rectangle(rectangle.TopLeft, rectangle.Size, col1, col2, col3, col4);
        
        public static void FillRectangle(Point location, Vector size, Color color)
        {
            (var x, var y) = location;
            (var w, var h) = size;
            Device.Draw(PrimitiveType.TriangleStrip, new[] { new GraphicsPoint(x, y), new GraphicsPoint(x + w, y), new GraphicsPoint(x, y + h), new GraphicsPoint(x + w, y + h) }, color);
        }
        public static void FillRectangle(Rectangle rectangle, Color color) => FillRectangle(rectangle.TopLeft, rectangle.Size, color);

        public static void FillRectangle(Point location, Vector size, Color col1, Color col2, Color col3, Color col4)
        {
            var (x, y) = location;
            var (w, h) = size;
            Device.Draw(PrimitiveType.TriangleStrip, new[] { new GraphicsPoint(x, y), new GraphicsPoint(x + w, y), new GraphicsPoint(x, y + h), new GraphicsPoint(x + w, y + h) },
                        new[] { col1, col2, col3, col4 });
        }
        public static void FillRectangle(Rectangle rectangle, Color col1, Color col2, Color col3, Color col4) => FillRectangle(rectangle.TopLeft, rectangle.Size, col1, col2, col3, col4);

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

			Device.Draw(PrimitiveType.TriangleFan, vertices, colors);
		}
        
        public static void Circle(Point location, double radius, Color color) => Device.Draw(PrimitiveType.LineLoop, GRaff.Polygon.Circle(location, radius).Outline(), color);

        public static void FillCircle(Point location, double radius, Color color) => Device.Draw(PrimitiveType.TriangleStrip, GRaff.Polygon.Circle(location, radius).Tesselate(), color);

        public static void FillCircle(Point location, double radius, Color innerColor, Color outerColor) => _drawFan((GraphicsPoint)location, GRaff.Polygon.Circle(location, radius).Outline(), innerColor, outerColor);


        public static void Ellipse(Point location, Vector radii, Color color) 
            => Device.Draw(PrimitiveType.LineLoop, GRaff.Polygon.Ellipse(location, radii.X, radii.Y).Outline(), color);

        public static void Ellipse(Rectangle rectangle, Color color) => Device.Draw(PrimitiveType.LineLoop, GRaff.Polygon.Ellipse(rectangle).Outline(), color);

        public static void FillEllipse(Point location, Vector radii, Color color) => Device.Draw(PrimitiveType.TriangleStrip, GRaff.Polygon.Ellipse(location, radii.X, radii.Y).Tesselate(), color);

        public static void FillEllipse(Rectangle rectangle, Color color) => Device.Draw(PrimitiveType.TriangleStrip, GRaff.Polygon.Ellipse(rectangle).Tesselate(), color);

        public static void FillEllipse(Point location, Vector radii, Color innerColor, Color outerColor) => _drawFan((GraphicsPoint)location, GRaff.Polygon.Ellipse(location, radii.X, radii.Y).Outline(), innerColor, outerColor);

        public static void FillEllipse(Rectangle rectangle, Color innerColor, Color outerColor) => _drawFan((GraphicsPoint)rectangle.Center, GRaff.Polygon.Ellipse(rectangle).Outline(), innerColor, outerColor);

        public static void Texture(SubTexture texture, Point location, Color blend)
        {
            if (texture != null)
                Device.DrawTexture(texture, 0, 0, Matrix.Translation(location), blend);
        }
        public static void Texture(SubTexture texture, Point location) => Texture(texture, location, Colors.White);
        public static void Texture(SubTexture texture, Point location, Color c1, Color c2, Color c3, Color c4)
            => Texture(texture, (location, texture.Size), c1, c2, c3, c4);

        public static void Texture(SubTexture texture, Rectangle rect, Color blend)
        {
            if (texture != null)
                Device.DrawTexture(texture, 0, 0, new Matrix(rect.Width / texture.Width, 0, rect.Left, 0, rect.Height / texture.Height, rect.Top), blend);
        }
        public static void Texture(SubTexture texture, Rectangle rect) => Texture(texture, rect, Colors.White);

        public static void Texture(SubTexture texture, Rectangle rect, Color c1, Color c2, Color c3, Color c4)
        {
            if (texture != null)
            {
                var pts = new[] { (GraphicsPoint)rect.TopLeft, (GraphicsPoint)rect.TopRight, (GraphicsPoint)rect.BottomLeft, (GraphicsPoint)rect.BottomRight };
                var cols = new[] { c1, c2, c4, c3 };
                Device.DrawTexture(texture.Texture, PrimitiveType.TriangleStrip, pts, cols, texture.StripCoords);
            }
                                   
        }

        public static void Sprite(Sprite sprite, double imageIndex, Matrix transform, Color blend)
        {
            Contract.Requires<ArgumentNullException>(transform != null);
            if (sprite != null)
                Device.DrawTexture(sprite.SubImage(imageIndex), sprite.XOrigin, sprite.YOrigin, transform, blend);
        }
        public static void Sprite(Sprite sprite, double imageIndex, Matrix transform) => Sprite(sprite, imageIndex, transform, Colors.White);
        public static void Sprite(Sprite sprite, double imageIndex, Point location) => Sprite(sprite, imageIndex, Matrix.Translation(location), Colors.White);
        public static void Sprite(Sprite sprite, int imageIndex, Transform transform) => Sprite(sprite, imageIndex, transform.GetMatrix(), Colors.White);
        public static void Sprite(Sprite sprite, int imageIndex, Transform transform, Color blend) => Sprite(sprite, imageIndex, transform.GetMatrix(), blend);
        public static void Model(Model model)
        {
            if (model != null)
                Sprite(model.Sprite, model.Index, model.Transform.GetMatrix(), model.Blend);
        }

        public static void Polygon(Polygon polygon, Color color)
		{
			if (polygon != null)
				Device.Draw(PrimitiveType.LineStrip, polygon.Vertices.Select(v => (GraphicsPoint)v).ToArray(), color);
		}

		public static void FillPolygon(Polygon polygon, Color color)
		{
			if (polygon != null)
                Device.Draw(PrimitiveType.TriangleFan, polygon.Vertices.Select(v => (GraphicsPoint)v).ToArray(), color);
        }




		public static void Primitive(PrimitiveType primitiveType, GraphicsPoint[] vertices, Color color)
		{
			Contract.Requires<ArgumentNullException>(vertices != null);
			Device.Draw(primitiveType, vertices.Select(p => (GraphicsPoint)p).ToArray(), color);
		}

		public static void Primitive(PrimitiveType primitiveType, (GraphicsPoint vertex, Color color)[] primitive)
		{
			Contract.Requires<ArgumentNullException>(primitive != null);
            Device.Draw(primitiveType, primitive);
		}

		public static void Primitive(PrimitiveType primitiveType, Texture buffer, params (Point vertex, Point texCoord)[] vertices)
		{
            Device.DrawTexture(buffer, primitiveType, vertices.Select(v => (GraphicsPoint)v.vertex).ToArray(), Colors.White, vertices.Select(v => (GraphicsPoint)v.texCoord).ToArray());
		}

//TODO//		public static void Primitive(PrimitiveType primitiveType, TextureBuffer buffer, (GraphicsPoint vertex, Color color, GraphicsPoint texCoord)[] primitive)
//		{
//            Device.DrawTexture(buffer, primitiveType, primitive);
//		}

        public static void Lines(IEnumerable<Line> lines, Color color)
            => Primitive(PrimitiveType.Lines, lines.SelectMany(l => ((GraphicsPoint)l.Origin, (GraphicsPoint)l.Destination)).ToArray(), color);

        public static void Lines(IEnumerable<(Line line, Color color)> primitives)
            => Primitive(PrimitiveType.Lines, primitives.SelectMany(p => (((GraphicsPoint)p.line.Origin, p.color), ((GraphicsPoint)p.line.Destination, p.color))).ToArray());

        public static void Triangles(IEnumerable<Triangle> triangles, Color color)
            => Primitive(PrimitiveType.Triangles, triangles.SelectMany(t => ((GraphicsPoint)t.V1, (GraphicsPoint)t.V2, (GraphicsPoint)t.V3)).ToArray(), color);

        public static void Triangles(IEnumerable<(Triangle triangle, Color color)> primitives)
            => Primitive(PrimitiveType.Triangles, primitives.SelectMany(p => (((GraphicsPoint)p.triangle.V1, p.color), ((GraphicsPoint)p.triangle.V2, p.color), ((GraphicsPoint)p.triangle.V3, p.color))).ToArray());

#warning Clean up these overloads
        public static void Text(string text, TextRenderer renderer, Matrix transform, Color color)
		{
			Contract.Requires<ArgumentNullException>(renderer != null && transform != null);
			Device.DrawText(renderer, color, text, transform);
		}
        public static void Text(string text, TextRenderer renderer, Point location) => Text(text, renderer, Matrix.Translation(location), Colors.Black);
		public static void Text(string text, TextRenderer renderer, Point location, Color color) => Text(text, renderer, Matrix.Translation(location), color);
		public static void Text(string text, TextRenderer renderer, Transform transform, Color color)
		{
			Contract.Requires<ArgumentNullException>(renderer != null && transform != null);
			Text(text, renderer, transform.GetMatrix(), color);
		}
        public static void Text(string text, Font font, Point location) => Text(text, font, location, Colors.Black);
		public static void Text(string text, Font font, Point location, Color color) => Text(text, new TextRenderer(font), Matrix.Translation(location), color);
        public static void Text(string text, Font font, FontAlignment alignment, Point location) => Text(text, new TextRenderer(font, alignment), Matrix.Translation(location), Colors.Black);
		public static void Text(string text, Font font, FontAlignment alignment, Point location, Color color) => Text(text, new TextRenderer(font, alignment), Matrix.Translation(location), color);

		//public static void Text(string text, Font font, Color color, double x, double y) => CurrentSurface.DrawText(font, )

		/*
        public static void Text(string text, Font font, Color color, Transform transform) => CurrentSurface.DrawText(font, FontAlignment.TopLeft, color, text, transform);
        
        public static void Text(string text, Font font, FontAlignment alignment, Color color, double x, double y) => CurrentSurface.DrawText(font, alignment, color, text, new PointF(x, y));

        public static void Text(string text, Font font, FontAlignment alignment, Color color, Point location) => CurrentSurface.DrawText(font, alignment, color, text, (PointF)location);

        public static void Text(string text, Font font, FontAlignment alignment, Color color, Transform transform) => CurrentSurface.DrawText(font, alignment, color, text, transform);
        */



    }
}
