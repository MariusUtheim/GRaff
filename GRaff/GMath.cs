using System;
using System.Diagnostics.Contracts;
using GRaff.Geometry;

namespace GRaff
{
	public static partial class GMath
	{
		public const double Tau = 6.283185307179586476925286766559;
		/// <summary>Conversion factor for degrees to radians. Is equal to τ / 360.</summary>
		public const double DegToRad = 0.01745329251994329576923690768489;
		/// <summary>Conversion factor for radians to degrees. Is equal to 360 / τ.</summary>
		public const double RadToDeg = 57.2957795130823208767981548141050;
		/// <summary>The golden ratio, φ</summary>
		public const double Phi = 1.6180339887498948482045868343656;

		public const float FloatEpsilon = 0.000000059604644775390625f;
		/// <summary>The smallest detectable relative rounding error in double-precision arithmetic.</summary>
		public const double MachineEpsilon = 2.2204460492503130808472633361816e-16;
		/// <summary>A default resolution for discerning the value of two numbers</summary>
		internal const double DefaultDelta = 1e-9;

		public static double Frac(double x) => (x >= 0) ? x - Floor(x) : x - Ceiling(x);

		public static int HashCombine(params int[] values)
		{
			if (values == null)
				return 0;
			unchecked
			{
				int result = 0;
				for (int i = 0; i < values.Length; i++)
					result = 37 * result + values[i];
				return result;
			}
		}

		public static byte Median(byte x1, byte x2, byte x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static sbyte Median(sbyte x1, sbyte x2, sbyte x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static short Median(short x1, short x2, short x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static ushort Median(ushort x1, ushort x2, ushort x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static int Median(int x1, int x2, int x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static uint Median(uint x1, uint x2, uint x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static long Median(long x1, long x2, long x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static ulong Median(ulong x1, ulong x2, ulong x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static float Median(float x1, float x2, float x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static double Median(double x1, double x2, double x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static decimal Median(decimal x1, decimal x2, decimal x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static Point Median(Point x1, Point x2, Point x3) => new Point(Median(x1.X, x2.X, x3.X), Median(x1.Y, x2.Y, x3.Y));
		public static Vector Median(Vector x1, Vector x2, Vector x3) => new Vector(Median(x1.X, x2.X, x3.X), Median(x1.Y, x2.Y, x3.Y));

		public static int Remainder(int x, int q)
		{
			Contract.Requires(q != 0);
			return ((x % q) + q) % q;
		}

		public static double Remainder(double x, double q) => ((x % q) + q) % q;

		public static int RoundInt(double x) => (int)Round(x);
		public static int RoundInt(float x) => (int)Round(x);
		public static int RoundInt(decimal x) => (int)Round(x);
		public static uint RoundUInt(double x) => (uint)Round(x);
		public static uint RoundUInt(float x) => (uint)Round(x);
		public static uint RoundUInt(decimal x) => (uint)Round(x);
		public static long RoundLong(double x) => (long)Round(x);
		public static long RoundLong(decimal x) => (long)Round(x);
		public static ulong RoundULong(double x) => (ulong)Round(x);
		public static ulong RoundULong(decimal x) => (ulong)Round(x);


		public static int Sqr(byte x) => x * x;
		public static int Sqr(sbyte x) => x * x;
		public static int Sqr(short x) => x * x;
		public static int Sqr(ushort x) => x * x;
		public static int Sqr(int x) => x * x;
		public static uint Sqr(uint x) => x * x;
		public static long Sqr(long x) => x * x;
		public static ulong Sqr(ulong x) => x * x;
		public static float Sqr(float x) => x * x;
		public static double Sqr(double x) => x * x;
		public static decimal Sqr(decimal x) => x * x;
	}
}


