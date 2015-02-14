using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	/// <summary>
	/// Provides static methods for generating pseudo-random numbers.
	/// These methods encapsulate an instance of the System.Random class, initialized with a seed value that is
	/// dependant on when the game started. 
	/// </summary>
	/// <remarks>The extension methods are not thread-safe. The other static methods use the same System.Random object to generate random numbers, using serialized thread-safe access.</remarks>
	public static class GRandom
	{
		#region System.Random calls

		/// <summary>
		/// Returns a nonnegative random number.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <returns>An integer greater than or equal to zero and less than System.Int32.MaxValue.</returns>
		public static int Integer(this Random rnd) { return rnd.Next(); }


		/// <summary>
		/// Returns a nonnegative random number strictly less than the specified maximum.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.</param>
		/// <returns>An integer greater than or equal to zero and less than maxValue. If however maxValue is zero, then 0 is returned.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">maxValue is less than zero.</exception>
		public static int Integer(this Random rnd, int maxValue) { return rnd.Next(maxValue); }


		/// <summary>
		/// Returns a random integer in the specified range.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to minValue.</param>
		/// <returns>An integer greater than or equal to minValue and less than maxValue. If however maxValue and minValue are equal, that value is returned.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">minValue is greater than maxValue</exception>
		public static int Integer(this Random rnd, int minValue, int maxValue) { return rnd.Next(minValue, maxValue); }


		/// <summary>
		/// Returns a random number between 0.0 and 1.0 exclusive.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <returns>A number greater than or equal to 0.0 and less than 1.0.</returns>
		public static double Double(this Random rnd) { return rnd.NextDouble(); }


		/// <summary>
		/// Returns a random number between 0.0 and boundaryValue exclusive.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="boundaryValue">The exclusive boundary value. The returned value will have the same sign as this number.</param>
		/// <returns>A number between 0.0 inclusive and boundaryValue exclusive.</returns>
		public static double Double(this Random rnd, double boundaryValue) { return rnd.NextDouble() * boundaryValue; }


		/// <summary>
		/// Returns a random number in the specified range.
		/// Note that Double(a, b) is equivalent to Double(b, a) except inclusivity is reversed.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="firstValueInclusive">The first inclusive bound of the random number returned.</param>
		/// <param name="secondValueExclusive">The second exclusive bound of the random number returned.</param>
		/// <returns>A number in the specified range.</returns>
		public static double Double(this Random rnd, double firstValueInclusive, double secondValueExclusive) { return firstValueInclusive + rnd.NextDouble() * (secondValueExclusive - firstValueInclusive); }

		/// <summary>
		/// Returns a string of random letters with the specified length.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="length">The length of the string.</param>
		/// <returns>A random string of letters.</returns>
		public static string String(this Random rnd, int length)
		{
			StringBuilder stringBuilder = new StringBuilder(length);
			for (int i = 0; i < length; i++)
				stringBuilder.Append((char)rnd.Next('A', 'z'));
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Returns a random number that is distributed in accordance with standard normal distribution.
		/// I.e. a normally distributed variable with mean 0 and standard deviation 1.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <returns>A standard normally distributed number.</returns>
		public static double Gaussian(this Random rnd) { return GMath.Sqrt(-2 * GMath.Log(rnd.NextDouble())); }

		/// <summary>
  /// Returns a random number that is distributed in accordance with a normal distribution with mean zero and the specified standard deviation.
  /// </summary>
  /// <param name="rnd">The System.Random to generate the numbers.</param>
  /// <param name="std">The standard deviation of the distribution.</param>
  /// <returns>A normally distributed number.</returns>
		public static double Gaussian(this Random rnd, double std) { return std * rnd.Gaussian(); }


		/// <summary>
		/// Returns a random number that is distributed in accordance with a normal distribution with the specified mean and standard deviation.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="mean">The mean of the distribution.</param>
		/// <param name="std">The standard deviation of the distribution.</param>
		/// <returns>A normally distributed number.</returns>
		public static double Gaussian(this Random rnd, double mean, double std) { return std * rnd.Gaussian() + mean; }

		/// <summary>
		/// Returns a unit GRaff.Vector with random direction.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <returns>A unit vector with random direction.</returns>
		public static Vector Vector(this Random rnd) { return new Vector(1, rnd.Angle()); }


		/// <summary>
		/// Returns a random GRaff.Angle.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <returns>Returns a random angle.</returns>
		public static Angle Angle(this Random rnd)
		{
			// System.Random.Next() can generate up to 30 full bits uniformly, since it excludes the upper bound
			ulong firstBits, middleBits, lastFourBits;

			firstBits = (ulong)rnd.Next(1 << 30);
			middleBits = (ulong)rnd.Next(1 << 30);
			lastFourBits = (ulong)rnd.Next(1 << 4);

			ulong data = (firstBits << 34) | (middleBits << 4) | (lastFourBits);

			return new Angle(data);
		}


		/// <summary>
		/// Returns a random GRaff.Angle inside the clockwise range between the specified boundaries.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <returns>A random angle in the specified range.</returns>
		public static Angle Angle(this Random rnd, Angle lowerBound, Angle upperBound)
		{
			return GRaff.Angle.Rad(lowerBound.Radians + rnd.NextDouble() * (upperBound - lowerBound).Radians);
		}


		/// <summary>
		/// Takes a list of items and picks one of them randomly.
		/// </summary>
		/// <typeparam name="T">The type of the items.</typeparam>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="items">The list of items.</param>
		/// <returns>One of the items chosen randomly. If the list is empty, returns the default value of the type T.</returns>
		public static T Choose<T>(this Random rnd, params T[] items)
		{
			if (items == null || items.Length == 0)
				return default(T);
			if (items.Length == 0)
				return default(T);
			else
				return items[rnd.Next(items.Length)];
		}

		/// <summary>
		/// Returns true with probability p; otherwise, returns false.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="p">The probability of returning true</param>
		/// <returns>True with probability p; otherwise, false. If p ≥ 1.0, it always returns true, and if p ≤ 0, it always returns false.</returns>
		public static bool Probability(this Random rnd, double p)
		{
			if (rnd.NextDouble() < p)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Rolls a number of dice with the specified number of sides, and returns the sum of the roll.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="dice">The number of dice to roll. Must be greater than or equal to 0.</param>
		/// <param name="sides">The number of sides on each die. Must be greater than 0.</param>
		/// <returns>The sum of the dice rolled. If nDice is equal to zero, returns 0.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">if nDice is less than 0, or if nSides is less than or equal to zero.</exception>
		public static int Roll(this Random rnd, int dice, int sides)
		{
			if (dice < 0) throw new ArgumentOutOfRangeException("dice", "Must be greater than or equal to 0");
			if (sides < 1) throw new ArgumentOutOfRangeException("sides", "Must be greater than 0");

			if (dice == 0)
				return 0;

			int sum = dice;
			for (int i = 0; i < dice; i++)
				sum += rnd.Next(sides);
			return sum;
		}

		/// <summary>
		/// Returns a GRaff.Color with random RGB channels.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <returns>A random GRaff.Color with random RGB channels.</returns>
		public static Color Color(this Random rnd)
		{
			return (Color)(0xFF000000 | (rnd.Next() % 0xFF000000));
		}

		#endregion


		#region Static calls
		/*C#6.0*/
		private static Random _rnd = new Random(Time.MachineTime);

		public static Random Source { get { return _rnd; } }

		/// <summary>
		/// Returns a nonnegative random number.
		/// </summary>
		/// <returns>An integer greater than or equal to zero and less than System.Int32.MaxValue.</returns>
		public static int Integer() { lock (_rnd) return _rnd.Integer(); }


		/// <summary>
		/// Returns a nonnegative random number strictly less than the specified maximum.
		/// </summary>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.</param>
		/// <returns>An integer greater than or equal to zero and less than maxValue. If however maxValue is zero, then 0 is returned.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">maxValue is less than zero.</exception>
		public static int Integer(int maxValue) { lock (_rnd) return _rnd.Integer(maxValue); }


		/// <summary>
		/// Returns a random integer in the specified range.
		/// </summary>
		/// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to minValue.</param>
		/// <returns>An integer greater than or equal to minValue and less than maxValue. If however maxValue and minValue are equal, that value is returned.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">minValue is greater than maxValue</exception>
		public static int Integer(int minValue, int maxValue) { lock (_rnd) return _rnd.Integer(minValue, maxValue); }


		/// <summary>
		/// Returns a random number between 0.0 and 1.0 exclusive.
		/// </summary>
		/// <returns>A number greater than or equal to 0.0 and less than 1.0.</returns>
		public static double Double() { lock (_rnd) return _rnd.Double(); }


		/// <summary>
		/// Returns a random number between 0.0 and boundaryValue exclusive.
		/// </summary>
		/// <param name="boundaryValue">The exclusive boundary value. The returned value will have the same sign as this number.</param>
		/// <returns>A number between 0.0 inclusive and boundaryValue exclusive.</returns>
		public static double Double(double boundaryValue) { lock (_rnd) return _rnd.Double(boundaryValue); }


		/// <summary>
		/// Returns a random number in the specified range.
		/// Note that Double(a, b) is equivalent to Double(b, a) except inclusivity is reversed.
		/// </summary>
		/// <param name="firstValue">The first inclusive bound of the random number returned.</param>
		/// <param name="secondValue">The second exclusive bound of the random number returned.</param>
		/// <returns>A number in the specified range.</returns>
		public static double Double(double firstValue, double secondValue) { lock (_rnd) return _rnd.Double(firstValue, secondValue); }

		/// <summary>
		/// Returns a string of random letters with the specified length.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="length">The length of the string.</param>
		/// <returns>A random string of letters.</returns>
		public static String String(int length) { lock (_rnd) return _rnd.String(length); }

		/// <summary>
		/// Returns a random number that is distributed in accordance with standard normal distribution.
		/// I.e. a normally distributed variable with mean 0 and standard deviation 1.
		/// </summary>
		/// <returns>A standard normally distributed number.</returns>
		public static double Gaussian() { lock (_rnd) return _rnd.Gaussian(); }


		/// <summary>
		/// Returns a random number that is distributed in accordance with a normal distribution with the specified mean and standard deviation.
		/// </summary>
		/// <param name="mean">The mean of the distribution.</param>
		/// <param name="std">The standard deviation of the distribution.</param>
		/// <returns>A normally distributed number.</returns>
		public static double Gaussian(double mean, double std) { lock (_rnd) return _rnd.Gaussian(mean, std); }


		/// <summary>
		/// Returns a unit vector with random direction.
		/// </summary>
		/// <returns>A unit vector with random direction.</returns>
		public static Vector Vector() { lock (_rnd) return _rnd.Vector(); }


		/// <summary>
		/// Returns a random angle.
		/// </summary>
		/// <returns>Returns a random angle.</returns>
		public static Angle Angle() { lock (_rnd) return _rnd.Angle(); }


		/// <summary>
		/// Returns a random angle inside the clockwise range between the specified boundaries.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <returns>A random angle in the specified range.</returns>
		public static Angle Angle(Angle lowerBound, Angle upperBound) { lock (_rnd) return _rnd.Angle(lowerBound, upperBound); }


		/// <summary>
		/// Takes a list of items and picks one of them randomly.
		/// </summary>
		/// <typeparam name="T">The type of the items.</typeparam>
		/// <param name="items">The list of items.</param>
		/// <returns>One of the items chosen randomly. If the list is empty, returns the default value of the type T.</returns>
		public static T Choose<T>(params T[] items) { lock (_rnd) return _rnd.Choose(items); }

		/// <summary>
		/// Returns true with probability p; otherwise, returns false.
		/// </summary>
		/// <param name="p">The probability of returning true</param>
		/// <returns>True with probability p; otherwise, false. If p ≥ 1.0, it always returns true, and if p ≤ 0, it always returns false.</returns>
		public static bool Probability(double p) { lock (_rnd) return _rnd.Probability(p); }

		/// <summary>
		/// Rolls a number of dice with the specified number of sides, and returns the sum of the roll.
		/// </summary>
		/// <param name="dice">The number of dice to roll. Must be greater than or equal to 0.</param>
		/// <param name="sides">The number of sides on each die. Must be greater than 0.</param>
		/// <returns>The sum of the dice rolled. If nDice is equal to zero, returns 0.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">if nDice is less than 0, or if nSides is less than or equal to zero.</exception>
		public static int Roll(int dice, int sides) { lock (_rnd) return _rnd.Roll(dice, sides); }

		/// <summary>
		/// Returns a GRaff.Color with random RGB channels.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <returns>A random GRaff.Color with random RGB channels.</returns>
		public static Color Color() { lock (_rnd) return _rnd.Color(); }

		#endregion

	}

}
