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
	public struct Vector(double magnitude, Angle direction)
	{
		{
			if (magnitude < 0)
			{
				magnitude = -magnitude;
				direction *= -1;
			}
		}

		/// <summary>
		/// Represents the vector [0, 0].
		/// </summary>
		public static Vector Zero = new Vector();

		/// <summary>
		/// Initializes a new instance of the GameMaker.Vector class using the components specified as cartesian coordinates.
		/// </summary>
		/// <param name="x">The x-component.</param>
		/// <param name="y">The y-component.</param>
		public Vector(double x, double y) : this(GMath.Sqrt(x * x + y * y), (x == 0 && y == 0) ? Angle.Zero : GMath.Atan2(y, x)) { }

		/// <summary>
		/// Gets the magnitude of this GameMaker.Vector.
		/// </summary>
		public double Magnitude { get; } = magnitude;

		/// <summary>
		/// Gets the direction of this GameMaker.Vector.
		/// </summary>
		public Angle Direction { get; } = direction;

		/// <summary>
		/// Gets the x-component of this GameMaker.Vector.
		/// </summary>
		public double X => Magnitude * GMath.Cos(Direction);

		/// <summary>
		/// Gets the y-component of this GameMaker.Vector.
		/// </summary>
		public double Y => Magnitude * GMath.Sin(Direction);


		/// <summary>
		/// Gets the normal vector pointing in the same direction as this GameMaker.Vector.
		/// </summary>
		public Vector Normal => new Vector(1, this.Direction);


		/// <summary>
		/// Computes the dot product of this and the specified GameMaker.Vector.
		/// </summary>
		/// <param name="other">The other GameMaker.Vector.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public double DotProduct(Vector other) => Magnitude * other.Magnitude * GMath.Cos(Direction - other.Direction);


		/// <summary>
		/// Converts this GameMaker.Vector to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GameMaker.Vector.</returns>
		public override string ToString() => String.Format("[{0}, {1}]", X, Y);

#warning TODO: Documentation
		public override bool Equals(object obj) => (obj is Vector) ? (this == (Vector)obj) : base.Equals(obj);

		public override int GetHashCode() => Magnitude.GetHashCode() ^ Direction.GetHashCode();

		public static bool operator ==(Vector left, Vector right) => (left.Magnitude == right.Magnitude && left.Direction == right.Direction);

		public static bool operator !=(Vector left, Vector right) => (left.Magnitude != right.Magnitude || left.Direction != right.Direction);


		/// <summary>
		/// Computes the sum of the two GameMaker.Vector structures.
		/// </summary>
		/// <param name="left">The first GameMaker.Vector.</param>
		/// <param name="right">The second GameMaker.Vector.</param>
		/// <returns>The sum of the two GameMaker.Vector structures.</returns>
		public static Vector operator +(Vector left, Vector right) => new Vector(left.X + right.X, left.Y + right.Y);


		/// <summary>
		/// Comptues the difference of the two GameMaker.Vector structures.
		/// </summary>
		/// <param name="left">The first GameMaker.Vector.</param>
		/// <param name="right">The second GameMaker.Vector.</param>
		/// <returns>The difference of the two GameMaker.Vector structures.</returns>
		public static Vector operator -(Vector left, Vector right) => new Vector(left.X - right.X, left.Y - right.Y);


		/// <summary>
		/// Scales the GameMaker.Vector by a specified scalar.
		/// </summary>
		/// <param name="v">The GameMaker.Vector to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled GameMaker.Vector.</returns>
		public static Vector operator *(Vector v, double d) => new Vector(v.Magnitude * d, v.Direction);


		/// <summary>
		/// Scales the GameMaker.Vector by a specified scalar.
		/// </summary>
		/// <param name="d">The double to scale by.</param>
		/// <param name="v">The GameMaker.Vector to be scaled.</param>
		/// <returns>The scaled GameMaker.Vector.</returns>
		public static Vector operator *(double d, Vector v) => new Vector(d * v.Magnitude, v.Direction);


		/// <summary>
		/// Scales the GameMaker.Vector by dividing by a specified scalar.
		/// </summary>
		/// <param name="v">The GameMaker.Vector to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled GameMaker.Vector.</returns>
		public static Vector operator /(Vector v, double d) => new Vector(v.Magnitude / d, v.Direction);


		/// <summary>
		/// Rotates the GameMaker.Vector by adding an GameMaker.Angle to its direction.
		/// </summary>
		/// <param name="v">The GameMaker.Vector to be rotated.</param>
		/// <param name="a">The GameMaker.Angle to be added.</param>
		/// <returns>The rotated GameMaker.Vector.</returns>
		public static Vector operator +(Vector v, Angle a) => new Vector(v.Magnitude, v.Direction + a);


		/// <summary>
		/// Rotates the GameMaker.Vector by subtracting a GameMaker.Angle from its direction.
		/// </summary>
		/// <param name="v">The GameMaker.Vector to be rotated.</param>
		/// <param name="a">The GameMaker.Angle to be subtracted.</param>
		/// <returns>The rotated GameMaker.Vector.</returns>
		public static Vector operator -(Vector v, Angle a) => new Vector(v.Magnitude, v.Direction - a);


		/// <summary>
		/// Converts the specified GameMaker.Vector to a GameMaker.Point.
		/// </summary>
		/// <param name="v">The GameMaker.Vector to be converted.</param>
		/// <returns>The GameMaker.Point that results from the conversion.</returns>
		public static explicit operator Point(Vector v) => new Point(v.X, v.Y);
	}
}
