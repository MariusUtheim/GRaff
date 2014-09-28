using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	/// <summary>
	/// Represents a complex number.
	/// </summary>
	public struct Complex(double real, double imaginary)
	{
		/// <summary>
		/// Gets or sets the real part of this GameMaker.Complex instance.
		/// </summary>
		public double Real { get; set; } = real;

		/// <summary>
		/// Gets or sets the imaginary part of this GameMaker.Complex instance.
		/// </summary>
		public double Imaginary { get; set; } = imaginary;

		/// <summary>
		/// Converts this GameMaker.Complex to a human-readable string, showing the value in Cartesian form x + yi.
		/// </summary>
		/// <returns>A string that represents this GameMaker.Complex</returns>
		public override string ToString()
		{
			if (Imaginary == 0)
				return Real.ToString();
			else if (Real == 0)
				return Imaginary.ToString() + "i";
			else if (Imaginary > 0)
				return String.Format("{0} + {1}i", Real, Imaginary);
			else
				return String.Format("{0} - {1}i", Real, -Imaginary);
		}

		/// <summary>
		/// Specifies whether this GameMaker.Complex is equal to the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GameMaker.Complex and the two complex numbers are equal.</returns>
		public override bool Equals(object obj) => (obj is Complex) ? (this == (Complex)obj) : base.Equals(obj);

		/// <summary>
		/// Returns a hash code for this GameMaker.Complex.
		/// </summary>
		/// <returns>An integer value that specified a hash value for this GameMaker.Complex.</returns>
		public override int GetHashCode() => Real.GetHashCode() ^ Imaginary.GetHashCode();

		/// <summary>
		/// Compares two GameMaker.Complex objects. The results specifies whether the two complex numbers are equal.
		/// </summary>
		/// <param name="left">The first GameMaker.Complex to compare.</param>
		/// <param name="right">The second GameMaker.Complex to compare.</param>
		/// <returns>true if the two complex numbers are equal.</returns>
		public static bool operator ==(Complex left, Complex right) => (left.Real == right.Real && left.Imaginary == right.Imaginary);

		/// <summary>
		/// Compares two GameMaker.Complex objects. The result specifies whether the two complex numbers are unequal. 
		/// </summary>
		/// <param name="left">The first GameMaker.Complex to compare.</param>
		/// <param name="right">The second GameMaker.Complex to compare.</param>
		/// <returns>true if the two complex numbers are unequal.</returns>
		public static bool operator !=(Complex left, Complex right) => (left.Real != right.Real || left.Imaginary != right.Imaginary);


		/// <summary>
		/// Performs complex addition of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex sum of the two numbers.</returns>
		public static Complex operator +(Complex left, Complex right) => new Complex(left.Real + right.Real, left.Imaginary + right.Imaginary);


		/// <summary>
		/// Performs complex subtraction of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex difference of the two numbers.</returns>
		public static Complex operator -(Complex left, Complex right) => new Complex(left.Real - right.Real, left.Imaginary - right.Imaginary);


		/// <summary>
		/// Performs complex multiplication of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex product of the two numbers.</returns>
		public static Complex operator *(Complex left, Complex right) => new Complex(left.Real * right.Real - left.Imaginary * right.Imaginary, left.Real * right.Imaginary + left.Imaginary * right.Real);
		

		/// <summary>
		/// Performs complex division of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex ratio of the two numbers.</returns>
		public static Complex operator /(Complex left, Complex right)
		{
			if (right == 0)	throw new DivideByZeroException();
			double m = 1 / (right.Real * right.Real + right.Imaginary * right.Imaginary);
			return new Complex(m * (left.Real * right.Real + left.Imaginary * right.Imaginary), m * (left.Imaginary * right.Real - left.Real * right.Imaginary));
		}

		/// <summary>
		/// Converts the specified double to a purely real complex number.
		/// </summary>
		/// <param name="d">A real number.</param>
		/// <returns>The GameMaker.Complex that results from the conversion.</returns>
		public static implicit operator Complex(double d) => new Complex(d, 0);
	}
}
