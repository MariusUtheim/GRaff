using System;

namespace GRaff.Pathfinding
{
	public class DiagonalGridEdge : IEdge<DiagonalGridVertex, DiagonalGridEdge>
	{

		public DiagonalGridEdge(DiagonalGrid graph, DiagonalGridVertex from, DiagonalGridVertex to)
		{
			this.Graph = graph;
			this.From = from;
			this.To = to;
			this.Weight = from.HeuristicDistance(to);
		}

		public DiagonalGridVertex From { get; }

		public DiagonalGrid Graph { get; }

		IGraph<DiagonalGridVertex, DiagonalGridEdge> IEdge<DiagonalGridVertex, DiagonalGridEdge>.Graph => Graph;

		public DiagonalGridVertex To { get; }

		public double Weight { get; }
	}
}