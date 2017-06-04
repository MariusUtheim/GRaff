using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRaff.Geometry
{
	/// <summary>
	/// Represents a two dimensional vector.
	/// <remarks>
	/// GRaff.Vector is similar to GRaff.Point, but the direction is conserved even if magnitude is set to zero.
	/// </remarks>
	/// </summary>
	public struct Vector : IEquatable<Vector>
	{
		/// <summary>
		/// Represents the vector [0, 0].
		/// </summary>
		public static Vector Zero { get; } = new Vector();

		/// <summary>
		/// Initializes a new instance of the GRaff.Vector class using the components specified as cartesian coordinates.
		/// </summary>
		/// <param name="x">The x-component.</param>
		/// <param name="y">The y-component.</param>
		public Vector(double x, double y) 
			: this()
		{
			X = x;
			Y = y;
		}


		public Vector(double magnitude, Angle direction)
			: this()
		{
			X = magnitude * GMath.Cos(direction);
			Y = magnitude * GMath.Sin(direction);
		}

		/// <summary>
		/// Gets the x-component of this GRaff.Vector.
		/// </summary>
		public double X { get; private set; }

		/// <summary>
		/// Gets the y-component of this GRaff.Vector.
		/// </summary>
		public double Y { get; private set; }


		/// <summary>
		/// Gets the magnitude of this GRaff.Vector.
		/// </summary>
		public double Magnitude => GMath.Sqrt(X * X + Y * Y);

		/// <summary>
		/// Gets the direction of this GRaff.Vector.
		/// </summary>
		public Angle Direction => GMath.Atan2(Y, X); 


		/// <summary>
		/// Gets the unit vector pointing in the same direction as this GRaff.Vector.
		/// </summary>
		public Vector UnitVector => new Vector(1, this.Direction);


		/// <summary>
		/// Computes the dot product of this and the specified GRaff.Vector.
		/// </summary>
		/// <param name="other">The other GRaff.Vector.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public double DotProduct(Vector other)
			=> X * other.X + Y * other.Y;


		/// <summary>
		/// Converts this GRaff.Vector to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GRaff.Vector.</returns>
		public override string ToString() => $"[{X}, {Y}]";

		public bool Equals(Vector other)
			=> X == other.X && Y == other.Y;

		/// <summary>
		/// Specifies whether this GRaff.Vector contains the same coordinates as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.Vector and has the same coordinates as this GRaff.Vector.</returns>
		public override bool Equals(object obj)
			=> (obj is Vector) ? Equals((Vector)obj) : base.Equals(obj);

		/// <summary>
		/// Returns a hash code for this GRaff.Vector.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.Vector.</returns>
		public override int GetHashCode()
			=> GMath.HashCombine(X.GetHashCode(), Y.GetHashCode());

		/// <summary>
		/// Compares two GRaff.Vector objects. The result specifies whether their magnitude and direction are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Vector to compare.</param>
		/// <param name="right">The second GRaff.Vector to compare.</param>
		/// <returns>true if the magnitudes and the directions of the two GRaff.Vector structures are equal.</returns>
		public static bool operator ==(Vector left, Vector right) => left.Equals(right);

		/// <summary>
		/// Compares two GRaff.Vector objects. The result specifies whether their magnitude and direction are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.Vector to compare.</param>
		/// <param name="right">The second GRaff.Vector to compare.</param>
		/// <returns>true if the magnitudes and the directions of the two GRaff.Vector structures are unequal.</returns>
		public static bool operator !=(Vector left, Vector right) => !left.Equals(right);


		/// <summary>
		/// Computes the sum of the two GRaff.Vector structures.
		/// </summary>
		/// <param name="left">The first GRaff.Vector.</param>
		/// <param name="right">The second GRaff.Vector.</param>
		/// <returns>The sum of the two GRaff.Vector structures.</returns>
		public static Vector operator +(Vector left, Vector right) 
			=> new Vector(left.X + right.X, left.Y + right.Y);


		/// <summary>
		/// Computes the difference of the two GRaff.Vector structures.
		/// </summary>
		/// <param name="left">The first GRaff.Vector.</param>
		/// <param name="right">The second GRaff.Vector.</param>
		/// <returns>The difference of the two GRaff.Vector structures.</returns>
		public static Vector operator -(Vector left, Vector right) 
			=> new Vector(left.X - right.X, left.Y - right.Y);

		/// <summary>
		/// Reverses the direction of the specified GRaff.Vector structure.
		/// </summary>
		/// <param name="v">The GRaff.Vector</param>
		/// <returns>A GRaff.Vector structure with the same magnitude and opposite direction of the specified GRaff.Vector.</returns>
		public static Vector operator -(Vector v) 
			=> new Vector(-v.X, -v.Y);

		public static Vector operator *(Vector left, Vector right) 
			=> new Vector(left.X * right.X, left.Y * right.Y);

		/// <summary>
		/// Scales the GRaff.Vector by a specified scalar.
		/// </summary>
		/// <param name="v">The GRaff.Vector to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled GRaff.Vector.</returns>
		public static Vector operator *(Vector v, double d) 
			=> new Vector(v.X * d, v.Y * d);


		/// <summary>
		/// Scales the GRaff.Vector by a specified scalar.
		/// </summary>
		/// <param name="d">The double to scale by.</param>
		/// <param name="v">The GRaff.Vector to be scaled.</param>
		/// <returns>The scaled GRaff.Vector.</returns>
		public static Vector operator *(double d, Vector v) 
			=> new Vector(d * v.X, d * v.Y);


		/// <summary>
		/// Scales the GRaff.Vector by dividing by a specified scalar.
		/// </summary>
		/// <param name="v">The GRaff.Vector to be scaled.</param>
		/// <param name="d">The double to scale by.</param>
		/// <returns>The scaled GRaff.Vector.</returns>
		public static Vector operator /(Vector v, double d) 
			=> new Vector(v.X / d, v.Y / d);


		/// <summary>
		/// Rotates the GRaff.Vector by adding an GRaff.Angle to its direction.
		/// </summary>
		/// <param name="v">The GRaff.Vector to be rotated.</param>
		/// <param name="a">The GRaff.Angle to be added.</param>
		/// <returns>The rotated GRaff.Vector.</returns>
		public static Vector operator +(Vector v, Angle a) 
			=> new Vector(v.Magnitude, v.Direction + a);


		/// <summary>
		/// Rotates the GRaff.Vector by subtracting a GRaff.Angle from its direction.
		/// </summary>
		/// <param name="v">The GRaff.Vector to be rotated.</param>
		/// <param name="a">The GRaff.Angle to be subtracted.</param>
		/// <returns>The rotated GRaff.Vector.</returns>
		public static Vector operator -(Vector v, Angle a) 
			=> new Vector(v.Magnitude, v.Direction - a);


		/// <summary>
		/// Converts the specified GRaff.Vector to a GRaff.Point.
		/// </summary>
		/// <param name="v">The GRaff.Vector to be converted.</param>
		/// <returns>The GRaff.Point that results from the conversion.</returns>
		public static explicit operator Point(Vector v) 
			=> new Point(v.X, v.Y);


        public static implicit operator Vector((double x, double y) v)
            => new Vector(v.x, v.y);

        public void Deconstruct(out double x, out double y)
        {
            x = this.X;
            y = this.Y;
        }
	}
}
