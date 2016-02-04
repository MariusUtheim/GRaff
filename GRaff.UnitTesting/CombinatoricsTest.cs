using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Randomness;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class CombinatoricsTest
	{
		[TestMethod]
		public void Combinatorics_Combinations()
		{
			// Basic use cases
			Assert.AreEqual(4455100, Combinatorics.Combinations(300, 3));
            Assert.AreEqual(3268760, Combinatorics.Combinations(25, 10));
			Assert.AreEqual(15000, Combinatorics.Combinations(15000, 1));
			Assert.AreEqual(1, Combinatorics.Combinations(15000, 15000));

			// Special cases
			Assert.AreEqual(0, Combinatorics.Combinations(10, -2));
			Assert.AreEqual(0, Combinatorics.Combinations(15, 20));
			Assert.AreEqual(1, Combinatorics.Combinations(0, 0));

			Assert.AreEqual(10, Combinatorics.Combinations(-4, 2));
			Assert.AreEqual(-20, Combinatorics.Combinations(-4, 3));
        }

		[TestMethod]
		public void Combinatorics_Permutations()
		{
			Assert.AreEqual(239500800, Combinatorics.Permutations(12, 10));
			Assert.AreEqual(0, Combinatorics.Permutations(4, 10));
			Assert.AreEqual(1, Combinatorics.Permutations(0, 0));
        }
	}
}
