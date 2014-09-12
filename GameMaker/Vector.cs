using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	/// <summary>
	/// Represents a two dimensional vector. 
	/// <remarks>
	/// GameMaker.Vector is similar to GameMaker.Point, but this struct stores its structure internally
	/// in polar coordinates (as opposed to cartesian coordinates used by GameMaker.Point).
	/// This way, the class conserves direction even when the magnitude is set to zero.
	/// </remarks>
	/// </summary>
	public struct Vector
	{

		/// <summary>
		/// Initializes a new instance of the GameMaker.Vector class using the components specified as cartesian coordinates.
		/// </summary>
		/// <param name="x">The x-component.</param>
		/// <param name="y">The y-component.</param>
		public Vector(double x, double y)
			: this()
		{
			_setCartesian(x, y);
		}

		/// <summary>
		/// Initializes a new instance of the GameMaker.Vector class using the components specified as polar coordinates.
		/// </summary>
		/// <param name="magnitude">The magnitude.</param>
		/// <param name="direction">The direction.</param>
		public Vector(double magnitude, Angle direction)
			: this()
		{
			this.Magnitude = magnitude;
			this.Direction = direction;
		}

		/// <summary>
		/// Gets or sets the magnitude of this GameMaker.Vector.
		/// </summary>
		public double Magnitude { get; set; }

		/// <summary>
		/// Gets or sets the direction of this GameMaker.Vector.
		/// </summary>
		public Angle Direction { get; set; }

		/// <summary>
		/// Gets or sets the x-component of this GameMaker.Vector.
		/// </summary>
		public double X
		{
			get { return Magnitude * GMath.Cos(Direction); }
			set { _setCartesian(value, Y); }
		}

		/// <summary>
		/// Gets or sets the y-component of this GameMaker.Vector.
		/// </summary>
		public double Y
		{
			get { return Magnitude * GMath.Sin(Direction); }
			set { _setCartesian(X, value); }
		}

		private void _setCartesian(double x, double y)
		{
			Magnitude = Math.Sqrt(x * x + y * y);
			if (x != 0 || y != 0)
				Direction = Angle.Direction(x, y);
		}

		/// <summary>
		/// Gets the normal vector pointing in the same direction as this GameMaker.Vector.
		/// </summary>
		public Vector Normal
		{
			get { return new Vector(1, this.Direction); }
		}

		/// <summary>
		/// Computes the dot product of this and the specified GameMaker.Vector.
		/// </summary>
		/// <param name="other">The other GameMaker.Vector.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public double DotProduct(Vector other)
		{
			return Magnitude * other.Magnitude * Math.Cos((Direction - other.Direction).Radians);
		}

		/// <summary>
		/// Converts this GameMaker.Vector to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GameMaker.Vector.</returns>
		public override string ToString()
		{
			return String.Format("[{0}, {1}]", X, Y);
		}

#warning TODO: Equality

		/// <summary>
		/// Computes the sum of the two GameMaker.Vector structures.
		/// </summary>
		/// <param name="left">The first GameMaker.Vector.</param>
		/// <param name="right">The second GameMaker.Vector.</param>
		/// <returns>The sum of the two GameMaker.Vector structures.</returns>
		public static Vector operator +(Vector left, Vector right) { return new Vector(left.X + right.X, left.Y + right.Y); }

		/// <summary>
		/// Comptues the difference of the two GameMaker.Vector structures.
		/// </summary>
		/// <param name="left">The first GameMaker.Vector.</param>
		/// <param name="right">The second GameMaker.Vector.</param>
		/// <returns>The difference of the two GameMaker.Vector structures.</returns>
		public static Vector operator -(Vector left, Vector right) { return new Vector(left.X - right.X, left.Y - right.Y); }

		/// <summary>
		/// Scales the GameMaker.Vector by a specified scalar.
		/// </summary>
		/// <param name="v">The GameMaker.Vector to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled GameMaker.Vector.</returns>
		public static Vector operator *(Vector v, double d) { return new Vector(v.Magnitude * d, v.Direction); }

		/// <summary>
		/// Scales the GameMaker.Vector by a specified scalar.
		/// </summary>
		/// <param name="d">The double to scale by.</param>
		/// <param name="v">The GameMaker.Vector to be scaled.</param>
		/// <returns>The scaled GameMaker.Vector.</returns>
		public static Vector operator *(double d, Vector v) { return new Vector(d * v.Magnitude, v.Direction); }

		/// <summary>
		/// Scales the GameMaker.Vector by dividing by a specified scalar.
		/// </summary>
		/// <param name="v">The GameMaker.Vector to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled GameMaker.Vector.</returns>
		public static Vector operator /(Vector v, double d) { return new Vector(v.Magnitude / d, v.Direction); }

		/// <summary>
		/// Rotates the GameMaker.Vector by adding an GameMaker.Angle to its direction.
		/// </summary>
		/// <param name="v">The GameMaker.Vector to be rotated.</param>
		/// <param name="a">The GameMaker.Angle to be added.</param>
		/// <returns>The rotated GameMaker.Vector.</returns>
		public static Vector operator +(Vector v, Angle a) { return new Vector(v.Magnitude, v.Direction + a); }

		/// <summary>
		/// Rotates the GameMaker.Vector by subtracting a GameMaker.Angle from its direction.
		/// </summary>
		/// <param name="v">The GameMaker.Vector to be rotated.</param>
		/// <param name="a">The GameMaker.Angle to be subtracted.</param>
		/// <returns>The rotated GameMaker.Vector.</returns>
		public static Vector operator -(Vector v, Angle a) { return new Vector(v.Magnitude, v.Direction - a); }

		/// <summary>
		/// Converts the specified GameMaker.Vector to a GameMaker.Point.
		/// </summary>
		/// <param name="v">The GameMaker.Vector to be converted.</param>
		/// <returns>The GameMaker.Point that results from the conversion.</returns>
		public static explicit operator Point(Vector v) { return new Point(v.X, v.Y); }
	}
}
