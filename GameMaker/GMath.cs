using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static partial class GMath
	{
		public const double Tau = 6.283185307179586476925286766559;

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

		public static double DegToRad(double degrees)
		{
			return degrees * GMath.Tau / 360.0;
		}

		public static double RadToDeg(double radians)
		{
			return radians * 360.0 / GMath.Tau;
		}

		public static int Sqr(byte x)
		{
			return x * x;
		}
		public static int Sqr(sbyte x)
		{
			return x * x;
		}
		public static int Sqr(short x)
		{
			return x * x;
		}
		public static int Sqr(ushort x)
		{
			return x * x;
		}
		public static int Sqr(int x)
		{
			return x * x;
		}
		public static uint Sqr(uint x)
		{
			return x * x;
		}
		public static long Sqr(long x)
		{
			return x * x;
		}
		public static ulong Sqr(ulong x)
		{
			return x * x;
		}
		public static float Sqr(float x)
		{
			return x * x;
		}
		public static double Sqr(double x)
		{
			return x * x;
		}
		public static decimal Sqr(decimal x)
		{
			return x * x;
		}

	}
}


