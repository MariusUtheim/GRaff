using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRaff.Randomness
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns true or false with equal probability.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the boolean value.</param>
        /// <returns>A boolean value.</returns>
        public static bool Boolean(this Random rnd)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            return rnd.Next() % 2 == 0;
        }

        /// <summary>
        /// Returns true with probability p and false with probability 1-p.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the probability.</param>
        /// <param name="p">The probability of returning true</param>
        /// <returns>True with probability p; otherwise, false. If p ≥ 1.0, it always returns true, and if p ≤ 0, it always returns false.</returns>
        public static bool Boolean(this Random rnd, double p)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            return rnd.NextDouble() < p;
        }

        /// <summary>
        /// Returns a random byte.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the probability.</param>
        /// <returns>A byte greater than or equal to zero and less or equal to 255.</returns>
        public static byte Byte(this Random rnd)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            var buffer = new byte[1];
            rnd.NextBytes(buffer);
            return buffer[0];
        }

        /// <summary>
        /// Returns a nonnegative random number.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <returns>An integer greater than or equal to zero and less than System.Int32.MaxValue.</returns>
        public static int Integer(this Random rnd)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            return rnd.Next();
        }


        /// <summary>
        /// Returns a nonnegative random number strictly less than the specified maximum.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.</param>
        /// <returns>An integer greater than or equal to zero and less than maxValue. If however maxValue is zero, then 0 is returned.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">maxValue is less than zero.</exception>
        public static int Integer(this Random rnd, int maxValue)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            Contract.Requires<ArgumentNullException>(maxValue >= 0);
            return rnd.Next(maxValue);
        }


        /// <summary>
        /// Returns a random integer in the specified range.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <param name="minValue">The inclusive lower bound of the random number to be generated.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>An integer greater than or equal to minValue and less than maxValue. If however maxValue and minValue are equal, that value is returned.</returns>
        public static int Integer(this Random rnd, int minValue, int maxValue)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            if (minValue <= maxValue)
                return rnd.Next(minValue, maxValue);
            else
                return 1 + rnd.Next(maxValue, minValue);
        }


        /// <summary>
        /// Returns a random number between 0.0 (inclusive) and 1.0 (exclusive).
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <returns>A number greater than or equal to 0.0 and less than 1.0.</returns>
        public static double Double(this Random rnd)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            return rnd.NextDouble();
        }


        /// <summary>
        /// Returns a random number between 0.0 and boundaryValue exclusive.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <param name="boundaryValue">The exclusive boundary value. The returned value will have the same sign as this number.</param>
        /// <returns>A number between 0.0 inclusive and boundaryValue exclusive.</returns>
        public static double Double(this Random rnd, double boundaryValue)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            return rnd.NextDouble() * boundaryValue;
        }


        /// <summary>
        /// Returns a random number in the specified range.
        /// Note that Double(a, b) is equivalent to Double(b, a) except inclusivity is reversed.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <param name="firstValueInclusive">The first inclusive bound of the random number returned.</param>
        /// <param name="secondValueExclusive">The second exclusive bound of the random number returned.</param>
        /// <returns>A number in the specified range.</returns>
        public static double Double(this Random rnd, double firstValueInclusive, double secondValueExclusive)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            return firstValueInclusive + rnd.NextDouble() * (secondValueExclusive - firstValueInclusive);
        }

        /// <summary>
        /// Returns a string of random letters with the specified length.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <param name="length">The length of the string.</param>
        /// <returns>A random string of letters.</returns>
        public static string String(this Random rnd, int length)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            Contract.Requires<ArgumentOutOfRangeException>(length >= 0);
            var stringBuilder = new StringBuilder(length);
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
        public static double Gaussian(this Random rnd)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            return GMath.Sqrt(-2 * GMath.Log(1.0 - rnd.NextDouble())); // This ensures the number will be in the range (0, 1], so that the logarithm is well-defined
        }

        /// <summary>
        /// Returns a random number that is distributed in accordance with a normal distribution with mean zero and the specified standard deviation.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <param name="std">The standard deviation of the distribution.</param>
        /// <returns>A normally distributed number.</returns>
        public static double Gaussian(this Random rnd, double std)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            return std * rnd.Gaussian();
        }


        /// <summary>
        /// Returns a random number that is distributed in accordance with a normal distribution with the specified mean and standard deviation.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <param name="mean">The mean of the distribution.</param>
        /// <param name="std">The standard deviation of the distribution.</param>
        /// <returns>A normally distributed number.</returns>
        public static double Gaussian(this Random rnd, double mean, double std)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            return std * rnd.Gaussian() + mean;
        }

        /// <summary>
        /// Returns a unit GRaff.Vector with random direction.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <returns>A unit vector with random direction.</returns>
        public static Vector Vector(this Random rnd)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            return new Vector(1, rnd.Angle());
        }


        /// <summary>
        /// Returns a random GRaff.Angle.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <returns>Returns a random angle.</returns>
        public static Angle Angle(this Random rnd)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
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
            Contract.Requires<ArgumentNullException>(rnd != null);
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
            Contract.Requires<ArgumentNullException>(rnd != null);
            if (items == null || items.Length == 0)
                return default(T);
            else
                return items[rnd.Next(items.Length)];
        }

        /// <summary>
        /// Takes a list of relative weights and returns the index of one of them, with probability proportional to the weight of that element.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <param name="weights">A list of non-negative weights.</param>
        /// <returns>The index of one weight at random, with probability proportional to the weight of that element.</returns>
        public static int Pick(this Random rnd, double[] weights)
        {
            Contract.Requires<ArgumentNullException>(weights != null);
            Contract.Requires<ArgumentException>(weights.Length > 0);

            double sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                if (weights[i] < 0)
                    throw new InvalidOperationException("One or more weights were negative");
                sum += weights[i];
            }

            if (sum == 0)
                return rnd.Next(weights.Length);

            for (int i = 0; i < weights.Length; i++)
            {
                sum -= weights[i];
                if (sum <= 0)
                    return i;
            }

            return weights.Length - 1;
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
            Contract.Requires<ArgumentOutOfRangeException>(dice >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(sides >= 1);

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
            Contract.Requires<ArgumentNullException>(rnd != null);
            return (Color)((uint)rnd.Next(0x1000000));
        }

        /// <summary>
        /// Randomizes the order of the elements in the specified array.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers for the shuffling.</param>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to be shuffled.</param>
        public static void ShuffleInPlace<T>(this Random rnd, T[] array)
        {
            rnd.ShuffleInPlace(array, 0, array.Length);
        }

        /// <summary>
        /// Randomizes the order of a range of elements in the specified array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to be shuffled.</param>
        /// <param name="index">The starting index of the range to shuffle.</param>
        /// <param name="end">The last index of the range to shuffle, exclusive.</param>
        public static void ShuffleInPlace<T>(this Random rnd, T[] array, int index, int end)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            Contract.Requires<ArgumentOutOfRangeException>(array == null || (index >= 0 && index <= end && index < array.Length && end <= array.Length));
            if (array == null)
                return;

            for (int i = index + 1; i < end; i++)
            {
                var randomIndex = rnd.Next(i + 1);
                (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
            }
        }


        /// <summary>
        /// Creates a new collection containing the elements of the specified collection in a random order.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <returns>The randomized array.</returns>
        public static IEnumerable<T> Shuffle<T>(this Random rnd, IEnumerable<T> collection)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            if (collection == null)
                yield break;

            var array = collection.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                int randomIndex = rnd.Next(i, array.Length);
                (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
                yield return array[i];
            }
        }

        /// <summary>
        /// Returns the numbers from the specified range in a randomized order.
        /// </summary>
        /// <param name="rnd">The System.Random to generate the numbers.</param>
        /// <param name="start">The first integer in the range to be generated.</param>
        /// <param name="count">The number of integers to be generated.</param>
        /// <returns>the numbers from the range in a randomized order.</returns>
        public static IEnumerable<int> Range(this Random rnd, int start, int count)
        {
            Contract.Requires<ArgumentNullException>(rnd != null);
            return rnd.Shuffle(Enumerable.Range(start, count));
        }

    }
}
