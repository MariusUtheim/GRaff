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
		private readonly bool[,] _isAccessible;
		private readonly GridVertex[,] _vertices;

		public Grid(bool[,] access)
		{
			Contract.Requires<ArgumentNullException>(access != null);
			this._isAccessible = (bool[,])access.Clone();
			_vertices = new GridVertex[access.GetLength(0), access.GetLength(1)];
			for (int i = 0; i < access.GetLength(0); i++)
				for (int j = 0; j < access.GetLength(1); j++)
					if (access[i, j])
						_vertices[i, j] = new GridVertex(this, i, j);
		}

		IEnumerable<IntVector> enumerateRange() => Enumerable.Range(0, Width).Zip(Enumerable.Range(0, Height), (x, y) => new IntVector(x, y));
		[ContractInvariantMethod]
		void invariants()
		{
			Contract.Invariant(Contract.ForAll(enumerateRange(), c => _isAccessible[c.X, c.Y] == (_vertices[c.X, c.Y] != null)));
		}

		public bool IsAccessible(int x, int y)
		{
			return (x >= 0 && y >= 0 && x < Width && y < Height && _isAccessible[x, y]);
		}

		public GridVertex this[int x, int y]
		{
			get
			{
				Contract.Requires<IndexOutOfRangeException>(x >= 0 && x < Width);
				Contract.Requires<IndexOutOfRangeException>(y >= 0 && y < Height);
				Contract.Requires<InvalidOperationException>(IsAccessible(x, y), "Attempting to get a vertex that is not accessible");
                Contract.Ensures(Contract.Result<GridVertex>() != null);
				return _vertices[x, y];
			}
		}
		public GridVertex this[IntVector p] => this[p.X, p.Y];

		public int Width => _isAccessible.GetLength(0);

		public int Height => _isAccessible.GetLength(1);

		public IEnumerable<GridVertex> Vertices
		{
            get
			{
				for (var x = 0; x < Width; x++)
					for (var y = 0; y < Height; y++)
						if (_isAccessible[x, y])
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

		public bool[,] GetMap() => (bool[,])_isAccessible.Clone();
	}
}
