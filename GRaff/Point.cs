using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GRaff
{
	/// <summary>
	/// Represents a point in space.
	/// </summary>
	public struct Point
	{ /*C#6.0*/
		public Point(double x, double y)
			: this()
		{
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Gets the x-coordinate of this GRaff.Point.
		/// </summary>
		public double X { get; private set; }

		/// <summary>
		/// Gets the y-coordinate of this GRaff.Point.
		/// </summary>
		public double Y { get; private set; }


		/// <summary>
		/// Represents the point (0, 0).
		/// </summary>
		public static readonly Point Zero = new Point(0, 0);


		/// <summary>
		/// Converts this GRaff.Point to a human-readable string, showing the value of the coordinates.
		/// </summary>
		/// <returns>A string that represents this GRaff.Point.</returns>
		public override string ToString() { return String.Format("({0}, {1})", X, Y); }


		/// <summary>
		/// Specifies whether this GRaff.Point contains the same coordinates as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.Point and has the same coordinates as this GRaff.Point.</returns>
		public override bool Equals(object obj) { return ((obj is Point) ? (this == (Point)obj) : base.Equals(obj)); }


		/// <summary>
		/// Returns a hash code for this GRaff.Point.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.Point.</returns>
		public override int GetHashCode() { return GMath.HashCombine(X.GetHashCode(), Y.GetHashCode()); }


		/// <summary>
		/// Compares two GRaff.Point objects. The result specifies whether their x- and y-coordinates are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Point to compare.</param>
		/// <param name="right">The second GRaff.Point to compare.</param>
		/// <returns>true if the x- and y-coordinates of the two GRaff.Point objects are equal.</returns>
		public static bool operator ==(Point left, Point right) { return (left.X == right.X && left.Y == right.Y); }


		/// <summary>
		/// Compares two GRaff.Point objects. The result specifies whether their x- and y-coordinates are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.Point to compare.</param>
		/// <param name="right">The second GRaff.Point to compare.</param>
		/// <returns>true if the x- and y-coordinates of the two GRaff.Point structures are unequal.</returns>
		public static bool operator !=(Point left, Point right) { return (left.X != right.X || left.Y != right.Y); }
		

		/// <summary>
		/// Computes the coordinate-wise sum of the two GRaff.Point structures.
		/// </summary>
		/// <param name="left">The first GRaff.Point.</param>
		/// <param name="right">The second GRaff.Point.</param>
		/// <returns>The sum of the coordinates of each GRaff.Point.</returns>
		public static Point operator +(Point left, Point right) { return new Point(left.X + right.X, left.Y + right.Y); }


		/// <summary>
		/// Translates the GRaff.Point by a specified GRaff.Vector.
		/// </summary>
		/// <param name="p">The GRaff.Point to be translated.</param>
		/// <param name="v">The GRaff.Vector to translate by.</param>
		/// <returns>The translated GRaff.Point.</returns>
		public static Point operator +(Point p, Vector v) { return new Point(p.X + v.X, p.Y + v.Y); }


		/// <summary>
		/// Translates the GRaff.Point by a specified GRaff.IntVector.
		/// </summary>
		/// <param name="p">The GRaff.Point to be translated.</param>
		/// <param name="v">The GRaff.IntVector to translate by.</param>
		/// <returns>The translated GRaff.Point.</returns>
		public static Point operator +(Point p, IntVector v) { return new Point(p.X + v.X, p.Y + v.Y); }


		/// <summary>
		/// Computes the vector between the two specified GRaff.Point structures.
		/// </summary>
		/// <param name="dest">The destination GRaff.Point.</param>
		/// <param name="orig">The origin GRaff.Point.</param>
		/// <returns>The GRaff.Vector pointing from orig to dest.</returns>
		public static Vector operator -(Point dest, Point orig) { return new Vector(dest.X - orig.X, dest.Y - orig.Y); }


		/// <summary>
		/// Translates the GRaff.Point by a specified GRaff.Vector.
		/// </summary>
		/// <param name="p">The GRaff.Point to be translated.</param>
		/// <param name="v">The negative GRaff.Vector to translate by.</param>
		/// <returns>The translated GRaff.Point.</returns>
		public static Point operator -(Point p, Vector v) { return new Point(p.X - v.X, p.Y - v.Y); }


		/// <summary>
		/// Translates the GRaff.Point by a specified GRaff.IntVector.
		/// </summary>
		/// <param name="p">The GRaff.Point to be translated.</param>
		/// <param name="v">The negative GRaff.IntVector to translate by.</param>
		/// <returns>The translated GRaff.Point.</returns>
		public static Point operator -(Point p, IntVector v) { return new Point(p.X - v.X, p.Y - v.Y); }


		/// <summary>
		/// Scales the GRaff.Point by a specified scalar.
		/// </summary>
		/// <param name="p">The GRaff.Point to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled GRaff.Point.</returns>
		public static Point operator *(Point p, double d) { return new Point(p.X * d, p.Y * d); }


		/// <summary>
		/// Scales the GRaff.Point by a specified scalar.
		/// </summary>
		/// <param name="d">The double to scale by.</param>
		/// <param name="p">The GRaff.Point to be scaled.</param>
		/// <returns>The scaled GRaff.Point.</returns>
		public static Point operator *(double d, Point p) { return new Point(d * p.X, d * p.Y); }


		/// <summary>
		/// Scales the GRaff.Point by dividing by a specified scalar.
		/// </summary>
		/// <param name="p">The GRaff.Point to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled GRaff.Point.</returns>
		public static Point operator /(Point p, double d) { return new Point(p.X / d, p.Y / d); }


		/// <summary>
		/// Converts the specified GRaff.Point to a GRaff.Vector.
		/// </summary>
		/// <param name="p">The GRaff.Point to be converted.</param>
		/// <returns>The GRaff.Vector that results from the conversion.</returns>
		public static explicit operator Vector(Point p) { return new Vector(p.X, p.Y); }
	}
}
