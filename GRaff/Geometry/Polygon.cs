using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace GRaff
{ 
	/// <summary>
	/// Specifies a positively-oriented convex polygon.
	/// </summary>
	/// <remarks>
	/// If the constructor vertices specify a negatively-oriented polygon, the direction is reversed.
	/// The condition that the polygon must be convex is a strong condition that is unlikely to occur with arbitraty vertices.
	/// In most cases, developers should not have to create their own polygons directly.
	/// </remarks>
	public sealed class Polygon
	{
		private Point[] _pts;

		internal Polygon(Point[] pts)
		{
			_pts = pts;	// Used to create Polygon in an efficient way without doing sanity checks
		}

		public Polygon(IEnumerable<Point> pts)
		{
			Contract.Requires<ArgumentNullException>(pts != null);
			_pts = pts.ToArray();

			if (_pts.Length <= 2)
				return;

			double sum = 0;
			Angle a;
			Vector previous, next;
			previous = Edge(-1).Direction;
			next = Edge(0).Direction;

			a = next.Direction - previous.Direction;
			if (a.Degrees > 180)
			{
				Array.Reverse(_pts);
				previous = Edge(-1).Direction;
				next = Edge(0).Direction;
				a = next.Direction - previous.Direction;
			}
			sum += a.Degrees;

			for (int i = 1; i < _pts.Length; i++)
			{
				previous = next;
				next = Edge(i).Direction;
				a = next.Direction - previous.Direction;
				if (a.Degrees > 180)
					throw new ArgumentException("The points must specify a convex polygon.");
				sum += a.Degrees;
			}

			if (GMath.Abs(sum - 360) > GMath.DefaultDelta)
				throw new ArgumentException($"The points must specify a convex polygon with winding number equal to 1. (Winding is {sum} degrees)");
		}

		[ContractInvariantMethod]
		private void invariants()
		{
			Contract.Invariant(_pts == null || _pts.Length > 0);
		}

		#region Static constructors

		public static Polygon Regular(int degree, double radius)
		{
			Contract.Requires<ArgumentOutOfRangeException>(degree >= 2);
			return Regular(degree, radius, Point.Zero);
		}

		public static Polygon Regular(int degree, double radius, Point center)
		{
			Contract.Requires<ArgumentOutOfRangeException>(degree >= 2);

			double dt = GMath.Tau / degree;
			double c = GMath.Cos(dt), s = GMath.Sin(dt);

			double x = 0, y = -radius, tmp;
			Point[] pts = new Point[degree];

			for (int i = 0; i < degree; i++)
			{
				pts[i] = new Point(center.X + x, center.Y + y);

				tmp = x;
				x = c * x - s * y;
				y = s * tmp + c * y;
			}

			return new Polygon(pts);
		}

		public static Polygon Rectangle(double width, double height)
		{
			return new Polygon(new[] {
				new Point(-width / 2, -height / 2),
				new Point(width / 2, -height / 2),
				new Point(width / 2, height / 2),
				new Point(-width / 2, height / 2)
			});
		}

		public static Polygon Circle(double radius)
		{
			return Circle(Point.Zero, radius);
		}

		public static Polygon Circle(Point center, double radius)
		{
			if (radius == 0)
				return new Polygon(new[] { center });

			int precision = (int)GMath.Ceiling(GMath.Tau * GMath.Abs(radius));
			if (precision < 2)
				precision = 2;
			
			return Regular(precision, radius, center);
		}

		public static Polygon Ellipse(Point center, double xRadius, double yRadius)
		{
			int precision = (int)GMath.Ceiling(GMath.Pi * GMath.Abs(xRadius + yRadius));
			if (precision <= 0)
				return new Polygon(new[] { center });
			double dt = GMath.Tau / precision;
			double c = GMath.Cos(dt), s = GMath.Sin(dt);

			double x = 1, y = 0, tmp;

			var pts = new Point[precision];
			for (int i = 0; i < precision; i++)
			{
				pts[i] = new Point(center.X + x * xRadius, center.Y + y * yRadius);

				tmp = x;
				x = c * x - s * y;
				y = s * tmp + c * y;
			}

			return new Polygon(pts);
		}

		public static Polygon Ellipse(Rectangle rectangle)
		{
			return Ellipse(rectangle.Center, rectangle.Width / 2, rectangle.Height / 2);
		}

		#endregion

		/// <summary>
		/// Gets the number of vertices in this GRaff.Polygon
		/// </summary>
		public int Length => _pts.Length;

		public Point Center => _pts.Aggregate((p, q) => p + q) / _pts.Length;

		public Point Vertex(int index)
			=> _pts[(index % _pts.Length + _pts.Length) % _pts.Length];


		public Line Edge(int index)
			=> checked(new Line(Vertex(index), Vertex(index + 1)));

		public IEnumerable<Point> Vertices
			=> Array.AsReadOnly(_pts);

		public IEnumerable<Line> Edges
		{
			get
			{
				for (int i = 0; i < _pts.Length - 1; i++)
					yield return new Line(_pts[i], _pts[i + 1]);
				if (_pts.Length != 0)
					yield return new Line(_pts[Length - 1], _pts[0]);
			}
        }

		
        /// <summary>
        /// Returns true if the point is strictly inside the Polygon. If this polygon has 
        /// 2 or less vertices, this method always returns false. If the polygon has 3 or more
        /// vertices and the point lies on the boundary, this method has no definite behaviour.
        /// </summary>
        public bool ContainsPoint(Point p)
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

            if (Length <= 2)
                return false;

			foreach (Line L in Edges)
				if (L.LeftNormal.Dot(p - L.Origin) >= 0)
					return false;

			return true;
		}

        /// <summary>
        /// Returns true if the point is inside the Polygon or on its boundary.
        /// </summary>
        public bool ContainsPoint(double x, double y)
			=> ContainsPoint(new Point(x, y));

        /// <summary>
        /// Returns true if this Polygon intersects other Polygon.
        /// For polygons with 3 or more vertices, the boundary is not considered part of the
        /// polygon. For polygons with 2 vertices, the endpoints are not considered part of the polygon.
        /// For a polygons with 1 vertex, that point is considered part of the polygon.
        /// Polygons with 0 vertices will never be considered intersecting other polygons.
        /// </summary>
        /// <returns></returns>
        public static bool Intersects(Polygon first, Polygon second)
        {
            if (first == null || second == null || first.Length == 0 || second.Length == 0)
                return false;
            if (first.Length < second.Length)
                (first, second) = (second, first);

            // From here on out: first.Length >= second.Length >= 1
            if (first.Length == 1)
                return first.Vertex(0) == second.Vertex(0);
            else if (first.Length == 2)
            {
                if (second.Length == 1)
                    return false;
                else // if (second.Length == 2)
                    return first.Edge(0).Intersects(second.Edge(0));
            }
            else
            {
                if (second.Length == 1)
                    return first.ContainsPoint(second.Vertex(0));
                else if (second.Length == 2)
                {
                    if (first.ContainsPoint(second.Vertex(0)) || first.ContainsPoint(second.Vertex(1)))
                        return true;
                    var l = second.Edge(0);
                    return first.Edges.Any(e => e.Intersects(l));
                }
                else
                    return first._Intersects(second) && second._Intersects(first);
            }
        }

        public bool Intersects(Polygon other) => Intersects(this, other);

		private bool _Intersects(Polygon other)
		{
			if (other == null) return false;
			/**
			 * Using the separation axis theorem: 
			 * http://stackoverflow.com/questions/753140/how-do-i-determine-if-two-convex-polygons-intersect
			 * */
			IEnumerable<Point> otherVertices = other.Vertices;
			foreach (Line l in Edges)
			{
				if (otherVertices.All(pt => l.LeftNormal.Dot(pt - l.Origin) >= 0))
					return false;
			}

			return true;
		}



		public static Polygon operator +(Polygon left, Vector right)
			=> new Polygon(left._pts.Select(p => p + right).ToArray());
		
		public static Polygon operator +(Vector left, Polygon right)
			=> new Polygon(right._pts.Select(p => left + p).ToArray());

		public static Polygon operator -(Polygon left, Vector right)
			=> new Polygon(left._pts.Select(p => p - right).ToArray());


	}
}
