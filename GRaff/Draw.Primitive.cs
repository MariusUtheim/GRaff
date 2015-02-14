using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
#if !PUBLISH
	public static partial class Draw
    {
		public static class Primitive
		{
			public static void Points(params Point[] vertices)
			{
				throw new NotImplementedException();
			}

			public static void Lines(params Line[] lines)
			{
				throw new NotImplementedException();
			}

			public static void LineStrip(Point origin, Point v1, params Point[] vertices)
			{
				throw new NotImplementedException();
			}

			public static void Triangles(params Point[] triangles)
			{
				throw new NotImplementedException();
			}

			public static void TriangleStrip(Point origin, Point v1, Point v2, params Point[] vertices)
			{
				throw new NotImplementedException();
			}

			public static void TriangleFan(Point origin, params Point[] vertices)
			{
				throw new NotImplementedException();
			}
		}
    }
#endif
}
