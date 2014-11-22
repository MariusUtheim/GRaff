using System;
using System.Collections.Generic;
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

		public Polygon(IEnumerable<Point> pts)
		{
			this._pts = pts.ToArray();
			_SanityCheck();
		}

		public Polygon(params Point[] pts)
		{
			if (pts == null)
				throw new ArgumentNullException("pts", "Cannot be null.");
			this._pts = pts.Clone() as Point[];
			_SanityCheck();
		}

		public Polygon(int degree, int radius)
			: this(degree, radius, Point.Zero) { }

		public Polygon(int degree, int radius, Point center)
		{
			if (degree < 3) throw new ArgumentException("Degree must be greater than or equal to 3.", "degree");

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

			_pts = pts;
		}

		private Polygon()
		{
			return;	// Used to create Polygon in an efficient way without doing sanity checks
		}

		private void _SanityCheck()
		{
			if (_pts.Length <= 2)
				return;

			Angle sum = Angle.Zero;
			Angle a;
			Vector previous, next;
			previous = Edge(-1).Direction;
			next = Edge(0).Direction;

			a = next.Direction - previous.Direction;
			if (a.Degrees > 180)
				_pts = _pts.Reverse().ToArray();
			sum += a;

			for (int i = 1; i < _pts.Length; i++)
			{
				previous = next;
				next = Edge(i).Direction;
				a = next.Direction - previous.Direction;
				if (a.Degrees > 180)
					throw new ArgumentException("The points must specify a convex polygon.");
				sum += a;
			}

			if (sum != Angle.Epsilon)
				throw new ArgumentException("The points must specify a convex polygon with winding number equal to 1. Winding is " + sum.ToString());
		}


		public static Polygon Circle(Point center, double radius)
		{
			if (radius == 0)
				return new Polygon { _pts = new[] { center } };

			int precision = (int)GMath.Ceiling(radius);			

			double dt = GMath.Tau / precision;
			double c = GMath.Cos(dt), s = GMath.Sin(dt);

			double x = radius, y = 0, tmp;
			Point[] pts = new Point[precision];

			for (int i = 0; i < precision; i++)
			{
				pts[i] = new Point(center.X + x, center.Y + y);

				tmp = x;
				x = c * x - s * y;
				y = s * tmp + c * y;
			}

			return new Polygon { _pts = pts };
		}

		public static Polygon Ellipse(Point center, double xRadius, double yRadius)
		{
			int precision = (int)GMath.Ceiling(GMath.Pi * (xRadius + yRadius));
			double dt = GMath.Tau / precision;
			double c = GMath.Cos(dt), s = GMath.Sin(dt);

			double x = 1, y = 0, tmp;

			Point[] pts = new Point[precision];
			for (int i = 0; i < precision; i++)
			{
				pts[i] = new Point(center.X + x * xRadius, center.Y + y * yRadius);

				tmp = x;
				x = c * x - s * y;
				y = s * tmp + c * y;
			}

			return new Polygon { _pts = pts };
		}

		/*
		public static Polygon Star(int arms, double innerRadius, double outerRadius)
		{
			return Star(arms, Point.Zero, innerRadius, outerRadius);
		}
		
		public static Polygon Star(int arms, Point center, double innerRadius, double outerRadius)
		{
			if (arms < 2) throw new ArgumentException("Must have at least 2 arms.", "arms");

			double dt = GMath.Tau / arms;
			double c = GMath.Cos(dt), s = GMath.Sin(dt);

			double outerX = 0, outerY = -outerRadius;
			double innerX = innerRadius * GMath.Cos(GMath.Tau / 4 + dt / 2), innerY = innerRadius * GMath.Sin(GMath.Tau / 4 + dt / 2);
			double tmp;
			Point[] pts = new Point[arms * 2];

			for (int i = 0; i < arms; i++)
			{
				pts[2 * i] = new Point(center.X + outerX, center.Y + outerY);
				pts[2 * i + 1] = new Point(center.X + innerX, center.Y + innerY);

				tmp = outerX;
				outerX = c * outerX - s * outerY;
				outerY = s * tmp + c * outerY;

				tmp = innerX;
				innerX = c * innerX - s * innerY;
				innerY = s * tmp + c * innerY;
			}

			return new Polygon { _pts = pts };
		}
		*/
		/// <summary>
		/// Gets the number of vertices in this GRaff.Polynomial.
		/// </summary>
		public int Length { get { return _pts.Length; } }


		public Point Vertex(int index)
		{
			if (Length == 0)
				throw new InvalidOperationException("The specified polygon has no vertices.");
			return _pts[(index % _pts.Length + _pts.Length) % _pts.Length];
		}

		public Line Edge(int index)
		{
			if (Length == 0)
				throw new InvalidOperationException("The specified polygon has no vertices.");
			checked
			{
				return new Line(Vertex(index), Vertex(index + 1));
			}
		}

		public IEnumerable<Point> Vertices
		{
			get { return _pts.AsEnumerable(); }
		}

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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pt")]
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

		public bool ContainsPoint(double x, double y)
		{
			return ContainsPoint(new Point(x, y));
		}

		public bool Intersects(Polygon other)
		{
			if (other == null) return false;
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

				var x = _pts.Where(pt => pt.X > 0).Select(pt => pt.X * pt.X).First();
			}

			return result;
		}
	}
}
