using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public class Polygon
	{
		public int Length
		{
			get { throw new NotImplementedException(); }
		}

		public Point Vertex(int index)
		{
			throw new NotImplementedException();
		}

		public Line Edge(int index)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Point> Vertices
		{
			get { throw new NotImplementedException(); }
		}

		public IEnumerable<Line> Edges
		{
			get { throw new NotImplementedException(); }
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
				if (L.LeftNormal.DotProduct(pt - L.Origin) > 0)
					return false;

			return true;
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
				Vector n = l.LeftNormal;
				if (otherVertices.All(pt => n.DotProduct(pt - l.Origin) > 0))
					return false;
			}

			return result;
		}
	}
}
