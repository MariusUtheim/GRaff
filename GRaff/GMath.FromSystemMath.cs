using System;
using System.Diagnostics.Contracts;

namespace GRaff
{
	public static partial class GMath
	{
		public const double Pi = 3.1415926535897932384626433832795;
		public const double E = 2.7182818284590452353602874713527;

		public static sbyte Abs(sbyte value) => Math.Abs(value);
		public static short Abs(short value) => Math.Abs(value);
		public static int Abs(int value) => Math.Abs(value);
		public static long Abs(long value) => Math.Abs(value);
		public static float Abs(float value) => Math.Abs(value);
		public static double Abs(double value) => Math.Abs(value);
		public static decimal Abs(decimal value) => Math.Abs(value);

		public static Angle Atan(double d) => Angle.Rad(Math.Atan(d));
		public static Angle Atan2(double y, double x) => Angle.Rad(Math.Atan2(y, x));

		public static long BigMul(int a, int b) => Math.BigMul(a, b);

		public static double Ceiling(double d)
		{
			Contract.Ensures(Contract.Result<double>() >= d);
			Contract.Assume(Math.Ceiling(d) >= d);
			return Math.Ceiling(d);
		}
		
		public static decimal Ceiling(decimal d) => Math.Ceiling(d);

		public static double Cos(double d) => Math.Cos(d);
		public static double Cos(Angle a) => Math.Cos(a.Radians);
		public static Complex Cos(Complex c) => new Complex(Cos(c.Real) * Cosh(c.Imaginary), -Sin(c.Real) * Sinh(c.Imaginary));

		public static double Cosh(double d) => Math.Cosh(d);
		public static double Cosh(Angle a) => Math.Cosh(a.Radians);

		public static int DivRem(int a, int b, out int remainder) => Math.DivRem(a, b, out remainder);
		public static long DivRem(long a, long b, out long remainder) => Math.DivRem(a, b, out remainder);

		public static double Exp(double d) => Math.Exp(d);

		public static double Floor(double d) => Math.Floor(d);
		public static decimal Floor(decimal d) => Math.Floor(d);

		public static double Log(double d) => Math.Log(d);
		public static double Log(double d, double newBase) => Math.Log(d, newBase);

		public static double Log10(double d) => Math.Log10(d);

		public static byte Max(byte val1, byte val2) => Math.Max(val1, val2);
		public static sbyte Max(sbyte val1, sbyte val2) => Math.Max(val1, val2);
		public static short Max(short val1, short val2) => Math.Max(val1, val2);
		public static ushort Max(ushort val1, ushort val2) => Math.Max(val1, val2);
		public static int Max(int val1, int val2) => Math.Max(val1, val2);
		public static uint Max(uint val1, uint val2) => Math.Max(val1, val2);
		public static long Max(long val1, long val2) => Math.Max(val1, val2);
		public static ulong Max(ulong val1, ulong val2) => Math.Max(val1, val2);
		public static float Max(float val1, float val2) => Math.Max(val1, val2);
		public static double Max(double val1, double val2) => Math.Max(val1, val2);
		public static decimal Max(decimal val1, decimal val2) => Math.Max(val1, val2);


		public static byte Min(byte val1, byte val2) => Math.Min(val1, val2);
		public static sbyte Min(sbyte val1, sbyte val2) => Math.Min(val1, val2);
		public static short Min(short val1, short val2) => Math.Min(val1, val2);
		public static ushort Min(ushort val1, ushort val2) => Math.Min(val1, val2);
		public static int Min(int val1, int val2) => Math.Min(val1, val2);
		public static uint Min(uint val1, uint val2) => Math.Min(val1, val2);
		public static long Min(long val1, long val2) => Math.Min(val1, val2);
		public static ulong Min(ulong val1, ulong val2) => Math.Min(val1, val2);
		public static float Min(float val1, float val2) => Math.Min(val1, val2);
		public static double Min(double val1, double val2) => Math.Min(val1, val2);
		public static decimal Min(decimal val1, decimal val2) => Math.Min(val1, val2);


		public static double Pow(double x, double y) => Math.Pow(x, y);

		public static double Round(double d) => Math.Round(d);
		public static double Round(double d, int decimals) => Math.Round(d, decimals);
		public static double Round(double d, MidpointRounding mode) => Math.Round(d, mode);
		public static double Round(double d, int decimals, MidpointRounding mode) => Math.Round(d, decimals, mode);
		public static decimal Round(decimal d) => Math.Round(d);
		public static decimal Round(decimal d, int decimals) => Math.Round(d, decimals);
		public static decimal Round(decimal d, MidpointRounding mode) => Math.Round(d, mode);
		public static decimal Round(decimal d, int decimals, MidpointRounding mode) => Math.Round(d, decimals, mode);

		public static int Sign(sbyte value) => Math.Sign(value);
		public static int Sign(short value) => Math.Sign(value);
		public static int Sign(int value) => Math.Sign(value);
		public static int Sign(long value) => Math.Sign(value);
		public static int Sign(float value) => Math.Sign(value);
		public static int Sign(double value) => Math.Sign(value);
		public static int Sign(decimal value) => Math.Sign(value);

		public static double Sin(double a) => Math.Sin(a);
		public static double Sin(Angle a) => Math.Sin(a.Radians);

		public static double Sinh(double a) => Math.Sinh(a);
		public static double Sinh(Angle a) => Math.Sinh(a.Radians);

		public static double Sqrt(double d) => Math.Sqrt(d);

		public static double Tan(double a) => Math.Tan(a);
		public static double Tan(Angle a) => Math.Tan(a.Radians);

		public static double Tanh(double a) => Math.Tanh(a);
		public static double Tanh(Angle a) => Math.Tanh(a.Radians);

	}
}
