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
		public double Real = real;

		/// <summary>
		/// Gets or sets the imaginary part of this GameMaker.Complex instance.
		/// </summary>
		public double Imaginary = imaginary;

#warning TODO: Documentation

		public override string ToString() => String.Format("{0} + {1}i", Real, Imaginary);


		public override bool Equals(object obj) => (obj is Complex) ? (this == (Complex)obj) : base.Equals(obj);


		public override int GetHashCode() => Real.GetHashCode() ^ Imaginary.GetHashCode();


		public static bool operator ==(Complex left, Complex right) => (left.Real == right.Real && left.Imaginary == right.Imaginary);


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
