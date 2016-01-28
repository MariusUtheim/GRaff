using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GRaff.Pathfinding
{
	public class Vertex : IVertex
	{
		internal readonly List<Edge> edges = new List<Edge>();

		public Vertex(Graph owner)
		{
			Contract.Requires<ArgumentNullException>(owner != null);
			this.Owner = owner;
		}

		public Graph Owner { get; }

		public bool IsConnectedTo(IVertex other)
			=> Edges.Any(e => (e.Vertex1 == this && e.Vertex2 == other) || (e.Vertex1 == other && e.Vertex2 == this));

		public IEnumerable<Edge> Edges => edges.AsReadOnly();

		IGraph<IVertex, IEdge> IVertex.Owner => Owner;

		IEnumerable<IEdge> IVertex.Edges => Edges;
	}
}