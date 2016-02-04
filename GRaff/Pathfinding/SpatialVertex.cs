using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding
{
	public class SpatialVertex : IVertex<SpatialVertex, SpatialEdge>
	{
		internal readonly List<SpatialEdge> edges = new List<SpatialEdge>();

		public SpatialVertex(IGraph<SpatialVertex, SpatialEdge> graph, double x, double y)
		{
			Contract.Requires<ArgumentNullException>(graph != null);
			this.Graph = graph;
			this.X = x;
			this.Y = y;
		}

		public SpatialVertex(IGraph<SpatialVertex, SpatialEdge> graph, Point location)
			: this (graph, location.X, location.Y)
		{
			Contract.Requires<ArgumentNullException>(graph != null);
		}

		public double X { get; set; }
		public double Y { get; set; }
		public Point Location
		{
			get { return new Point(X, Y); }
			set { X = value.X; Y = value.Y; }
		}


		public IEnumerable<SpatialEdge> Edges => edges.AsReadOnly();

		public IGraph<SpatialVertex, SpatialEdge> Graph { get; }

		public double HeuristicDistance(SpatialVertex other) => (Location - other.Location).Magnitude;

		public bool IsConnectedTo(SpatialVertex other)
		{
			return Edges.Any(e => e.To == other);
		}
	}
}
