using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	/// <summary>
	/// Represents a vector with integer coordinates.
	/// </summary>
	public struct IntVector
	{
		/// <summary>
		/// Represents the vector [0, 0].
		/// </summary>
		public static IntVector Zero = new IntVector();

		/// <summary>
		///  a new instance of the GameMaker.IntVector class, using the specified x and y components.
		/// </summary>
		/// <param name="x">The x component.</param>
		/// <param name="y">The y component.</param>
		public IntVector(int x, int y)
			: this()
		{
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Gets or sets the x component of this GameMaker.IntVector.
		/// </summary>
		public int X { get; set; }

		/// <summary>
		/// Gets or sets the y component of this GameMaker.IntVector.
		/// </summary>
		public int Y { get; set; }

		/// <summary>
		/// Converts the specified GameMaker.IntVector to a GameMaker.Vector.
		/// </summary>
		/// <param name="i">The GameMaker.IntVector to be converted.</param>
		/// <returns>The GameMaker.Vector that results from the conversion.</returns>
		public static implicit operator Vector(IntVector i) { return new Vector(i.X, i.Y); }

		/// <summary>
		/// Converts this GameMaker.IntRectangle to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GameMaker.IntRectangle</returns>
		public override string ToString()
		{
			return String.Format("[{0}, {1}]", X, Y);
		}

		public override bool Equals(object obj)
		{
			if (obj is IntVector)
				return this == (IntVector)obj;
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return X ^ Y;
		}

		public static bool operator ==(IntVector left, IntVector right) { return left.X == right.X && left.Y == right.Y; }

		public static bool operator !=(IntVector left, IntVector right) { return left.X != right.X || left.Y != right.Y; }

#warning TODO: Equality


		/// <summary>
		/// Computes the sum of the two vectors.
		/// </summary>
		/// <param name="left">The first GameMaker.IntVector.</param>
		/// <param name="right">The second GameMaker.IntVector.</param>
		/// <returns>The sum of the two vectors.</returns>
		public static IntVector operator +(IntVector left, IntVector right) { return new IntVector(left.X + right.X, left.Y + right.Y); }

		/// <summary>
		/// Computes the difference between the two vectors.
		/// </summary>
		/// <param name="left">The first GameMaker.IntVector.</param>
		/// <param name="right">The second GameMaker.IntVector.</param>
		/// <returns>The difference of the two vectors.</returns>
		public static IntVector operator -(IntVector left, IntVector right) { return new IntVector(left.X - right.X, left.Y - right.Y); }

		/// <summary>
		/// Scales the vector by multiplying each component by the scalar.
		/// </summary>
		/// <param name="v">The GameMaker.IntVector to scale.</param>
		/// <param name="i">The int to scale by.</param>
		/// <returns>The scaled GameMaker.IntVector.</returns>
		public static IntVector operator *(IntVector v, int i) { return new IntVector(v.X * i, v.Y * i); }
		
		/// <summary>
		/// Scales the vector by dividing each component by the integer. The result is rounded down like common int division.
		/// </summary>
		/// <param name="v">The GameMaker.IntVector to scale.</param>
		/// <param name="i">The int to scale by.</param>
		/// <returns>The scaled GameMaker.IntVector.</returns>
		public static IntVector operator /(IntVector v, int i) { return new IntVector(v.X / i, v.Y / i); }

		/// <summary>
		/// Negates the vector by negating each component.
		/// </summary>
		/// <param name="v">The GameMaker.IntVector to negate.</param>
		/// <returns>The negative of this vector.</returns>
		public static IntVector operator -(IntVector v) { return new IntVector(-v.X, -v.Y); }

		/// <summary>
		/// Converts the specified GameMaker.IntVector to a GameMaker.Point.
		/// </summary>
		/// <param name="i">The GameMaker.IntVector to be converted.</param>
		/// <returns>The GameMaker.Point that results from the conversion.</returns>
		public static implicit operator Point(IntVector v) { return new Point(v.X, v.Y); }

	}
}
