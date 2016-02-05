using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GRaff.Pathfinding
{
	public class Path<TVertex, TEdge> : IEquatable<Path<TVertex, TEdge>>
		where TVertex : IVertex<TVertex, TEdge>
		where TEdge : IEdge<TVertex, TEdge>
	{
		private TVertex[] _vertices;

		public Path(IEnumerable<TEdge> edges)
		{
			Contract.Requires<ArgumentNullException>(edges != null);
			Contract.Requires<ArgumentException>(edges.Count() > 0);
			var e = edges.ToArray();
			_vertices = new TVertex[e.Length + 1];
			for (int i = 0; i < e.Length; i++)
			{
				if (i < e.Length - 1 && !e[i].To.Equals(e[i + 1].From))
					throw new ArgumentException("The edges must specify a continuous path");
				_vertices[i] = e[i].From;
			}

			_vertices[e.Length] = e.Last().To;
		}

		public Path(IEnumerable<TVertex> vertices)
		{
			Contract.Requires<ArgumentNullException>(vertices != null);
			_vertices = vertices.ToArray();
		}

		public int Length => _vertices.Length;

		public IEnumerable<TVertex> Vertices => Array.AsReadOnly(_vertices);

		public IEnumerable<TEdge> Edges
		{
			get
			{
				for (var i = 0; i < _vertices.Length - 1; i++)
					yield return _vertices[i].EdgeTo(_vertices[i + 1]);
            }
		}

		public bool Equals(Path<TVertex, TEdge> other)
		{
			if (other == null || _vertices.Length != other.Length)
				return false;
			for (var i = 0; i < _vertices.Length; i++)
				if (!_vertices[i].Equals(other._vertices[i]))
					return false;
			return true;
		}

		public override bool Equals(object obj) => Equals(obj as Path<TVertex, TEdge>);

		public override int GetHashCode() => GMath.HashCombine(_vertices.Select(v => v.GetHashCode()).ToArray());
	}
}