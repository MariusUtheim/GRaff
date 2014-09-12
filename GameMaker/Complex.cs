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
	public struct Complex
	{
		private double _r, _i;

		/// <summary>
		/// Creates a new instance of the GameMaker.Complex class, using the specified real- and imaginary parts.
		/// Purely real complex numbers can be implicitly converted from System.Double.
		/// </summary>
		/// <param name="r">The real part.</param>
		/// <param name="i">The imaginary part.</param>
		public Complex(double r, double i)
		{
			this._r = r;
			this._i = i;
		}

		/// <summary>
		/// Creates a new instance of the GameMaker.Complex class, using the value specified in polar form.
		/// </summary>
		/// <param name="magnitude">The magnitude of the number.</param>
		/// <param name="argument">The argument of the number.</param>
		/// <returns></returns>
		public Complex(double magnitude, Angle argument)
		{
			this._r = magnitude * GMath.Cos(argument);
			this._i = magnitude * GMath.Sin(argument);
		}

		/// <summary>
		/// Gets or sets the real part of this GameMaker.Complex instance.
		/// </summary>
		public double Real
		{
			get { return _r; }
			set { _r = value; }
		}

		/// <summary>
		/// Gets or sets the imaginary part of this GameMaker.Complex instance.
		/// </summary>
		public double Imaginary
		{
			get { return _i; }
			set { _i = value; }
		}

#warning TODO: ToString, and equality operators

		/// <summary>
		/// Performs complex addition of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex sum of the two numbers.</returns>
		public static Complex operator +(Complex left, Complex right)
		{
			return new Complex(left._r + right._r, left._i + right._i);
		}

		/// <summary>
		/// Performs complex subtraction of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex difference of the two numbers.</returns>
		public static Complex operator -(Complex left, Complex right)
		{
			return new Complex(left._r - right._r, left._i - right._i);
		}

		/// <summary>
		/// Performs complex multiplication of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex product of the two numbers.</returns>
		public static Complex operator *(Complex left, Complex right)
		{
			return new Complex(left._r * right._r - left._i * right._i, left._r * right._i + left._i * right._r);
		}

		/// <summary>
		/// Performs complex division of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex ratio of the two numbers.</returns>
		public static Complex operator /(Complex left, Complex right)
		{
			double m = 1 / (right._r * right._r + right._i * right._i);
			return new Complex(m * (left._r * right._r + left._i * right._i), m * (left._i * right._r - left._r * right._i));
		}

		/// <summary>
		/// Converts the specified double to a purely real complex number.
		/// </summary>
		/// <param name="d">A real number.</param>
		/// <returns>The GameMaker.Complex that results from the conversion.</returns>
		public static implicit operator Complex(double d)
		{
			return new Complex(d, 0);
		}
	}
}
