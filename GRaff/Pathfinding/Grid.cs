using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Pathfinding
{
	public class Grid
	{
		private bool[,] _access;

		public Grid(bool[,] access)
		{
			Contract.Requires<ArgumentNullException>(access != null);
			this._access = (bool[,])access.Clone();
		}

		public bool this[int x, int y]
		{
			get { return _access[x, y]; }
			set { _access[x, y] = value; }
		}

		public bool this[IntVector p]
		{
			get { return _access[p.X, p.Y]; }
			set { _access[p.X, p.Y] = value; }
		}

		public int Width => _access.GetLength(0);

		public int Height => _access.GetLength(1);

		public bool[,] GetMap() => (bool[,])_access.Clone();

		public IntVector[] ShortestPath(IntVector from, IntVector to)
		{


			throw new NotImplementedException();
		}
		
	}
}
