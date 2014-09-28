﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	/// <summary>
	/// Represents a vector with integer coordinates.
	/// </summary>
	public struct IntVector(int x, int y)
	{
		/// <summary>
		/// Represents the vector [0, 0].
		/// </summary>
		public static readonly IntVector Zero = new IntVector();

		/// <summary>
		/// Gets the x component of this GameMaker.IntVector.
		/// </summary>
		public int X { get; } = x;

		/// <summary>
		/// Gets the y component of this GameMaker.IntVector.
		/// </summary>
		public int Y { get; } = y;

		/// <summary>
		/// Converts this GameMaker.IntVector to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GameMaker.IntVector</returns>
		public override string ToString() => String.Format("[{0}, {1}]", X, Y);

		/// <summary>
		/// Specifies whether this GameMaker.IntVector contains the same coordinates as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GameMaker.IntVector and has the same coordinates as this GameMaker.IntVector.</returns>
		public override bool Equals(object obj) => ((obj is IntVector) ? (this == (IntVector)obj) : base.Equals(obj));

		/// <summary>
		/// Returns a hash code for this GameMaker.IntVector.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GameMaker.IntVector.</returns>
		public override int GetHashCode() => X ^ Y;

		/// <summary>
		/// Compares two GameMaker.IntVector objects. The result specifies whether their x- and y-coordinates are equal.
		/// </summary>
		/// <param name="left">The first GameMaker.IntVector to compare.</param>
		/// <param name="right">The second GameMaker.IntVector to compare.</param>
		/// <returns>true if the x- and y-coordinates of the two GameMaker.IntVector structures are equal.</returns>
		public static bool operator ==(IntVector left, IntVector right) => (left.X == right.X && left.Y == right.Y);

		/// <summary>
		/// Compares two GameMaker.IntVector objects. The result specifies whether their x- and y-coordinates are unequal.
		/// </summary>
		/// <param name="left">The first GameMaker.IntVector to compare.</param>
		/// <param name="right">The second GameMaker.IntVector to compare.</param>
		/// <returns>true if the x- and y-coordinates of the two GameMaker.IntVector structures are unequal.</returns>
		public static bool operator !=(IntVector left, IntVector right) => (left.X != right.X || left.Y != right.Y);


		/// <summary>
		/// Computes the sum of the two vectors.
		/// </summary>
		/// <param name="left">The first GameMaker.IntVector.</param>
		/// <param name="right">The second GameMaker.IntVector.</param>
		/// <returns>The sum of the two vectors.</returns>
		public static IntVector operator +(IntVector left, IntVector right) => new IntVector(left.X + right.X, left.Y + right.Y);

		/// <summary>
		/// Computes the difference between the two vectors.
		/// </summary>
		/// <param name="left">The first GameMaker.IntVector.</param>
		/// <param name="right">The second GameMaker.IntVector.</param>
		/// <returns>The difference of the two vectors.</returns>
		public static IntVector operator -(IntVector left, IntVector right) => new IntVector(left.X - right.X, left.Y - right.Y);

		/// <summary>
		/// Scales the vector by multiplying each component by the scalar.
		/// </summary>
		/// <param name="v">The GameMaker.IntVector to scale.</param>
		/// <param name="i">The int to scale by.</param>
		/// <returns>The scaled GameMaker.IntVector.</returns>
		public static IntVector operator *(IntVector v, int i) => new IntVector(v.X * i, v.Y * i);
		
		/// <summary>
		/// Scales the vector by dividing each component by the integer. The result is rounded down like common int division.
		/// </summary>
		/// <param name="v">The GameMaker.IntVector to scale.</param>
		/// <param name="i">The int to scale by.</param>
		/// <returns>The scaled GameMaker.IntVector.</returns>
		public static IntVector operator /(IntVector v, int i) => new IntVector(v.X / i, v.Y / i);

		/// <summary>
		/// Negates the vector by negating each component.
		/// </summary>
		/// <param name="v">The GameMaker.IntVector to negate.</param>
		/// <returns>The negative of this vector.</returns>
		public static IntVector operator -(IntVector v) => new IntVector(-v.X, -v.Y);

		/// <summary>
		/// Converts the specified GameMaker.IntVector to a GameMaker.Point.
		/// </summary>
		/// <param name="i">The GameMaker.IntVector to be converted.</param>
		/// <returns>The GameMaker.Point that results from the conversion.</returns>
		public static implicit operator Point(IntVector v) => new Point(v.X, v.Y);

		/// <summary>
		/// Converts the specified GameMaker.IntVector to a GameMaker.Vector.
		/// </summary>
		/// <param name="i">The GameMaker.IntVector to be converted.</param>
		/// <returns>The GameMaker.Vector that results from the conversion.</returns>
		public static implicit operator Vector(IntVector i) => new Vector(i.X, i.Y);


	}
}
