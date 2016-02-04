using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GRaff.Pathfinding;
using System.Linq;
using GRaff;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class GraphTest
	{

		[TestMethod]
		public void SimpleGraph_Constructor()
		{
			var adjacency = new[,] {
				{ false, true, true },
				{ true, false, false },
				{ true, false, false }
			};

			var graph = new SimpleGraph(adjacency);

			var vertices = graph.Vertices.ToArray();
			for (int i = 0; i < adjacency.GetLength(0); i++)
				for (int j = 0; j < adjacency.GetLength(1); j++)
					Assert.AreEqual(adjacency[i, j], vertices[i].IsConnectedTo(vertices[j]));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void SimpleGraph_AdjacencyRequiresFalseDiagonal()
		{
			new SimpleGraph(new[,] {
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
		public void Graph_DetectDirectedness()
		{
			const int length = 20;
			bool[,] matrix;
			Graph graph;

			matrix = new bool[length, length];
			for (var i = 0; i < length; i++)
			{
				matrix[i, i] = false;
				for (var j = i + 1; j < length; j++)
				{
					matrix[i, j] = GRandom.Boolean();
					matrix[j, i] = (j + i) % 2 == 0;
				}
			}
			graph = new Graph(matrix);
			Assert.IsTrue(graph.IsDirected);


			matrix = new bool[length, length];
			for (var i = 0; i < length; i++)
			{
				matrix[i, i] = false;
				for (var j = i + 1; j < length; j++)
					matrix[i, j] = matrix[j, i] = GRandom.Boolean();
			}
			graph = new Graph(matrix);
			Assert.IsFalse(graph.IsDirected);
		}

		[TestMethod]
		public void Graph_IsTree()
		{
			Graph graph;
			
			graph = new Graph(new[,] {
				{ false, false, false, false, false, true },
				{ false, false, false, false, false, true },
				{ false, false, false, false, true, false },
				{ false, false, false, false, true, false },
				{ false, false, true, true, false, true },
				{ true, true, false, false, true, false }
			});
			Assert.IsTrue(graph.IsTree());

			graph = new Graph(new[,] {
				{ false, false, false, true, false, false },
				{ false, false, true, false, false, false },
				{ false, true, false, true, true, false },
				{ true, false, true, false, true, false },
				{ false, false, true, true, false, true },
                { false, false, false, false, true, false }
			});
			Assert.IsFalse(graph.IsTree());
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
