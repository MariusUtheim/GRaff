using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding
{

	public class SimpleGraph : IGraph<Vertex, Edge>
	{
		private readonly List<Vertex> _vertices;
		private readonly List<Edge> _edges = new List<Edge>();


		public SimpleGraph(bool[,] adjacency)
		{
			Contract.Requires<ArgumentNullException>(adjacency != null);
			Contract.Requires<ArgumentException>(adjacency.GetLength(0) == adjacency.GetLength(1));

			int n = adjacency.GetLength(0);
			_vertices = Enumerable.Range(0, n).Select(_ => new Vertex(this)).ToList();

			for (int i = 0; i < n; i++)
			{
				if (adjacency[i, i])
					throw new ArgumentException("A simple graph cannot contain loops (i.e. the diagonal of the adjacency matrix cannot contain true values)");
                for (int j = i + 1; j < n; j++)
				{
					if (adjacency[i, j] != adjacency[j, i])
						throw new ArgumentException("A simple graph must be undirected (i.e. the adjacency matrix must be symmetrix");
					if (adjacency[i, j])
						this.Connect(_vertices[i], _vertices[j]);
				}
			}
		}


		public bool IsDirected { get; private set; }


		public Vertex AddVertex()
		{
			var v = new Vertex(this);
			_vertices.Add(v);
			return v;
		}

		public void Connect(Vertex from, Vertex to)
		{
			Contract.Requires<ArgumentNullException>(from != null && to != null);
			Contract.Requires<ArgumentException>(from.Graph == this && to.Graph == this);
			Contract.Requires<InvalidOperationException>(!from.IsConnectedTo(to), "An edge already exists between the specified vertices.");
			_edges.Add(new Edge(this, from, to));
			_edges.Add(new Edge(this, to, from));
		}

		private void _removeUnsafe(Edge edge)
		{
			Contract.Assume(edge != null);
			var backEdge = edge.To.edges.First(e => e.To == edge.From);
			edge.From.edges.Remove(edge);
			edge.To.edges.Remove(backEdge);
			_edges.Remove(edge);
			_edges.Remove(backEdge);
		}
		public void RemoveEdge(Edge e)
		{
			Contract.Requires<ArgumentNullException>(e != null);
			Contract.Requires<ArgumentException>(e.Graph == this);
			Contract.Requires<InvalidOperationException>(Edges.Contains(e), "The specified edge has already been removed.");
			_removeUnsafe(e);
		}

		public void RemoveVertex(Vertex v)
		{
			Contract.Requires<ArgumentNullException>(v != null);
			Contract.Requires<ArgumentException>(v.Graph == this);
			Contract.Requires<InvalidOperationException>(Vertices.Contains(v), "The specified vertex has already been removed.");
			foreach (var e in v.Edges)
				_removeUnsafe(e);
			_vertices.Remove(v);
		}

		public IEnumerable<Vertex> Vertices => _vertices.AsReadOnly();

		public IEnumerable<Edge> Edges => _edges.AsReadOnly();
	}
}
