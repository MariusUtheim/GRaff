using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GRaff.Pathfinding
{
	public class Path<TVertex, TEdge> 
		where TVertex : IVertex<TVertex, TEdge>
		where TEdge : IEdge<TVertex, TEdge>
	{
		private TVertex[] _vertices;

		public Path(IEnumerable<TVertex> vertices)
		{
			Contract.Requires<ArgumentNullException>(vertices != null);
			_vertices = vertices.ToArray();
		}

		public TVertex this[int index] => _vertices[index];

		public int Length => _vertices.Length;
	}
}