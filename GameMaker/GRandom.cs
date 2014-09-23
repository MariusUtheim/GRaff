using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	/// <summary>
	/// Provides static methods for generating pseudo-random numbers.
	/// These methods encapsulate an instance of the System.Random class, initialized with a seed value that is
	/// dependant on when the game started. 
	/// </summary>
	public static class GRandom
	{
		private static Random _rnd;

		static GRandom()
		{ 
			_rnd = new Random(Time.GameTime);
		}


		/// <summary>
		/// Returns a nonnegative random number.
		/// </summary>
		/// <returns>An integer greater than or equal to zero and less than System.Int32.MaxValue.</returns>
		public static int Int() => _rnd.Next();


		/// <summary>
		/// Returns a nonnegative random number strictly less than the specified maximum.
		/// </summary>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.</param>
		/// <returns>An integer greater than or equal to zero and less than maxValue. If however maxValue is zero, then 0 is returned.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">maxValue is less than zero.</exception>
		public static int Int(int maxValue) => _rnd.Next(maxValue);
	

		/// <summary>
		/// Returns a random integer in the specified range.
		/// </summary>
		/// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to minValue.</param>
		/// <returns>An integer greater than or equal to minValue and less than maxValue. If however maxValue and minValue are equal, that value is returned.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">minValue is greater than maxValue</exception>
		public static int Int(int minValue, int maxValue) => _rnd.Next(minValue, maxValue);
		

		/// <summary>
		/// Returns a random number between 0.0 and 1.0 exclusive.
		/// </summary>
		/// <returns>A number greater than or equal to 0.0 and less than 1.0.</returns>
		public static double Double() => _rnd.NextDouble();
		

		/// <summary>
		/// Returns a random number between 0.0 and boundaryValue exclusive.
		/// </summary>
		/// <param name="boundaryValue">The exclusive boundary value. The returned value will have the same sign as this number.</param>
		/// <returns>A number between 0.0 inclusive and boundaryValue exclusive.</returns>
		public static double Double(double boundaryValue) => _rnd.NextDouble() * boundaryValue;
		

		/// <summary>
		/// Returns a random number in the specified range.
		/// Note that Double(a, b) is equivalent to Double(b, a) except inclusivity is reversed.
		/// </summary>
		/// <param name="firstValue">The first inclusive bound of the random number returned.</param>
		/// <param name="secondValue">The second exclusive bound of the random number returned.</param>
		/// <returns>A number in the specified range.</returns>
		public static double Double(double firstValue, double secondValue) => firstValue + _rnd.NextDouble() * (secondValue - firstValue);
		

		/// <summary>
		/// Returns a random number that is distributed in accordance with standard normal distribution.
		/// I.e. a normally distributed variable with mean 0 and standard deviation 1.
		/// </summary>
		/// <returns>A standard normally distributed number.</returns>
		public static double Gaussian() => GMath.Sqrt(-2 * GMath.Log(Double()));
		

		/// <summary>
		/// Returns a random number that is distributed in accordance with a normal distribution with the specified mean and standard deviation.
		/// </summary>
		/// <param name="mean">The mean of the distribution.</param>
		/// <param name="std">The standard deviation of the distribution.</param>
		/// <returns>A normally distributed number.</returns>
		public static double Gaussian(double mean, double std) => std * (Gaussian() + mean);
		

		/// <summary>
		/// Returns a unit vector with random direction.
		/// </summary>
		/// <returns>A unit vector with random direction.</returns>
		public static Vector Vector() => new Vector(1, GRandom.Angle());
	

		/// <summary>
		/// Returns a random angle.
		/// </summary>
		/// <returns>Returns a random angle.</returns>
		public static Angle Angle() => GameMaker.Angle.Deg(_rnd.NextDouble() * 360.0);
		

		/// <summary>
		/// Returns a random angle inside the clockwise range between the specified boundaries.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <returns>A random angle in the specified range.</returns>
		public static Angle Angle(Angle lowerBound, Angle upperBound) => GameMaker.Angle.Deg(lowerBound.Degrees + _rnd.NextDouble() * (upperBound - lowerBound).Degrees);
		

		/// <summary>
		/// Takes a list of items and picks one of them randomly.
		/// </summary>
		/// <typeparam name="T">The type of the items.</typeparam>
		/// <param name="items">The list of items.</param>
		/// <returns>One of the items chosen randomly. If the list is empty, returns the default value of the type T.</returns>
		public static T Choose<T>(params T[] items)
		{
			if (items.Length == 0)
				return default(T);
			else
				return items[_rnd.Next(items.Length)];
		}

		/// <summary>
		/// Returns true with probability p; otherwise, returns false.
		/// </summary>
		/// <param name="p">The probability of returning true</param>
		/// <returns>True with probability p; otherwise, false. If p ≥ 1.0, it always returns true, and if p ≤ 0, it always returns false.</returns>
		public static bool Probability(double p)
		{
			if (_rnd.NextDouble() < p)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Rolls a number of dice with the specified number of sides, and returns the sum of the roll.
		/// </summary>
		/// <param name="nDice">The number of dice to roll. Must be greater than or equal to 0.</param>
		/// <param name="nSides">The number of sides on each die. Must be greater than 0.</param>
		/// <returns>The sum of the dice rolled. If nDice is equal to zero, returns 0.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">if nDice is less than 0, or if nSides is less than or equal to zero.</exception>
		public static int Roll(int nDice, int nSides)
		{
			if (nDice < 0) throw new ArgumentOutOfRangeException("nDice", "Must be greater than or equal to 0");
			if (nSides < 1) throw new ArgumentOutOfRangeException("nSides", "Must be greater than 0");

			if (nDice == 0)
				return 0;

			int sum = nDice;
			for (int i = 0; i < nDice; i++)
				sum += GRandom.Int(nSides);
			return sum;
		}
	}
}
