using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace GRaff.Pathfinding
{
	public class GridVertex : IVertex<GridVertex, GridEdge>
	{
		private GridEdge[] _edges = null;

		internal GridVertex(Grid owner, int x, int y)
		{
			this.X = x;
			this.Y = y;
			this.Graph = owner;
		}

		public IEnumerable<GridEdge> Edges
		{
			get
			{
				if (_edges != null)
					return _edges;
				var edges = new List<GridEdge>(4);
				if (Graph.IsAccessible(X, Y - 1)) edges.Add(new GridEdge(Graph, this, Graph[X, Y - 1]));
				if (Graph.IsAccessible(X - 1, Y)) edges.Add(new GridEdge(Graph, this, Graph[X - 1, Y]));
				if (Graph.IsAccessible(X + 1, Y)) edges.Add(new GridEdge(Graph, this, Graph[X + 1, Y]));
				if (Graph.IsAccessible(X, Y + 1)) edges.Add(new GridEdge(Graph, this, Graph[X, Y + 1]));
				return _edges = edges.ToArray();
			}
		}
		
		public int X { get; }
		public int Y { get; }
		public IntVector Location => new IntVector(X, Y);

		public Grid Graph { get; }
		IGraph<GridVertex, GridEdge> IVertex<GridVertex, GridEdge>.Graph => Graph;


		public bool IsConnectedTo(GridVertex other)
		{
			var gridOther = other as GridVertex;
			if (gridOther == null || Graph != other.Graph)
				return false;
			return HeuristicDistance(gridOther) <= 1;
		}

		public double HeuristicDistance(GridVertex other)
		{
			return GMath.Abs(X - other.X) + GMath.Abs(Y - other.Y);
		}

		public double EuclideanDistance(GridVertex other)
		{
			double dx = GMath.Abs(X - other.X), dy = GMath.Abs(Y - other.Y);
			return GMath.Sqrt(dx * dx + dy * dy);
		}

		public override string ToString()
		{
			return $"GridVertex {Location}";
        }

	}
}