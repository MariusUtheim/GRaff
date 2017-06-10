using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding
{
	public class Graph : IGraph<Vertex, Edge>
	{
		private readonly List<Vertex> _vertices = new List<Vertex>();
		private readonly List<Edge> _edges = new List<Edge>();

		public Graph()
		{

		}

		public Graph(bool[,] adjacency)
		{
			Contract.Requires<ArgumentNullException>(adjacency != null);
			Contract.Requires<ArgumentException>(adjacency.GetLength(0) == adjacency.GetLength(1));

			int n = adjacency.GetLength(0);
			_vertices = Enumerable.Range(0, n).Select(_ => new Vertex(this)).ToList();

			for (int i = 0; i < n; i++)
			{
				if (adjacency[i, i])
					AddEdge(_vertices[i], _vertices[i]);
                for (int j = i + 1; j < n; j++)
				{
					if (adjacency[i, j])
						this.AddEdge(_vertices[i], _vertices[j]);
					if (adjacency[j, i])
						this.AddEdge(_vertices[j], _vertices[i]);
					if (adjacency[i, j] != adjacency[j, i])
						IsDirected = true;
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

		public Edge AddEdge(Vertex from, Vertex to)
		{
			Contract.Requires<ArgumentNullException>(from != null && to != null);
			Contract.Requires<ArgumentException>(from.Graph == this && to.Graph == this);
			var e = new Edge(this, from, to);
			_edges.Add(e);
			return e;
		}

		public void RemoveEdge(Edge e)
		{
			Contract.Requires<ArgumentNullException>(e != null);
			Contract.Requires<ArgumentException>(e.Graph == this);
			Contract.Requires<InvalidOperationException>(Edges.Contains(e), "The specified edge has already been removed.");
			e.From.edges.Remove(e);
			_edges.Remove(e);
		}

		public void RemoveVertex(Vertex v)
		{
			Contract.Requires<ArgumentNullException>(v != null);
			Contract.Requires<ArgumentException>(v.Graph == this);
			Contract.Requires<InvalidOperationException>(Vertices.Contains(v), "The specified vertex has already been removed.");
			foreach (var edge in v.Edges)
				_edges.Remove(edge);
			foreach (var edge in _edges.Where(e => e.To == v))
			{
				edge.From.edges.Remove(edge);
				_edges.Remove(edge);
			}
			_vertices.Remove(v);
		}

		public IEnumerable<Vertex> Vertices => _vertices.AsReadOnly();

		public IEnumerable<Edge> Edges => _edges.AsReadOnly();
	}
}
