using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Randomness;

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

        public static Random Source { get; private set; } = new Random(Time.StartTime);

		public static void Seed(int seed)
		{
			lock (Source)
				Source = new Random(seed);
		}

		/// <summary>
		/// Returns true or false with a equal probability.
		/// </summary>
		/// <returns>A boolean value.</returns>
		public static bool Boolean() { lock (Source) return Source.Boolean(); }

        /// <summary>
        /// Returns true with probability p and false with probability 1-p.
        /// </summary>
        /// <param name="p">The probability of returning true</param>
        /// <returns>True with probability p; otherwise, false. If p ≥ 1.0, it always returns true, and if p ≤ 0, it always returns false.</returns>
        public static bool Boolean(double p) { lock (Source) return Source.Boolean(p); }

        /// <summary>
        /// Returns a random byte.
        /// </summary>
        /// <returns>A byte greater than or equal to zero and less or equal to 255.</returns>
        public static byte Byte() { lock (Source) return Source.Byte(); }

		/// <summary>
		/// Returns a nonnegative random number.
		/// </summary>
		/// <returns>An integer greater than or equal to zero and less than System.Int32.MaxValue.</returns>
		public static int Integer() { lock (Source) return Source.Integer(); }


		/// <summary>
		/// Returns a nonnegative random number strictly less than the specified maximum.
		/// </summary>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.</param>
		/// <returns>An integer greater than or equal to zero and less than maxValue. If however maxValue is zero, then 0 is returned.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">maxValue is less than zero.</exception>
		public static int Integer(int maxValue)
		{
			Contract.Requires<ArgumentOutOfRangeException>(maxValue >= 0);
			lock (Source) return Source.Integer(maxValue);
		}


		/// <summary>
		/// Returns a random integer in the specified range.
		/// </summary>
		/// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
		/// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to minValue.</param>
		/// <returns>An integer greater than or equal to minValue and less than maxValue. If however maxValue and minValue are equal, that value is returned.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">minValue is greater than maxValue</exception>
		public static int Integer(int minValue, int maxValue)
		{
			Contract.Requires<ArgumentOutOfRangeException>(minValue <= maxValue);
			lock (Source) return Source.Integer(minValue, maxValue);
		}


		/// <summary>
		/// Returns a random number between 0.0 and 1.0 exclusive.
		/// </summary>
		/// <returns>A number greater than or equal to 0.0 and less than 1.0.</returns>
		public static double Double() { lock (Source) return Source.Double(); }


		/// <summary>
		/// Returns a random number between 0.0 and boundaryValue exclusive.
		/// </summary>
		/// <param name="boundaryValue">The exclusive boundary value. The returned value will have the same sign as this number.</param>
		/// <returns>A number between 0.0 inclusive and boundaryValue exclusive.</returns>
		public static double Double(double boundaryValue) { lock (Source) return Source.Double(boundaryValue); }


		/// <summary>
		/// Returns a random number in the specified range.
		/// Note that Double(a, b) is equivalent to Double(b, a) except inclusivity is reversed.
		/// </summary>
		/// <param name="firstValue">The first inclusive bound of the random number returned.</param>
		/// <param name="secondValue">The second exclusive bound of the random number returned.</param>
		/// <returns>A number in the specified range.</returns>
		public static double Double(double firstValue, double secondValue) { lock (Source) return Source.Double(firstValue, secondValue); }

		/// <summary>
		/// Returns a string of random letters with the specified length.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <param name="length">The length of the string.</param>
		/// <returns>A random string of letters.</returns>
		public static String String(int length)
		{
			Contract.Requires<ArgumentOutOfRangeException>(length >= 0);
			lock (Source) return Source.String(length);
		}

		/// <summary>
		/// Returns a random number that is distributed in accordance with standard normal distribution.
		/// I.e. a normally distributed variable with mean 0 and standard deviation 1.
		/// </summary>
		/// <returns>A standard normally distributed number.</returns>
		public static double Gaussian() { lock (Source) return Source.Gaussian(); }

		/// <summary>
		/// Returns a random number that is distributed in accordance with a normal distribution with mean zero and the specified standard deviation.
		/// </summary>
		/// <param name="std">The standard deviation of the distribution.</param>
		/// <returns>A normally distributed number.</returns>
		public static double Gaussian(double std) { lock (Source) return Source.Gaussian(std); }

		/// <summary>
		/// Returns a random number that is distributed in accordance with a normal distribution with the specified mean and standard deviation.
		/// </summary>
		/// <param name="mean">The mean of the distribution.</param>
		/// <param name="std">The standard deviation of the distribution.</param>
		/// <returns>A normally distributed number.</returns>
		public static double Gaussian(double mean, double std) { lock (Source) return Source.Gaussian(mean, std); }


		/// <summary>
		/// Returns a unit vector with random direction.
		/// </summary>
		/// <returns>A unit vector with random direction.</returns>
		public static Vector Vector() { lock (Source) return Source.Vector(); }


		/// <summary>
		/// Returns a random angle.
		/// </summary>
		/// <returns>Returns a random angle.</returns>
		public static Angle Angle() { lock (Source) return Source.Angle(); }


		/// <summary>
		/// Returns a random angle inside the clockwise range between the specified boundaries.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <returns>A random angle in the specified range.</returns>
		public static Angle Angle(Angle lowerBound, Angle upperBound) { lock (Source) return Source.Angle(lowerBound, upperBound); }


		/// <summary>
		/// Takes a list of items and picks one of them randomly.
		/// </summary>
		/// <typeparam name="T">The type of the items.</typeparam>
		/// <param name="items">The list of items.</param>
		/// <returns>One of the items chosen randomly. If the list is empty, returns the default value of the type T.</returns>
		public static T Choose<T>(params T[] items) { lock (Source) return Source.Choose(items); }

		/// <summary>
		/// Rolls a number of dice with the specified number of sides, and returns the sum of the roll.
		/// </summary>
		/// <param name="dice">The number of dice to roll. Must be greater than or equal to 0.</param>
		/// <param name="sides">The number of sides on each die. Must be greater than 0.</param>
		/// <returns>The sum of the dice rolled. If nDice is equal to zero, returns 0.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">if dice is less than 0, or if sides is less than or equal to zero.</exception>
		public static int Roll(int dice, int sides)
		{
			Contract.Requires<ArgumentOutOfRangeException>(dice >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(sides >= 1);
			lock (Source) return Source.Roll(dice, sides);
		}

		/// <summary>
		/// Returns a GRaff.Color with random RGB channels.
		/// </summary>
		/// <param name="rnd">The System.Random to generate the numbers.</param>
		/// <returns>A random GRaff.Color with random RGB channels.</returns>
		public static Color Color() { lock (Source) return Source.Color(); }

		/// <summary>
		/// Randomizes the order of the elements in the specified array.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="array">The array to be shuffled.</param>
		public static void ShuffleInPlace<T>(T[] array) { lock (Source) Source.ShuffleInPlace(array); }

		/// <summary>
		/// Randomizes the order of a range of elements in the specified array.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="array">The array to be shuffled.</param>
		/// <param name="index">The starting index of the range to shuffle.</param>
		/// <param name="end">The last index of the range to shuffle, exclusive.</param>
		public static void ShuffleInPlace<T>(T[] array, int index, int end) { lock (Source) Source.ShuffleInPlace(array, index, end); }

		/// <summary>
		/// Creates a new array containing the elements of the specified array in a random order.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="enumerable">The array the elements will be selected from.</param>
		/// <returns>The randomized array.</returns>
		public static IEnumerable<T> Shuffle<T>(IEnumerable<T> enumerable) { lock (Source) return Source.Shuffle(enumerable); }

		/// <summary>
		/// Returns the numbers from the specified range in a randomized order.
		/// </summary>
		/// <param name="start">The first integer in the range to be generated.</param>
		/// <param name="count">The number of integers to be generated.</param>
		/// <returns>the numbers from the range in a randomized order.</returns>
		public static IEnumerable<int> Range(int start, int count) { lock (Source) return Source.Range(start, count); }
	}

}
