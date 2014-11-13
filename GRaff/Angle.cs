using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	/// <summary>
	/// Represents an angle, that can be specified in angles or degrees.
	/// The value of the angle will always be in the range [0, τ).
	/// </summary>
#warning TODO: Test this properly
	public struct Angle
	{
		/*C#6.0*/
		private const double radToData = UInt32.MaxValue / GMath.Tau;
		private const double degToData = UInt32.MaxValue / 360.0;
		private const double dataToDeg = 360.0 / UInt32.MaxValue;
		private const double dataToRad = GMath.Tau / UInt32.MaxValue;
		private const uint acuteThreshold = UInt32.MaxValue / 2;
        private uint _data;

		/// <summary>
		/// Represents an angle of zero.
		/// </summary>
		public static readonly Angle Zero = new Angle();

		/// <summary>
		/// Represents the smallest representable non-zero angle.
		/// </summary>
		public static readonly Angle Epsilon = new Angle(1);

		/// <summary>
		/// Represents the largest representable angle that is less than a full turn.
		/// </summary>
		public static readonly Angle MaxAngle = new Angle(UInt32.MaxValue);

		private Angle(uint data)
		{
			_data = data;
		}

		/// <summary>
		/// Creates a GRaff.Angle with a value specified in radians.
		/// </summary>
		/// <param name="radians">The angle, in radians.</param>
		/// <returns>the created GRaff.Angle</returns>
		public static Angle Rad(double radians) { return new Angle(GMath.RoundUInt((radians * radToData) % UInt32.MaxValue)); }

		/// <summary>
		/// Creates a GRaff.Angle with a value specified in degrees.
		/// </summary>
		/// <param name="degrees">The angle, in degrees</param>
		/// <returns>the created GRaff.Angle</returns>
		public static Angle Deg(double degrees) { return new Angle(GMath.RoundUInt((degrees * degToData) % UInt32.MaxValue)); }

		
		/// <summary>
		/// Gets the value of this angle, in degrees.
		/// </summary>
		public double Degrees { get { return _data * dataToDeg; } }


		/// <summary>
		/// Gets the value of this angle, in radians.
		/// </summary>
		public double Radians { get { return _data * dataToRad; } }


		/// <summary>
		/// Finds the direction of the vector from the origin to the specified (x, y) point.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		/// <returns>The direction of the vector from the origin to the specified point.</returns>
		public static Angle Direction(double x, double y) { return GMath.Atan2(y, x); }

		/// <summary>
		/// Finds the direction of the vector from the origin to the specified point.
		/// </summary>
		/// <param name="p">The point.</param>
		/// <returns>The direction of the vector from the origin to the specified point.</returns>
		public static Angle Direction(Point p) { return GMath.Atan2(p.Y, p.X); }


		/// <summary>
		/// Finds the direction of the vector from the point (x1,y1) to the point (x2,y2).
		/// </summary>
		/// <param name="x1">The x-coordinate of the first point.</param>
		/// <param name="y1">The y-coordinate of the first point.</param>
		/// <param name="x2">The x-coordinate of the second point.</param>
		/// <param name="y2">The y-coordinate of the second point.</param>
		/// <returns>The direction of the vector from the first to the second point.</returns>
		public static Angle Direction(double x1, double y1, double x2, double y2) { return GMath.Atan2(y2 - y1, x2 - x1); }

		/// <summary>
		/// Finds the direction of the vector between the two points.
		/// </summary>
		/// <param name="p1">The first point.</param>
		/// <param name="p2">The second point.</param>
		/// <returns>The direction of the vector from the first to the second point.</returns>
		public static Angle Direction(Point p1, Point p2) { return GMath.Atan2(p2.Y - p1.Y, p2.X - p1.X); }

		/// <summary>
		/// Computes the acute angle between the two angles. This value is always in the interval [0° and 180°).
		/// </summary>
		/// <param name="deg1">The first angle, in degrees.</param>
		/// <param name="deg1">The second angle, in degrees.</param>
		/// <returns>The acute angle between the two angles.</returns>
		public static Angle Acute(double deg1, double deg2) { return _acute(deg2 - deg1); }

		/// <summary>
		/// Computes the acute angle between the two angles. This value is always between 0° and 180°.
		/// </summary>
		/// <param name="a1">The first GRaff.Angle to compute the acute angle with.</param>
		/// <param name="a2">The second GRaff.Angle to compute the acute angle with.</param>
		/// <returns>The acute angle between the two angles.</returns>
		public static Angle Acute(Angle a1, Angle a2) { return unchecked(new Angle((a1._data - a2._data < acuteThreshold) ? (a1._data - a2._data) : (a2._data - a1._data))); }

		private static Angle _acute(double d)
		{
			d = (d % 360 + 360) % 360;
			if (d > 180)
				d = 360 - d;
			return Angle.Deg(d);
		}


		/// <summary>
		/// Converts this GRaff.Angle to a human-readable string, showing the value in degrees.
		/// </summary>
		/// <returns>A string that represents this GRaff.Angle</returns>
		public override string ToString() { return String.Format("{0}\u00B0", Degrees); }

		/// <summary>
		/// Specifies whether this GRaff.Angle is equal to the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.Angle and has the same value as this GRaff.Angle.</returns>
		public override bool Equals(object obj) { return (obj is Angle) ? (this == (Angle)obj) : base.Equals(obj); }

		/// <summary>
		/// Returns a hash code for this GRaff.Angle.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.Angle.</returns>
		public override int GetHashCode() { return _data.GetHashCode(); }

		/// <summary>
		/// Compares two GRaff.Angle objects. The result specifies whether they are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Angle to compare.</param>
		/// <param name="right">The second GRaff.Angle to compare.</param>
		/// <returns>true if the values of the two GRaff.Angle structures are equal.</returns>
		public static bool operator ==(Angle left, Angle right) { return (left._data == right._data); }

		/// <summary>
		/// Compares two GRaff.Angle objects. The result specifies whether they are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.Angle to compare.</param>
		/// <param name="right">The second GRaff.Angle to compare.</param>
		/// <returns>true if the values of the two GRaff.Angle structures are unequal.</returns>
		public static bool operator !=(Angle left, Angle right) { return (left._data != right._data); }


		/// <summary>
		/// Computes the sum of the two angles.
		/// </summary>
		/// <param name="left">The first GRaff.Angle.</param>
		/// <param name="right">The second GRaff.Angle.</param>
		/// <returns>The sum of the two angles.</returns>
		public static Angle operator +(Angle left, Angle right) { return unchecked(new Angle(left._data + right._data)); }


		/// <summary>
		/// Computes the clockwise difference of the two angles. 
		/// </summary>
		/// <param name="left">The first GRaff.Angle.</param>
		/// <param name="right">The second GRaff.Angle.</param>
		/// <returns>The difference of the two angles.</returns>
		public static Angle operator -(Angle left, Angle right) { return unchecked(new Angle(left._data - right._data)); }

		/// <summary>
		/// Computes the conjugate of this GRaff.Angle structure.
		/// The two angles will sum to 360°. This is equivalent to mirroring the angle over the x-axis.
		/// </summary>
		/// <param name="a">The angle to invert.</param>
		/// <returns>The inverted GRaff.Angle.</returns>
		public static Angle operator -(Angle a) { return unchecked(new Angle(1 + ~a._data)); }

		/// <summary>
		/// Scales the angle by a specified scalar.
		/// </summary>
		/// <param name="a">The GRaff.Angle to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled angle.</returns>
		public static Angle operator *(Angle a, double d) { return Angle.Deg(a.Degrees * d); }


		/// <summary>
		/// Scales the angle by a specified scalar.
		/// </summary>
		/// <param name="d">The double to scale by.</param>
		/// <param name="a">The GRaff.Angle to be scaled.</param>
		/// <returns>The scaled angle.</returns>
		public static Angle operator *(double d, Angle a) { return Angle.Deg(d * a.Degrees); }
	}
}
