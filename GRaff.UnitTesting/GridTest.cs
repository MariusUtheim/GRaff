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
		public void Grid_ShortestPath()
		{
			var access = new[,] {
				{ true, true, true, true, true, true },
				{ true, false, false, true, false, true },
				{ true, false, true, true, false, true },
				{ false, false, true, false, true, true },
                { true, true, true, true, true, true }
			};

			var grid = new Grid(access);
			Assert.AreEqual(5, grid.Width);
			Assert.AreEqual(6, grid.Height);

			var path = grid.ShortestPath(0, 0, 4, 3);
			var c = path.Vertices.Select(v => v.Location).ToArray();
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
