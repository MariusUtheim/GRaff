using System;

namespace GRaff.Pathfinding
{
	public class GridEdge : IEdge<GridVertex, GridEdge>
	{

		public GridEdge(Grid graph, GridVertex from, GridVertex to)
		{
			this.Graph = graph;
			this.From = from;
			this.To = to;
		}

		public Grid Graph { get; }

		public GridVertex From { get; }

		public GridVertex To { get; }

		public double Weight => 1;

		IGraph<GridVertex, GridEdge> IEdge<GridVertex, GridEdge>.Graph => Graph;

		public override string ToString()
		{
			return $"GridEdge {From.Location} -> {To.Location}";
        }
	}
}