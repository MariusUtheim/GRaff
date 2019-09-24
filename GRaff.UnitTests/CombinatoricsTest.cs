using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Randomness;
using Xunit;

namespace GRaff.UnitTesting
{
	public class CombinatoricsTest
	{
		[Fact]
		public void Combinatorics_Combinations()
		{
			// Basic use cases
			Assert.Equal(4455100, Combinatorics.Combinations(300, 3));
            Assert.Equal(3268760, Combinatorics.Combinations(25, 10));
			Assert.Equal(15000, Combinatorics.Combinations(15000, 1));
			Assert.Equal(1, Combinatorics.Combinations(15000, 15000));

			// Special cases
			Assert.Equal(0, Combinatorics.Combinations(10, -2));
			Assert.Equal(0, Combinatorics.Combinations(15, 20));
			Assert.Equal(1, Combinatorics.Combinations(0, 0));

			Assert.Equal(10, Combinatorics.Combinations(-4, 2));
			Assert.Equal(-20, Combinatorics.Combinations(-4, 3));
        }

		[Fact]
		public void Combinatorics_Permutations()
		{
			Assert.Equal(239500800, Combinatorics.Permutations(12, 10));
			Assert.Equal(0, Combinatorics.Permutations(4, 10));
			Assert.Equal(1, Combinatorics.Permutations(0, 0));
        }
	}
}
