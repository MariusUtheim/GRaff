using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class GMath
	{
		public const double Tau = 6.283185307179586476925286766559;

		public static double Median(double x1, double x2, double x3)
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

		public static double Sqr(double x)
		{
			return x * x;
		}
	}
}
