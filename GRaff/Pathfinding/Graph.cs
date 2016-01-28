using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding
{
	public partial class Graph : IGraph<Vertex, Edge>
	{
		private readonly List<Vertex> _vertices;
		private readonly List<Edge> _edges = new List<Edge>();


		public Graph(bool[,] adjacency)
		{
			Contract.Requires<ArgumentNullException>(adjacency != null);
			Contract.Requires<ArgumentException>(adjacency.GetLength(0) == adjacency.GetLength(1));

			int n = adjacency.GetLength(0);
			_vertices = Enumerable.Range(0, n).Select(_ => new Vertex(this)).ToList();

			for (int i = 0; i < n; i++)
			{
				if (adjacency[i, i])
					throw new ArgumentException("The adjacency matrix specifies that a vertex is connected to itself.");
				for (int j = i + 1; j < n; j++)
				{
					if (adjacency[i, j] != adjacency[j, i])
						throw new ArgumentException("The adjacency matrix must be symmetric.");
					if (adjacency[i, j])
						this.AddEdge(_vertices[i], _vertices[j]);
				}
			}
		}

		public Vertex AddVertex()
		{
			var v = new Vertex(this);
			_vertices.Add(v);
			return v;
		}

		public Edge AddEdge(Vertex v1, Vertex v2)
		{
			Contract.Requires<ArgumentNullException>(v1 != null && v2 != null);
			Contract.Requires<ArgumentException>(v1.Owner == this && v2.Owner == this);
			Contract.Requires<InvalidOperationException>(!v1.IsConnectedTo(v2), "An edge already exists between the specified vertices.");
			var e = new Edge(this, v1, v2);
			_edges.Add(e);
			return e;
		}

		private void _removeUnsafe(Edge e)
		{
			Contract.Assume(e != null);
			e.Vertex1.edges.Remove(e);
			e.Vertex2.edges.Remove(e);
			_edges.Remove(e);
		}

		public void RemoveEdge(Edge e)
		{
			Contract.Requires<ArgumentNullException>(e != null);
			Contract.Requires<ArgumentException>(e.Owner == this);
			Contract.Requires<InvalidOperationException>(Edges.Contains(e), "The specified edge has already been removed.");
			_removeUnsafe(e);
		}

		public void RemoveVertex(Vertex v)
		{
			Contract.Requires<ArgumentNullException>(v != null);
			Contract.Requires<ArgumentException>(v.Owner == this);
			Contract.Requires<InvalidOperationException>(Vertices.Contains(v), "The specified vertex has already been removed.");
			foreach (var e in v.Edges)
				_removeUnsafe(e);
			_vertices.Remove(v);
		}


		public IEnumerable<Vertex> Vertices => _vertices.AsReadOnly();

		public IEnumerable<Edge> Edges => _edges.AsReadOnly();
	}
}
