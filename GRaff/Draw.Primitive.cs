using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff
{
#if !PUBLISH
	public static partial class Draw
    {
		public static class Primitive
		{
			public static void Custom(PrimitiveType primitiveType, Color color, params Point[] vertices)
			{
				Contract.Requires<ArgumentNullException>(vertices != null);
				CurrentSurface.DrawPrimitive(vertices.Select(p => (GraphicsPoint)p).ToArray(), Enumerable.Repeat(color, vertices.Length).ToArray(), primitiveType);
            }

			public static void Points(Color color, params Point[] vertices)
			{
				Contract.Requires<ArgumentNullException>(vertices != null);
				CurrentSurface.DrawPrimitive(vertices.Select(p => (GraphicsPoint)p).ToArray(), Enumerable.Repeat(color, vertices.Length).ToArray(), PrimitiveType.Points);
			}

			public static void Lines(Color color, params Line[] lines)
			{
				Contract.Requires<ArgumentNullException>(lines != null);
				GraphicsPoint[] vertices = new GraphicsPoint[lines.Length * 2];
				for (var i = 0; i < lines.Length; i++)
				{
					vertices[2 * i] = (GraphicsPoint)lines[i].Origin;
					vertices[2 * i + 1] = (GraphicsPoint)lines[i].Destination;
				}
				CurrentSurface.DrawPrimitive(vertices, Enumerable.Repeat(color, 2 * lines.Length).ToArray(), PrimitiveType.Lines);
			}

			public static void Lines(Color color, GraphicsPoint[] vertices)
			{
				Contract.Requires<ArgumentNullException>(vertices != null);
				Contract.Requires<ArgumentException>(vertices.Length % 2 == 0);
				CurrentSurface.DrawPrimitive(vertices, Enumerable.Repeat(color, vertices.Length).ToArray(), PrimitiveType.Lines);
			}

			public static void LineStrip(Color color, Point[] vertices)
			{
				Contract.Requires<ArgumentNullException>(vertices != null);
				Contract.Requires<ArgumentException>(vertices.Length >= 2);
				CurrentSurface.DrawPrimitive(vertices.Select(v => (GraphicsPoint)v).ToArray(), Enumerable.Repeat(color, vertices.Length).ToArray(), PrimitiveType.LineStrip);
			}

			public static void Triangles(Color color, params Triangle[] triangles)
			{
				Contract.Requires<ArgumentNullException>(triangles != null);
				GraphicsPoint[] vertices = new GraphicsPoint[triangles.Length * 3];
				for (var i = 0; i < triangles.Length; i++)
				{
					vertices[3 * i] = (GraphicsPoint)triangles[i].V1;
					vertices[3 * i + 1] = (GraphicsPoint)triangles[i].V2;
					vertices[3 * i + 2] = (GraphicsPoint)triangles[i].V3;
				}
				CurrentSurface.DrawPrimitive(vertices, Enumerable.Repeat(color, vertices.Length).ToArray(), PrimitiveType.Triangles);
			}

			public static void TriangleStrip(Color color, Point origin, Point v1, Point v2, params Point[] verts)
			{
				Contract.Requires<ArgumentNullException>(verts != null);
				var vertices = new GraphicsPoint[3 + verts.Length];
				for (var i = 0; i < verts.Length; i++)
					vertices[i + 3] = (GraphicsPoint)verts[i];
				CurrentSurface.DrawPrimitive(vertices, Enumerable.Repeat(color, vertices.Length).ToArray(), PrimitiveType.Triangles);
			}

			public static void TriangleFan(Point origin, params Point[] vertices)
			{
				throw new NotImplementedException();
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
				CurrentSurface.DrawPrimitive(vertices, Enumerable.Repeat(color, vertices.Length).ToArray(), PrimitiveType.Quads);

			}
		}
    }
#endif
}
