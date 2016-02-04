using System;
using System.Diagnostics.Contracts;

namespace GRaff.Pathfinding
{
	public class SpatialEdge : IEdge<SpatialVertex, SpatialEdge>
	{
		public SpatialEdge(IGraph<SpatialVertex, SpatialEdge> graph, SpatialVertex from, SpatialVertex to)
		{
			Contract.Requires<ArgumentNullException>(graph != null && from != null && to != null);
			Contract.Requires<ArgumentException>(from.Graph == graph && to.Graph == graph);
			Contract.Requires<ArgumentException>(from != to);
			this.Graph = graph;
			this.From = from;
			this.To = to;
			from.edges.Add(this);
			to.edges.Add(this);
		}

		public SpatialVertex From { get; }

		public IGraph<SpatialVertex, SpatialEdge> Graph { get; }

		public SpatialVertex To { get; }

		public double Weight => (From.Location - To.Location).Magnitude;

		public Line Line => new Line(From.Location, To.Location);
	}
}