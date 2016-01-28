using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GRaff.Pathfinding
{
	public class Vertex : IVertex<Vertex, Edge>
	{
		internal readonly List<Edge> edges = new List<Edge>();

		public Vertex(IGraph<Vertex, Edge> graph)
		{
			Contract.Requires<ArgumentNullException>(graph != null);
			this.Graph = graph;
		}

		public IGraph<Vertex, Edge> Graph { get; }

		public bool IsConnectedTo(Vertex other)
			=> Edges.Any(e => e.To == other);

		public double HeuristicDistance(Vertex other) => 0;

		public IEnumerable<Edge> Edges => edges.AsReadOnly();

	//	IEnumerable<Edge> Vertex.Edges => Edges;
	}
}