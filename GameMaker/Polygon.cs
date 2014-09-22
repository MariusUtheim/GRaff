using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public class Polygon
	{
		private Point[] pts;

		public Polygon(IEnumerable<Point> pts)
		{
			this.pts = pts.ToArray();
		}

		public Polygon(params Point[] pts)
		{
			if (pts == null)
				throw new ArgumentNullException("pts", "Cannot be null.");
			this.pts = pts.Clone() as Point[];
		}

		private void _SanityCheck()
		{
			if (Length > 2)
			{
				double sum = 0;
				for (int i = 0; i < Length; i++)
				{
					Angle vertex;
				}
			}
		}

		public int Length => pts.Length;

		public Point Vertex(int index)
		{
			if (Length == 0)
				throw new InvalidOperationException("The specified polygon has no vertices.");
			return pts[(index % pts.Length + pts.Length) % pts.Length];
		}

		public Line Edge(int index)
		{
			if (Length == 0)
				throw new InvalidOperationException("The specified polygon has no vertices.");
			return new Line(Vertex(index), Vertex(index + 1));
		}

		public IEnumerable<Point> Vertices
		{
			get { return pts.AsEnumerable(); }
		}

		public IEnumerable<Line> Edges
		{
			get
			{
				for (int i = 1; i < pts.Length; i++)
					yield return new Line(pts[i], pts[i - 1]);
				if (pts.Length != 0)
					yield return new Line(pts[0], pts[Length - 1]);
			}
		}

		public bool ContainsPoint(Point pt)
		{
			/**
			 * If the polygon is convex then one can consider the polygon as a "path" from the first vertex. 
			 * A point is on the interior of this polygons if it is always on the same side of all the line segments making up the path.
			 * Given a line segment between P0 (x0,y0) and P1 (x1,y1), another point P (x,y) has the following relationship to the line segment:
			 *
			 * Compute (y - y0) (x1 - x0) - (x - x0) (y1 - y0) 
			 * if it is less than 0 then P is to the right of the line segment, 
			 * if greater than 0 it is to the left, if equal to 0 then it lies on the line segment.
			 * */

			foreach (Line L in Edges)
				if (L.RightNormal.DotProduct(pt - L.Origin) > 0)
					return false;

			return true;
		}

		public bool ContainsPoint(double x, double y)
		{
			return ContainsPoint(new Point(x, y));
		}

		public bool Intersects(Polygon other)
		{
			return this._Intersects(other) && other._Intersects(this);
		}

		private bool _Intersects(Polygon other)
		{
			/**
			 * Using the separation axis theorem: 
			 * http://stackoverflow.com/questions/753140/how-do-i-determine-if-two-convex-polygons-intersect
			 * */
			bool result = true;
			IEnumerable<Point> otherVertices = other.Vertices;
			foreach (Line l in Edges)
			{
				Vector n = l.RightNormal;
				if (otherVertices.All(pt => n.DotProduct(pt - l.Origin) > 0))
					return false;
			}

			return result;
		}
	}
}
