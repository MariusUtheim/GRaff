using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding
{
	public class Grid : IGraph<GridVertex, GridEdge>
	{
		private readonly bool[,] _isBlocked;
		private readonly GridVertex[,] _vertices;

		public Grid(int width, int height)
		{
			Contract.Requires<ArgumentOutOfRangeException>(width > 0 && height > 0);
			_isBlocked = new bool[width, height];
			_vertices = new GridVertex[width, height];
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					_vertices[x, y] = new GridVertex(this, x, y);
		}

		public Grid(bool[,] blocked)
		{
			Contract.Requires<ArgumentNullException>(blocked != null);
			this._isBlocked = (bool[,])blocked.Clone();
			_vertices = new GridVertex[blocked.GetLength(0), blocked.GetLength(1)];
			for (int i = 0; i < blocked.GetLength(0); i++)
				for (int j = 0; j < blocked.GetLength(1); j++)
					if (!blocked[i, j])
						_vertices[i, j] = new GridVertex(this, i, j);
		}

		public bool IsAccessible(int x, int y)
		{
			return (x >= 0 && y >= 0 && x < Width && y < Height && !_isBlocked[x, y]);
		}

		public GridVertex this[int x, int y]
		{
			get
			{
				return IsAccessible(x, y) ? _vertices[x, y] : null;
			}
		}
		public GridVertex this[IntVector p] => this[p.X, p.Y];

		public int Width => _isBlocked.GetLength(0);

		public int Height => _isBlocked.GetLength(1);

		public IEnumerable<GridVertex> Vertices
		{
            get
			{
				for (var x = 0; x < Width; x++)
					for (var y = 0; y < Height; y++)
						if (!_isBlocked[x, y])
							yield return _vertices[x, y];
			}
		}

		public IEnumerable<GridEdge> Edges
		{
			get
			{
				foreach (var vertex in Vertices)
					foreach (var edge in vertex.Edges)
						yield return edge;
			}
		}

		public bool IsDirected => false;

		public Path<GridVertex, GridEdge> ShortestPath(int xFrom, int yFrom, int xTo, int yTo) => this.ShortestPath(this[xFrom, yFrom], this[xTo, yTo]);
		public Path<GridVertex, GridEdge> ShortestPath(IntVector from, IntVector to) => this.ShortestPath(this[from.X, from.Y], this[to.X, to.Y]);

		public bool[,] GetMap() => (bool[,])_isBlocked.Clone();
	}
}
