using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GRaff.Pathfinding;
using System.Linq;

namespace GameMaker.UnitTesting
{
	[TestClass]
	public class GraphTest
	{

		[TestMethod]
		public void Graph_Constructor()
		{
			var adjacency = new[,] {
				{ false, true, true },
				{ true, false, false },
				{ true, false, false }
			};

			var graph = new Graph(adjacency);

			var vertices = graph.Vertices.ToArray();
			for (int i = 0; i < adjacency.GetLength(0); i++)
				for (int j = 0; j < adjacency.GetLength(1); j++)
					Assert.AreEqual(adjacency[i, j], vertices[i].IsConnectedTo(vertices[j]));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Graph_AdjacencyRequiresSymmetry()
		{
			new Graph(new[,] {
				{ false, true },
                { false, false }
			});
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Graph_AdjacencyRequiresFalseDiagonal()
		{
			new Graph(new[,] {
				{ false, true },
				{ true, true }
			});
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Graph_AdjacencyRequiresSquareMatrix()
		{
			new Graph(new[,] {
				{ false, true, false },
				{ true, false, false }
			});
		}

		[TestMethod]
		public void Graph_ShortestPath()
		{
			var adjacency = new[,] {
				{ false, true, false, false, false, true },
				{ true, false, true, false, false, false },
				{ false, true, false, true, false, false },
				{ false, false, true, false, true, true },
				{ false, false, false, true, false, true },
				{ true, false, false, true, true, false }
			};

			var graph = new Graph(adjacency);
			var vertices = graph.Vertices.ToArray();

			var shortestPath = graph.ShortestPath(vertices[0], vertices[3]);
			Assert.AreEqual(shortestPath[0], vertices[0]);
			Assert.AreEqual(shortestPath[1], vertices[5]);
			Assert.AreEqual(shortestPath[2], vertices[3]);
		}
	}
}
