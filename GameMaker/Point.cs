using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	/// <summary>
	/// Represents a point in space. This struct is immutable.
	/// </summary>
	public struct Point
	{
		/// <summary>
		/// Represents the point (0, 0).
		/// </summary>
		public static readonly Point Zero = new Point(0, 0);

		/// <summary>
		/// Initializes a new instance of the GameMaker.Point class, using the specified x- and y-coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		public Point(double x, double y)
			: this()
		{
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Gets the x-coordinate of this GameMaker.Point.
		/// </summary>
		public double X { get; private set; }

		/// <summary>
		/// Gets the y-coordinate of this GameMaker.Point.
		/// </summary>
		public double Y { get; private set; }

		/// <summary>
		/// Specifies whether this GameMaker.Point contains the same coordinates as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>True if obj is a GameMaker.Point and has the same coordinates as this GameMaker.Point.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Point)
				return this == (Point)obj;
			return base.Equals(obj);
		}

		/// <summary>
		/// Returns a hash code for this GameMaker.Point.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GameMaker.Point.</returns>
		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		/// <summary>
		/// Converts this GameMaker.Point to a human-readable string, showing the value of the coordinates.
		/// </summary>
		/// <returns>A string that represents this GameMaker.Point.</returns>
		public override string ToString()
		{
			return String.Format("({0}, {1})", X, Y);
		}

		/// <summary>
		/// Computes the coordinate-wise sum of the two GameMaker.Point structures.
		/// </summary>
		/// <param name="left">The first GameMaker.Point.</param>
		/// <param name="right">The second GameMaker.Point.</param>
		/// <returns>The sum of the coordinates of each GameMaker.Point.</returns>
		public static Point operator +(Point left, Point right)
		{
			return new Point(left.X + right.X, left.Y + right.Y);
		}

		/// <summary>
		/// Translates the GameMaker.Point by a specified GameMaker.Vector.
		/// </summary>
		/// <param name="p">The GameMaker.Point to be translated.</param>
		/// <param name="v">The GameMaker.Vector to translate by.</param>
		/// <returns>The translated GameMaker.Point.</returns>
		public static Point operator +(Point p, Vector v)
		{
			return new Point(p.X + v.X, p.Y + v.Y);
		}

		/// <summary>
		/// Translates the GameMaker.Point by a specified GameMaker.IntVector.
		/// </summary>
		/// <param name="p">The GameMaker.Point to be translated.</param>
		/// <param name="v">The GameMaker.IntVector to translate by.</param>
		/// <returns>The translated GameMaker.Point.</returns>
		public static Point operator +(Point p, IntVector v)
		{
			return new Point(p.X + v.X, p.Y + v.Y);
		}

		/// <summary>
		/// Computes the vector between the two specified GameMaker.Point structures.
		/// </summary>
		/// <param name="dest">The destination GameMaker.Point.</param>
		/// <param name="orig">The origin GameMaker.Point.</param>
		/// <returns>The GameMaker.Vector pointing from orig to dest.</returns>
		public static Vector operator -(Point dest, Point orig)
		{
			return new Vector(dest.X - orig.X, dest.Y - orig.Y);
		}

		/// <summary>
		/// Translates the GameMaker.Point by a specified GameMaker.Vector.
		/// </summary>
		/// <param name="p">The GameMaker.Point to be translated.</param>
		/// <param name="v">The negative GameMaker.Vector to translate by.</param>
		/// <returns>The translated GameMaker.Point.</returns>
		public static Point operator -(Point p, Vector v)
		{
			return new Point(p.X - v.X, p.Y - v.Y);
		}

		/// <summary>
		/// Translates the GameMaker.Point by a specified GameMaker.IntVector.
		/// </summary>
		/// <param name="p">The GameMaker.Point to be translated.</param>
		/// <param name="v">The negative GameMaker.IntVector to translate by.</param>
		/// <returns>The translated GameMaker.Point.</returns>
		public static Point operator -(Point p, IntVector v)
		{
			return new Point(p.X - v.X, p.Y - v.Y);
		}

		/// <summary>
		/// Scales the GameMaker.Point by a specified scalar.
		/// </summary>
		/// <param name="p">The GameMaker.Point to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled GameMaker.Point.</returns>
		public static Point operator *(Point p, double d)
		{
			return new Point(p.X * d, p.Y * d);
		}

		/// <summary>
		/// Scales the GameMaker.Point by a specified scalar.
		/// </summary>
		/// <param name="d">The double to scale by.</param>
		/// <param name="p">The GameMaker.Point to be scaled.</param>
		/// <returns>The scaled GameMaker.Point.</returns>
		public static Point operator *(double d, Point p)
		{
			return new Point(d * p.X, d * p.Y);
		}

		/// <summary>
		/// Scales the GameMaker.Point by dividing by a specified scalar.
		/// </summary>
		/// <param name="p">The GameMaker.Point to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled GameMaker.Point.</returns>
		public static Point operator /(Point p, double d)
		{
			return new Point(p.X / d, p.Y / d);
		}

		/// <summary>
		/// Compares two GameMaker.Point objects. The result specifies whether their x- and y-coordinates are equal.
		/// </summary>
		/// <param name="p1">The first GameMaker.Point to compare.</param>
		/// <param name="p2">The second GameMaker.Point to compare.</param>
		/// <returns>True if the x- and y-coordinates of the two GameMaker.Point structures are equal.</returns>
		public static bool operator ==(Point p1, Point p2)
		{
			return p1.X == p2.X && p1.Y == p2.Y;
		}

		/// <summary>
		/// Compares two GameMaker.Point objects. The result specifies whether their x- and y-coordinates are unequal.
		/// </summary>
		/// <param name="p1">The first GameMaker.Point to compare.</param>
		/// <param name="p2">The second GameMaker.Point to compare.</param>
		/// <returns>True if the x- and y-coordinates of the two GameMaker.Point structures are unequal.</returns>
		public static bool operator !=(Point p1, Point p2)
		{
			return p1.X != p2.X || p1.Y != p2.Y;
		}

		/// <summary>
		/// Converts the specified GameMaker.Point to a GameMaker.Vector.
		/// </summary>
		/// <param name="p">The GameMaker.Point to be converted.</param>
		/// <returns>The GameMaker.Vector that results from the conversion.</returns>
		public static explicit operator Vector(Point p) { return new Vector(p.X, p.Y); }
	}
}
