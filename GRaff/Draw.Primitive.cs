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
				CurrentSurface.DrawPrimitive(vertices.Select(p => (PointF)p).ToArray(), Enumerable.Repeat(color, vertices.Length).ToArray(), primitiveType);
            }

			public static void Points(Color color, params Point[] vertices)
			{
				Contract.Requires(vertices != null);
				CurrentSurface.DrawPrimitive(vertices.Select(p => (PointF)p).ToArray(), Enumerable.Repeat(color, vertices.Length).ToArray(), PrimitiveType.Points);
			}

			public static void Lines(Color color, params Line[] lines)
			{
				Contract.Requires(lines != null);
				PointF[] vertices = new PointF[lines.Length * 2];
				for (var i = 0; i < lines.Length; i++)
				{
					vertices[2 * i] = (PointF)lines[i].Origin;
					vertices[2 * i + 1] = (PointF)lines[i].Destination;
				}
				CurrentSurface.DrawPrimitive(vertices, Enumerable.Repeat(color, 2 * lines.Length).ToArray(), PrimitiveType.Lines);
			}

			public static void LineStrip(Color color, Point origin, Point v1, params Point[] vertices)
			{
				Contract.Requires(vertices != null);
				CurrentSurface.DrawPrimitive(Enumerable.Concat(new[] { (PointF)origin, (PointF)v1 }, vertices.Cast<PointF>()).ToArray(), Enumerable.Repeat(color, vertices.Length).ToArray(), PrimitiveType.LineStrip);
			}

			public static void Triangles(Color color, params Triangle[] triangles)
			{
				Contract.Requires(triangles != null);
				PointF[] vertices = new PointF[triangles.Length * 3];
				for (var i = 0; i < triangles.Length; i++)
				{
					vertices[3 * i] = (PointF)triangles[i].V1;
					vertices[3 * i + 1] = (PointF)triangles[i].V2;
					vertices[3 * i + 2] = (PointF)triangles[i].V3;
				}
				CurrentSurface.DrawPrimitive(vertices, Enumerable.Repeat(color, vertices.Length).ToArray(), PrimitiveType.Triangles);
			}

			public static void TriangleStrip(Color color, Point origin, Point v1, Point v2, params Point[] verts)
			{
				Contract.Requires(verts != null);
				var vertices = new PointF[3 + verts.Length];
				for (var i = 0; i < verts.Length; i++)
					vertices[i + 3] = (PointF)verts[i];
				CurrentSurface.DrawPrimitive(vertices, Enumerable.Repeat(color, vertices.Length).ToArray(), PrimitiveType.Triangles);
			}

			public static void TriangleFan(Point origin, params Point[] vertices)
			{
				throw new NotImplementedException();
			}
		}
    }
#endif
}
