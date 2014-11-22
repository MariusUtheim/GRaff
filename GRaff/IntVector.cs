using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	/// <summary>
	/// Represents a vector with integer coordinates.
	/// </summary>
	public struct IntVector
	{  /*C#6.0*/
		public IntVector(int x, int y)
			:this()
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Represents the vector [0, 0].
		/// </summary>
		public static readonly IntVector Zero = new IntVector();

		/// <summary>
		/// Gets the x component of this GRaff.IntVector.
		/// </summary>
		public int X { get; private set;  }

		/// <summary>
		/// Gets the y component of this GRaff.IntVector.
		/// </summary>
		public int Y { get; private set; }

		/// <summary>
		/// Converts this GRaff.IntVector to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GRaff.IntVector</returns>
		public override string ToString() { return String.Format("[{0}, {1}]", X, Y); }

		/// <summary>
		/// Specifies whether this GRaff.IntVector contains the same coordinates as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.IntVector and has the same coordinates as this GRaff.IntVector.</returns>
		public override bool Equals(object obj) { return ((obj is IntVector) ? (this == (IntVector)obj) : base.Equals(obj)); }

		/// <summary>
		/// Returns a hash code for this GRaff.IntVector.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.IntVector.</returns>
		public override int GetHashCode() { return X ^ Y; }

		/// <summary>
		/// Compares two GRaff.IntVector objects. The result specifies whether their x- and y-coordinates are equal.
		/// </summary>
		/// <param name="left">The first GRaff.IntVector to compare.</param>
		/// <param name="right">The second GRaff.IntVector to compare.</param>
		/// <returns>true if the x- and y-coordinates of the two GRaff.IntVector structures are equal.</returns>
		public static bool operator ==(IntVector left, IntVector right) { return (left.X == right.X && left.Y == right.Y); }

		/// <summary>
		/// Compares two GRaff.IntVector objects. The result specifies whether their x- and y-coordinates are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.IntVector to compare.</param>
		/// <param name="right">The second GRaff.IntVector to compare.</param>
		/// <returns>true if the x- and y-coordinates of the two GRaff.IntVector structures are unequal.</returns>
		public static bool operator !=(IntVector left, IntVector right) { return (left.X != right.X || left.Y != right.Y); }


		/// <summary>
		/// Computes the sum of the two vectors.
		/// </summary>
		/// <param name="left">The first GRaff.IntVector.</param>
		/// <param name="right">The second GRaff.IntVector.</param>
		/// <returns>The sum of the two vectors.</returns>
		public static IntVector operator +(IntVector left, IntVector right) { return new IntVector(left.X + right.X, left.Y + right.Y); }

		/// <summary>
		/// Computes the difference between the two vectors.
		/// </summary>
		/// <param name="left">The first GRaff.IntVector.</param>
		/// <param name="right">The second GRaff.IntVector.</param>
		/// <returns>The difference of the two vectors.</returns>
		public static IntVector operator -(IntVector left, IntVector right) { return new IntVector(left.X - right.X, left.Y - right.Y); }

		/// <summary>
		/// Scales the vector by multiplying each component by the scalar.
		/// </summary>
		/// <param name="v">The GRaff.IntVector to scale.</param>
		/// <param name="i">The int to scale by.</param>
		/// <returns>The scaled GRaff.IntVector.</returns>
		public static IntVector operator *(IntVector v, int i) { return new IntVector(v.X * i, v.Y * i); }
		
		/// <summary>
		/// Scales the vector by dividing each component by the integer. The result is rounded down like common int division.
		/// </summary>
		/// <param name="v">The GRaff.IntVector to scale.</param>
		/// <param name="i">The int to scale by.</param>
		/// <returns>The scaled GRaff.IntVector.</returns>
		public static IntVector operator /(IntVector v, int i) { return new IntVector(v.X / i, v.Y / i); }

		/// <summary>
		/// Negates the vector by negating each component.
		/// </summary>
		/// <param name="v">The GRaff.IntVector to negate.</param>
		/// <returns>The negative of this vector.</returns>
		public static IntVector operator -(IntVector v) { return new IntVector(-v.X, -v.Y); }

		/// <summary>
		/// Converts the specified GRaff.IntVector to a GRaff.Point.
		/// </summary>
		/// <param name="i">The GRaff.IntVector to be converted.</param>
		/// <returns>The GRaff.Point that results from the conversion.</returns>
		public static implicit operator Point(IntVector v) { return new Point(v.X, v.Y); }

		/// <summary>
		/// Converts the specified GRaff.IntVector to a GRaff.Vector.
		/// </summary>
		/// <param name="i">The GRaff.IntVector to be converted.</param>
		/// <returns>The GRaff.Vector that results from the conversion.</returns>
		public static implicit operator Vector(IntVector i) { return new Vector(i.X, i.Y); }


	}
}
