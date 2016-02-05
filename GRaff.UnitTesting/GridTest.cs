using System;
using System.Linq;
using GRaff.Pathfinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class GridTest
	{
		[TestMethod]
		public void Grid_Dimensionality()
		{
			const int w = 20, h = 30;
			var block = new bool[w, h];
			for (var x = 0; x < w; x++)
				for (var y = 0; y < h; y++)
					block[x, y] = GRandom.Boolean();

			var grid = new Grid(block);
			Assert.AreEqual(w, grid.Width);
			Assert.AreEqual(h, grid.Height);

			for (var x = 0; x < w; x++)
				for (var y = 0; y < h; y++)
				{
					Assert.AreEqual(block[x, y], !grid.IsAccessible(x, y));
					Assert.AreEqual(block[x, y], grid[x, y] == null);
					Assert.IsTrue(grid[x, y] == grid[new IntVector(x, y)]);
				}
		}	

		[TestMethod]
		public void Grid_ShortestPath()
		{
			var block = new[,] {
				{ false, false, false, false, false, false },
				{ false, true, true, false, true, false },
				{ false, true, false, false, true, false },
				{ true, true, false, true, false, false },
                { false, false, false, false, false, false }
			};

			var grid = new Grid(block);
			Assert.AreEqual(5, grid.Width);
			Assert.AreEqual(6, grid.Height);

			var path = grid.ShortestPath(0, 0, 3, 4);
			var c = path.Vertices.Select(v => v.Location).ToArray();
			Assert.AreEqual(10, c.Length);
			Assert.AreEqual(new IntVector(0, 0), c[0]);
			Assert.AreEqual(new IntVector(0, 1), c[1]);
			Assert.AreEqual(new IntVector(0, 2), c[2]);
			Assert.AreEqual(new IntVector(0, 3), c[3]);
			Assert.AreEqual(new IntVector(0, 4), c[4]);
			Assert.AreEqual(new IntVector(0, 5), c[5]);
			Assert.AreEqual(new IntVector(1, 5), c[6]);
			Assert.AreEqual(new IntVector(2, 5), c[7]);
			Assert.AreEqual(new IntVector(3, 5), c[8]);
			Assert.AreEqual(new IntVector(3, 4), c[9]);
        }
	}
}
