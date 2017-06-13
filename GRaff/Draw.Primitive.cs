using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff
{
	public static partial class Draw
    {
		public static class Primitive
		{
			public static void Custom(PrimitiveType primitiveType, Color color, IEnumerable<Point> vertices)
			{
				Contract.Requires<ArgumentNullException>(vertices != null);
				Device.Draw(vertices.Select(p => (GraphicsPoint)p).ToArray(), color, primitiveType);
            }

            public static void Custom(PrimitiveType primitiveType, IEnumerable<(Color color, Point vertex)> primitive)
            {
                Contract.Requires<ArgumentNullException>(primitive != null);
                Device.Draw(primitive.Select(p => (GraphicsPoint)p.vertex).ToArray(), primitive.Select(p => p.color).ToArray(), primitiveType);
            }

            public static void Points(Color color, params Point[] vertices) => Custom(PrimitiveType.Points, color, vertices);

            public static void Lines(Color color, params Line[] lines) 
                => Custom(PrimitiveType.Lines, color, lines.SelectMany(l => (l.Origin, l.Destination)));
            

            public static void LineStrip(Color color, params Point[] vertices) => Custom(PrimitiveType.LineStrip, color, vertices);

            public static void Triangles(Color color, params Triangle[] triangles)
                => Custom(PrimitiveType.Triangles, color, triangles.SelectMany(t => (t.V1, t.V2, t.V3)));

			public static void TriangleStrip(Color color, params Point[] vertices)
			{
				Contract.Requires<ArgumentNullException>(vertices != null);
                Contract.Requires<ArgumentException>(vertices.Length >= 3);
				var v = new GraphicsPoint[3 + vertices.Length];
				for (var i = 0; i < vertices.Length; i++)
					v[i + 3] = (GraphicsPoint)vertices[i];
				Device.Draw(v, color, PrimitiveType.TriangleStrip);
			}

            public static void TriangleFan(Color color, Point origin, params Point[] vertices)
            {
                Contract.Requires<ArgumentNullException>(vertices != null);
                Contract.Requires<ArgumentException>(vertices.Length >= 2);

                var v = new GraphicsPoint[1 + vertices.Length];
                for (var i = 0; i < vertices.Length; i++)
                    v[i + 1] = (GraphicsPoint)vertices[i];

                Device.Draw(v, color, PrimitiveType.TriangleFan);
            }
            
			public static void Rectangles(Color color, params Rectangle[] rects)
			{
				Contract.Requires<ArgumentNullException>(rects != null);
				var vertices = new GraphicsPoint[6 * rects.Length];
				for (var i = 0; i < rects.Length; i++)
				{
					vertices[6 * i] = new GraphicsPoint(rects[i].Left, rects[i].Top);
					vertices[6 * i + 1] = vertices[6 * i + 3] = new GraphicsPoint(rects[i].Right, rects[i].Top);
					vertices[6 * i + 2] = vertices[6 * i + 4] = new GraphicsPoint(rects[i].Right, rects[i].Bottom);
					vertices[6 * i + 5] = new GraphicsPoint(rects[i].Left, rects[i].Bottom);
				}
                Device.Draw(vertices, color, PrimitiveType.Triangles);
			}

            public static void Polygon(Color color, params Point[] vertices) => Custom(PrimitiveType.Polygon, color, vertices);
            
            public static void CustomTextured(PrimitiveType primitiveType, TextureBuffer buffer, Color blend, params (GraphicsPoint vertex, GraphicsPoint texCoord)[] vertices)
            {
                Device.DrawTexture(buffer, vertices.Select(v => v.vertex).ToArray(), blend, vertices.Select(v => v.texCoord).ToArray(), primitiveType);
            }

            public static void CustomTextured(PrimitiveType primitiveType, TextureBuffer buffer, params (GraphicsPoint vertex, Color color, GraphicsPoint texCoord)[] vertices)
            {
                Device.DrawTexture(buffer, vertices.Select(v => v.vertex).ToArray(), vertices.Select(v => v.color).ToArray(), vertices.Select(v => v.texCoord).ToArray(), primitiveType);
            }
        }
    }
}
