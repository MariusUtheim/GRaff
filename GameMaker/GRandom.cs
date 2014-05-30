using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class GRandom
	{
		internal static Random _rnd;

		static GRandom()
		{
			_rnd = new Random();
		}

		public static int Int(int maxValue)
		{
			return _rnd.Next(maxValue);
		}

		public static int Int(int minValue, int maxValue)
		{
			return _rnd.Next(minValue, maxValue);
		}

		public static double Double()
		{
			return _rnd.NextDouble();
		}

		public static double Double(double maxValue)
		{
			return _rnd.NextDouble() * maxValue;
		}

		public static double Double(double minValue, double maxValue)
		{
			return minValue + _rnd.NextDouble() * (maxValue - minValue);
		}

		public static Vector Vector(double magnitude)
		{
			return new Vector(magnitude, GameMaker.Angle.FromRadians(_rnd.NextDouble() * GMath.Tau));
		}

		public static Vector Vector()
		{ 
			return GRandom.Vector(1);
		}

		public static Angle Angle() { return GameMaker.Angle.FromRadians(_rnd.NextDouble() * GMath.Tau); }

		public static Angle Angle(Angle lowerBound, Angle upperBound)
		{
			return GameMaker.Angle.FromRadians(lowerBound + _rnd.NextDouble() * (upperBound - lowerBound));
		}

		public static T Choose<T>(params T[] items)
		{
			return items[_rnd.Next(items.Length)];
		}

		/// <summary>
		/// Returns true with probability p; otherwise, returns false.
		/// </summary>
		/// <param name="p">The probability of returning true</param>
		/// <returns>true with probability p or if p > 1; otherwise, false</returns>
		public static bool Probability(double p)
		{
			if (_rnd.NextDouble() < p)
				return true;
			else
				return false;
		}

		public static int Roll(int nDice, int nSides)
		{
			int sum = nDice;
			for (int i = 0; i < nDice; i++)
				sum += GRandom.Int(nSides);
			return sum;
		}
	}
}
