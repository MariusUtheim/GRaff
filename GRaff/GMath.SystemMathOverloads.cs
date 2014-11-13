using System;


namespace GRaff
{
	public static partial class GMath
	{
		public static double Abs(Complex value) { return value.Imaginary * value.Imaginary + value.Real * value.Real; }

		public static Angle Acos(double d)
		{
			return Angle.Rad(Math.Acos(d));
		}

		public static Angle Asin(double d)
		{
			return Angle.Rad(Math.Asin(d));
		}

		public static double Arg(Complex z)
		{
			return (z.Imaginary >= 0) ? Math.Atan2(z.Imaginary, z.Real) : -Math.Atan2(-z.Imaginary, z.Real);
		}

		public static Angle Atan2(Vector v)
		{
			return Angle.Rad(Math.Atan2(v.Y, v.X));
		}

		public static Complex Ceiling(Complex c) { return new Complex(Ceiling(c.Real), Ceiling(c.Imaginary)); }

		public static Complex Cosh(Complex c) { return new Complex(Cosh(c.Real) * Cos(c.Imaginary), Sinh(c.Real) * Sin(c.Imaginary)); }

		public static Complex Exp(Complex z)
		{
			return Math.Exp(z.Real) * new Complex(Math.Cos(z.Imaginary), Math.Sin(z.Imaginary));
		}

		public static Complex Floor(Complex c) { return new Complex(Floor(c.Real), Floor(c.Imaginary)); }

		public static Complex Log(Complex z)
		{
			if (z.Real == 0 && z.Imaginary == 0)
				return new Complex(double.NegativeInfinity, 0);
			return new Complex(Log(z.Real * z.Real + z.Imaginary * z.Imaginary), Arg(z));
		}

		public static Complex Round(Complex c)
		{
			return new Complex(Round(c.Real), Round(c.Imaginary));
		}
		public static Complex Round(Complex c, int decimals)
		{
			return new Complex(Round(c.Real, decimals), Round(c.Imaginary, decimals));
		}
		public static Complex Round(Complex c, MidpointRounding mode)
		{
			return new Complex(Round(c.Real, mode), Round(c.Imaginary, mode));
		}
		public static Complex Round(Complex c, int decimals, MidpointRounding mode)
		{
			return new Complex(Round(c.Real, decimals, mode), Round(c.Imaginary, decimals, mode));
		}

		public static Complex Sin(Complex z)
		{
			return new Complex(Sin(z.Real) * Cosh(z.Imaginary), Cos(z.Real) * Sinh(z.Imaginary));
		}

		public static Complex Sinh(Complex z)
		{
			return new Complex(Sinh(z.Real) * Cos(z.Imaginary), Cosh(z.Real) * Sin(z.Imaginary));
		}

		public static Complex Tan(Complex z)
		{
			double k = 1 / (Cos(2 * z.Real) + Cosh(2 * z.Imaginary));
			return new Complex(k * Sin(2 * z.Real), k * Sinh(2 * z.Imaginary));
		}

		public static Complex Tanh(Complex z)
		{
			double h = 1 / (Cosh(2 * z.Real) + Cos(2 * z.Imaginary));
			return new Complex(h * Sinh(2 * z.Imaginary), h * Sin(2 * z.Imaginary));
		}
	}
}


