﻿using System;
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

		public static explicit operator IntVector(Vector v) { return new IntVector((int)v.X, (int)v.Y); }

		public static explicit operator IntVector(Point p) { return new IntVector((int)p.X, (int)p.Y); }
		
		public override string ToString()
		{
			return String.Format("[{0}, {1}]", X, Y);
		}

		public static IntVector operator +(IntVector v1, IntVector v2) { return new IntVector(v1.X + v2.X, v1.Y + v2.Y); }
		public static IntVector operator -(IntVector v1, IntVector v2) { return new IntVector(v1.X - v2.X, v1.Y - v2.Y); }
		public static IntVector operator *(IntVector v, int i) { return new IntVector(v.X * i, v.Y * i); }
		public static IntVector operator /(IntVector v, int i) { return new IntVector(v.X / i, v.Y / i); }
	}
}
