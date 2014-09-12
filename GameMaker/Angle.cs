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
	public struct Angle
	{
		//private double _radians;
		private double _degrees;

		/// <summary>
		/// Represents an angle of zero.
		/// </summary>
		public static readonly Angle Zero = new Angle();

		/// <summary>
		/// Creates a GameMaker.Angle with a value specified in radians.
		/// </summary>
		/// <param name="radians">The angle, in radians.</param>
		/// <returns>the created GameMaker.Angle</returns>
		public static Angle Rad(double radians)
		{
			return new Angle { Radians = radians };
		}

		/// <summary>
		/// Creates a GameMaker.Angle with a value specified in degrees.
		/// </summary>
		/// <param name="degrees">The angle, in degrees</param>
		/// <returns>the created GameMaker.Angle</returns>
		public static Angle Deg(double degrees)
		{
			return new Angle { Degrees = degrees };
		}

		/// <summary>
		/// Finds the direction of the vector from the origin to the specified (x, y) point.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		/// <returns>The direction of the vector from the origin to the specified point.</returns>
		public static Angle Direction(double x, double y)
		{
			return Angle.Rad(Math.Atan2(y, x));
		}

		/// <summary>
		/// Finds the direction of the vector from the point (x1,y1) to the point (x2,y2).
		/// </summary>
		/// <param name="x1">The x-coordinate of the first point.</param>
		/// <param name="y1">The y-coordinate of the first point.</param>
		/// <param name="x2">The x-coordinate of the second point.</param>
		/// <param name="y2">The y-coordinate of the second point.</param>
		/// <returns>The direction of the vector the first to the second point.</returns>
		public static Angle Direction(double x1, double y1, double x2, double y2)
		{
			return Angle.Rad(Math.Atan2(y2 - y1, x2 - x1));
		}

		/// <summary>
		/// Gets or sets the value of this angle, in radians.
		/// </summary>
		public double Radians
		{
			get { return _degrees * GMath.Tau / 360.0; }
			set { _degrees = value * 360.0 / GMath.Tau; }
		}

		/// <summary>
		/// Gets or sets the value of this angle, in degrees.
		/// </summary>
		public double Degrees
		{
			get { return _degrees; }
			set { _degrees = value; }
		}


		/// <summary>
		/// Computes the acute angle between the two angles. This value is always between 0° and 180°.
		/// </summary>
		/// <param name="other">The GameMaker.Angle to compute the acute angle with.</param>
		/// <returns>The acute angle between the two angles.</returns>
		public Angle Acute(Angle other)
		{
			double d = this._degrees - other._degrees;
			d = (d % 360 + 360) % 360;
			if (d > 180)
				d = 360 - d;
			return Angle.Deg(d);
		}


		/// <summary>
		/// Converts this GameMaker.Angle to a human-readable string, showing the value in degrees.
		/// </summary>
		/// <returns>A string that represents this GameMaker.Angle</returns>
		public override string ToString()
		{
			return "Angle " + _degrees.ToString();
		}

		/// <summary>
		/// Computes the sum of the two angles.
		/// </summary>
		/// <param name="left">The first GameMaker.Angle.</param>
		/// <param name="right">The second GameMaker.Angle.</param>
		/// <returns>The sum of the two angles.</returns>
		public static Angle operator +(Angle left, Angle right)
		{
			return Angle.Deg(left._degrees + right._degrees);
		}

		/// <summary>
		/// Computes the clockwise difference of the two angles. 
		/// </summary>
		/// <param name="left">The first GameMaker.Angle.</param>
		/// <param name="right">The second GameMaker.Angle.</param>
		/// <returns>The difference of the two angles.</returns>
		public static Angle operator -(Angle left, Angle right)
		{
			return Angle.Deg(left._degrees - right._degrees);
		}

		/// <summary>
		/// Scales the angle by a specified scalar.
		/// </summary>
		/// <param name="a">The GameMaker.Angle to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled angle.</returns>
		public static Angle operator *(Angle a, double d)
		{
			return Angle.Deg(a._degrees * d);
		}

	}
}
