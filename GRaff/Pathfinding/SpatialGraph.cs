using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding
{
	public class SpatialGraph : IGraph<SpatialVertex, SpatialEdge>
	{
		private readonly List<SpatialVertex> _vertices = new List<SpatialVertex>();
		private readonly List<SpatialEdge> _edges = new List<SpatialEdge>();
		
		public IEnumerable<SpatialEdge> Edges => _edges.AsReadOnly();

		public bool IsDirected => false;

		public IEnumerable<SpatialVertex> Vertices => _vertices.AsReadOnly();

		public SpatialVertex AddVertex(Point location)
		{
			var vertex = new SpatialVertex(this, location);
			_vertices.Add(vertex);
			return vertex;
		}

		public SpatialEdge AddEdge(SpatialVertex v1, SpatialVertex v2)
		{
			Contract.Requires<ArgumentNullException>(v1 != null && v2 != null);
			Contract.Requires<ArgumentException>(v1.Graph == this && v2.Graph == this);
			if (v1.IsConnectedTo(v2))
				throw new InvalidOperationException("The vertices are already connected");
			var edge = new SpatialEdge(this, v1, v2);
			_edges.Add(new SpatialEdge(this, v2, v1));
			_edges.Add(edge);
			return edge;
		}

	}
}
