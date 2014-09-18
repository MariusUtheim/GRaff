using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	/// <summary>
	/// Represents an angle, that can be specified in angles or degrees.
	/// </summary>
#warning TODO: Test
#warning TODO: Two angles are equal if their value differs by a multiple of tau.
	public struct Angle(double degrees)
	{
		{
			degrees = (degrees % 360.0 + 360.0) % 360.0;
        }
		/// <summary>
		/// Represents an angle of zero.
		/// </summary>
		public static readonly Angle Zero = new Angle();

		/// <summary>
		/// Creates a GameMaker.Angle with a value specified in radians.
		/// </summary>
		/// <param name="radians">The angle, in radians.</param>
		/// <returns>the created GameMaker.Angle</returns>
		public static Angle Rad(double radians) => new Angle(radians * GMath.RadToDeg);

		/// <summary>
		/// Creates a GameMaker.Angle with a value specified in degrees.
		/// </summary>
		/// <param name="degrees">The angle, in degrees</param>
		/// <returns>the created GameMaker.Angle</returns>
		public static Angle Deg(double degrees) => new Angle(degrees);

		
		/// <summary>
		/// Gets the value of this angle, in degrees.
		/// </summary>
		public double Degrees { get; } = degrees;


		/// <summary>
		/// Gets the value of this angle, in radians.
		/// </summary>
		public double Radians => Degrees * GMath.DegToRad;


		/// <summary>
		/// Finds the direction of the vector from the origin to the specified (x, y) point.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		/// <returns>The direction of the vector from the origin to the specified point.</returns>
		public static Angle Direction(double x, double y) => GMath.Atan2(y, x);


		/// <summary>
		/// Finds the direction of the vector from the point (x1,y1) to the point (x2,y2).
		/// </summary>
		/// <param name="x1">The x-coordinate of the first point.</param>
		/// <param name="y1">The y-coordinate of the first point.</param>
		/// <param name="x2">The x-coordinate of the second point.</param>
		/// <param name="y2">The y-coordinate of the second point.</param>
		/// <returns>The direction of the vector the first to the second point.</returns>
		public static Angle Direction(double x1, double y1, double x2, double y2) => GMath.Atan2(y2 - y1, x2 - x1);


		/// <summary>
		/// Computes the acute angle between the two angles. This value is always between 0° and 180°.
		/// </summary>
		/// <param name="other">The GameMaker.Angle to compute the acute angle with.</param>
		/// <returns>The acute angle between the two angles.</returns>
		public Angle Acute(Angle other)
		{
			double d = this.Degrees - other.Degrees;
			d = (d % 360 + 360) % 360;
			if (d > 180)
				d = 360 - d;
			return Angle.Deg(d);
		}


		/// <summary>
		/// Converts this GameMaker.Angle to a human-readable string, showing the value in degrees.
		/// </summary>
		/// <returns>A string that represents this GameMaker.Angle</returns>
		public override string ToString() => String.Format("Angle {0}", Degrees);

#warning TODO: Documentation
		public override bool Equals(object obj) => (obj is Angle) ? (this == (Angle)obj) : base.Equals(obj);


		public static bool operator ==(Angle left, Angle right) => (left.Degrees == right.Degrees);


		public static bool operator !=(Angle left, Angle right) => (left.Degrees != right.Degrees);


		/// <summary>
		/// Computes the sum of the two angles.
		/// </summary>
		/// <param name="left">The first GameMaker.Angle.</param>
		/// <param name="right">The second GameMaker.Angle.</param>
		/// <returns>The sum of the two angles.</returns>
		public static Angle operator +(Angle left, Angle right) => Angle.Deg(left.Degrees + right.Degrees);


		/// <summary>
		/// Computes the clockwise difference of the two angles. 
		/// </summary>
		/// <param name="left">The first GameMaker.Angle.</param>
		/// <param name="right">The second GameMaker.Angle.</param>
		/// <returns>The difference of the two angles.</returns>
		public static Angle operator -(Angle left, Angle right) => Angle.Deg(left.Degrees - right.Degrees);
		

		/// <summary>
		/// Scales the angle by a specified scalar.
		/// </summary>
		/// <param name="a">The GameMaker.Angle to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled angle.</returns>
		public static Angle operator *(Angle a, double d) => Angle.Deg(a.Degrees * d);


		/// <summary>
		/// Scales the angle by a specified scalar.
		/// </summary>
		/// <param name="d">The double to scale by.</param>
		/// <param name="a">The GameMaker.Angle to be scaled.</param>
		/// <returns>The scaled angle.</returns>
		public static Angle operator *(double d, Angle a) => Angle.Deg(d * a.Degrees);
	}
}
