using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff
{
#warning More?
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

			public static void TriangleStrip(Color color, Point origin, Point v1, Point v2, params Point[] verts)
			{
				Contract.Requires<ArgumentNullException>(verts != null);
				var vertices = new GraphicsPoint[3 + verts.Length];
				for (var i = 0; i < verts.Length; i++)
					vertices[i + 3] = (GraphicsPoint)verts[i];
				Device.Draw(vertices, color, PrimitiveType.Triangles);
			}
            
			public static void Rectangles(Color color, params Rectangle[] rects)
			{
				Contract.Requires<ArgumentNullException>(rects != null);
				var vertices = new GraphicsPoint[4 * rects.Length];
				for (var i = 0; i < rects.Length; i++)
				{
					vertices[4 * i] = new GraphicsPoint(rects[i].Left, rects[i].Top);
					vertices[4 * i + 1] = new GraphicsPoint(rects[i].Right, rects[i].Top);
					vertices[4 * i + 2] = new GraphicsPoint(rects[i].Right, rects[i].Bottom);
					vertices[4 * i + 3] = new GraphicsPoint(rects[i].Left, rects[i].Bottom);
				}
				Device.Draw(vertices, color, PrimitiveType.Quads);

			}
		}
    }
}
