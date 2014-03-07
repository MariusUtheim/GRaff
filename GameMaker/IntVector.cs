using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public struct IntVector
	{
		public IntVector(int x, int y)
			: this()
		{
			this.X = x;
			this.Y = y;
		}

		public int X { get; set; }

		public int Y { get; set; }

		public static implicit operator Vector(IntVector i) { return new Vector(i.X, i.Y); }

		public override string ToString()
		{
			return String.Format("[{0}, {1}]", X, Y);
		}
	}
}
