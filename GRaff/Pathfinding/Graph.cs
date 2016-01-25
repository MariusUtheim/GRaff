using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding
{
	public class Graph
	{
		List<IVertex> _vertices;
		List<IEdge> _edges;

		public class Vertex : IVertex
		{
			internal Graph Owner;
			internal List<Edge> Edges = new List<Edge>();

			internal Vertex(Graph owner)
			{
				this.Owner = owner;
			}

			public bool IsConnectedTo(Vertex other)
				=> Edges.Any(e => (e.V1 == this && e.V2 == other) || (e.V1 == other && e.V2 == this));

			IEnumerable<IEdge> IVertex.Edges() => Edges.ToArray();
		}

		public class Edge : IEdge
		{
			internal Graph Owner;
			internal Vertex V1;
			internal Vertex V2;

			internal Edge(Graph owner, Vertex v1, Vertex v2)
			{
				this.Owner = owner;
				this.V1 = v1;
				v1.Edges.Add(this);
				this.V2 = v2;
				v2.Edges.Add(this);
			}
		}


		public Graph(bool[,] adjacency)
		{
			throw new NotImplementedException();
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
			Contract.Requires<ArgumentException>(v1.Owner == this && v2.Owner == this, "Cannot add an edge between vertices that do not belong to the graph.");
			Contract.Requires<InvalidOperationException>(!v1.IsConnectedTo(v2), "An edge already exists between the specified vertices.");
			var e = new Edge(this, v1, v2);
			_edges.Add(e);
			return e;
		}

		private void _removeUnsafe(Edge e)
		{
			e.V1.Edges.Remove(e);
			e.V2.Edges.Remove(e);
			_edges.Remove(e);
		}

		public void RemoveEdge(Edge e)
		{
			Contract.Requires<ArgumentNullException>(e != null);
			Contract.Requires<ArgumentException>(e.Owner == this, "The specified edge that does not belong to the graph.");
			Contract.Requires<InvalidOperationException>(_edges.Contains(e), "The specified edge has already been removed.");
			_removeUnsafe(e);
		}

		public void RemoveVertex(Vertex v)
		{
			Contract.Requires<ArgumentNullException>(v != null);
			Contract.Requires<ArgumentException>(v.Owner == this, "The specified vertex does not belong to the graph.");
			Contract.Requires<InvalidOperationException>(_vertices.Contains(v), "The specified vertex has already been removed.");
			foreach (var e in v.Edges)
				_removeUnsafe(e);
			_vertices.Remove(v);
		}

		public bool EdgeExists(Vertex v1, Vertex v2)
		{
			Contract.Requires<ArgumentNullException>(v1 != null && v2 != null);
			Contract.Requires<ArgumentException>(v1.Owner == this && v2.Owner == this, "Cannot add an edge between vertices that do not belong to the graph.");
			return v1.IsConnectedTo(v2);
		}



		public IReadOnlyList<IVertex> Vertices => _vertices.AsReadOnly();

		public IReadOnlyList<IEdge> Edges => _edges.AsReadOnly();
		
		public Path ShortestPath(IVertex v1, IVertex v2)
		{
			throw new NotImplementedException();
		}
	}
}
