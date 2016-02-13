using System;
using System.Collections.Generic;

namespace GRaff.Pathfinding
{
	public class DiagonalGridVertex : IVertex<DiagonalGridVertex, DiagonalGridEdge>
	{
		private IEnumerable<DiagonalGridEdge> _edges = null;

		internal DiagonalGridVertex(DiagonalGrid owner, int x, int y)
		{
			this.X = x;
			this.Y = y;
			this.Graph = owner;
		}

		public IEnumerable<DiagonalGridEdge> Edges
		{
			get
			{
				if (_edges != null)
					return _edges;
				var edges = new List<DiagonalGridEdge>(8);
				if (Graph.IsAccessible(X, Y - 1)) edges.Add(new DiagonalGridEdge(Graph, this, Graph[X, Y - 1]));
				if (Graph.IsAccessible(X + 1, Y - 1)) edges.Add(new DiagonalGridEdge(Graph, this, Graph[X + 1, Y - 1]));
				if (Graph.IsAccessible(X + 1, Y)) edges.Add(new DiagonalGridEdge(Graph, this, Graph[X + 1, Y]));
				if (Graph.IsAccessible(X + 1, Y + 1)) edges.Add(new DiagonalGridEdge(Graph, this, Graph[X + 1, Y + 1]));
				if (Graph.IsAccessible(X, Y + 1)) edges.Add(new DiagonalGridEdge(Graph, this, Graph[X, Y + 1]));
				if (Graph.IsAccessible(X - 1, Y + 1)) edges.Add(new DiagonalGridEdge(Graph, this, Graph[X - 1, Y + 1]));
				if (Graph.IsAccessible(X - 1, Y)) edges.Add(new DiagonalGridEdge(Graph, this, Graph[X - 1, Y]));
				if (Graph.IsAccessible(X - 1, Y - 1)) edges.Add(new DiagonalGridEdge(Graph, this, Graph[X - 1, Y - 1]));
				return _edges = Array.AsReadOnly(edges.ToArray());
			}
		}

		public int X { get; }
		public int Y { get; }
		public IntVector Location => new IntVector(X, Y);

		public DiagonalGrid Graph { get; }
		IGraph<DiagonalGridVertex, DiagonalGridEdge> IVertex<DiagonalGridVertex, DiagonalGridEdge>.Graph => Graph;


		public bool IsConnectedTo(DiagonalGridVertex other)
		{
			var gridOther = other as DiagonalGridVertex;
			if (gridOther == null || Graph != other.Graph)
				return false;
			return HeuristicDistance(gridOther) <= 1;
		}

		public double HeuristicDistance(DiagonalGridVertex other)
		{
			double dx = GMath.Abs(X - other.X), dy = GMath.Abs(Y - other.Y);
			return GMath.Max(dx, dy) + (GMath.Sqrt(2) - 1) * GMath.Min(dx, dy); // sqrt(2) * min + (max - min)
		}

		public double EuclideanDistance(DiagonalGridVertex other)
		{
			double dx = GMath.Abs(X - other.X), dy = GMath.Abs(Y - other.Y);
			return GMath.Sqrt(dx * dx + dy * dy);
		}

		public override string ToString()
		{
			return $"DiagonalGridVertex {Location}";
		}

	}
}