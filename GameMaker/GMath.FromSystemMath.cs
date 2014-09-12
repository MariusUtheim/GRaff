using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static partial class GMath
	{
		public const double Pi = 3.1415926535897932384626433832795;
		public const double E = 2.7182818284590452353602874713527;

		public static sbyte Abs(sbyte value)
		{
			return Math.Abs(value);
		}
		public static short Abs(short value)
		{
			return Math.Abs(value);
		}
		public static int Abs(int value)
		{
			return Math.Abs(value);
		}
		public static long Abs(long value)
		{
			return Math.Abs(value);
		}
		public static float Abs(float value)
		{
			return Math.Abs(value);
		}
		public static double Abs(double value)
		{
			return Math.Abs(value);
		}
		public static decimal Abs(decimal value)
		{
			return Math.Abs(value);
		}

		public static Angle Acos(double d)
		{
			return Angle.Rad(Math.Acos(d));
		}

		public static Angle Asin(double d)
		{
			return Angle.Rad(Math.Asin(d));
		}

		public static Angle Atan(double d)
		{
			return Angle.Rad(Math.Atan(d));
		}

		public static Angle Atan2(double y, double x)
		{
			return Angle.Rad(Math.Atan2(y, x));
		}
		public static Angle Atan2(Vector v)
		{
			return Angle.Rad(Math.Atan2(v.Y, v.X));
		}

		public static long BigMul(int a, int b)
		{
			return Math.BigMul(a, b);
		}

		public static double Ceiling(double d)
		{
			return Math.Ceiling(d);
		}
		public static decimal Ceiling(decimal d)
		{
			return Math.Ceiling(d);
		}

		public static double Cos(double d)
		{
			return Math.Cos(d);
		}
		public static double Cos(Angle a)
		{
			return Math.Cos(a.Radians);
		}

		public static double Cosh(double d)
		{
			return Math.Cosh(d);
		}
		public static double Cosh(Angle a)
		{
			return Math.Cosh(a.Radians);
		}

		public static int DivRem(int a, int b, out int remainder)
		{
			return Math.DivRem(a, b, out remainder);
		}
		public static long DivRem(long a, long b, out long remainder)
		{
			return Math.DivRem(a, b, out remainder);
		}

		public static double Exp(double d)
		{
			return Math.Exp(d);
		}
		public static Complex Exp(Complex z)
		{
			return new Complex(Math.Exp(z.Real), Angle.Rad(z.Imaginary));
		}

		public static double Floor(double d)
		{
			return Math.Floor(d);
		}
		public static decimal Floor(decimal d)
		{
			return Math.Floor(d);
		}

		public static double Log(double d)
		{
			return Math.Log(d);
		}
		public static double Log(double d, double newBase)
		{
			return Math.Log(d, newBase);
		}

		public static double Log10(double d)
		{
			return Math.Log10(d);
		}

		public static byte Max(byte val1, byte val2)
		{
			return Math.Max(val1, val2);
		}
		public static sbyte Max(sbyte val1, sbyte val2)
		{
			return Math.Max(val1, val2);
		}
		public static short Max(short val1, short val2)
		{
			return Math.Max(val1, val2);
		}
		public static ushort Max(ushort val1, ushort val2)
		{
			return Math.Max(val1, val2);
		}
		public static int Max(int val1, int val2)
		{
			return Math.Max(val1, val2);
		}
		public static uint Max(uint val1, uint val2)
		{
			return Math.Max(val1, val2);
		}
		public static long Max(long val1, long val2)
		{
			return Math.Max(val1, val2);
		}
		public static ulong Max(ulong val1, ulong val2)
		{
			return Math.Max(val1, val2);
		}
		public static float Max(float val1, float val2)
		{
			return Math.Max(val1, val2);
		}
		public static double Max(double val1, double val2)
		{
			return Math.Max(val1, val2);
		}
		public static decimal Max(decimal val1, decimal val2)
		{
			return Math.Max(val1, val2);
		}


		public static byte Min(byte val1, byte val2)
		{
			return Math.Min(val1, val2);
		}
		public static sbyte Min(sbyte val1, sbyte val2)
		{
			return Math.Min(val1, val2);
		}
		public static short Min(short val1, short val2)
		{
			return Math.Min(val1, val2);
		}
		public static ushort Min(ushort val1, ushort val2)
		{
			return Math.Min(val1, val2);
		}
		public static int Min(int val1, int val2)
		{
			return Math.Min(val1, val2);
		}
		public static uint Min(uint val1, uint val2)
		{
			return Math.Min(val1, val2);
		}
		public static long Min(long val1, long val2)
		{
			return Math.Min(val1, val2);
		}
		public static ulong Min(ulong val1, ulong val2)
		{
			return Math.Min(val1, val2);
		}
		public static float Min(float val1, float val2)
		{
			return Math.Min(val1, val2);
		}
		public static double Min(double val1, double val2)
		{
			return Math.Min(val1, val2);
		}
		public static decimal Min(decimal val1, decimal val2)
		{
			return Math.Min(val1, val2);
		}


		public static double Pow(double x, double y)
		{
			return Math.Pow(x, y);
		}

		public static double Round(double d)
		{
			return Math.Round(d);
		}
		public static double Round(double d, int decimals)
		{
			return Math.Round(d, decimals);
		}
		public static double Round(double d, MidpointRounding mode)
		{
			return Math.Round(d, mode);
		}
		public static double Round(double d, int decimals, MidpointRounding mode)
		{
			return Math.Round(d, decimals, mode);
		}
		public static decimal Round(decimal d)
		{
			return Math.Round(d);
		}
		public static decimal Round(decimal d, int decimals)
		{
			return Math.Round(d, decimals);
		}
		public static decimal Round(decimal d, MidpointRounding mode)
		{
			return Math.Round(d, mode);
		}
		public static decimal Round(decimal d, int decimals, MidpointRounding mode)
		{
			return Math.Round(d, decimals, mode);
		}

		public static int Sign(sbyte value)
		{
			return Math.Sign(value);
		}
		public static int Sign(short value)
		{
			return Math.Sign(value);
		}
		public static int Sign(int value)
		{
			return Math.Sign(value);
		}
		public static int Sign(long value)
		{
			return Math.Sign(value);
		}
		public static int Sign(float value)
		{
			return Math.Sign(value);
		}
		public static int Sign(double value)
		{
			return Math.Sign(value);
		}
		public static int Sign(decimal value)
		{
			return Math.Sign(value);
		}

		public static double Sin(double a)
		{
			return Math.Sin(a);
		}
		public static double Sin(Angle a)
		{
			return Math.Sin(a.Radians);
		}

		public static double Sinh(double a)
		{
			return Math.Sinh(a);
		}
		public static double Sinh(Angle a)
		{
			return Math.Sinh(a.Radians);
		}

		public static double Sqrt(double d)
		{
			return Math.Sqrt(d);
		}

		public static double Tan(double a)
		{
			return Math.Tan(a);
		}
		public static double Tan(Angle a)
		{
			return Math.Tan(a.Radians);
		}

		public static double Tanh(double a)
		{
			return Math.Tanh(a);
		}
		public static double Tanh(Angle a)
		{
			return Math.Tanh(a.Radians);
		}

	}
}
