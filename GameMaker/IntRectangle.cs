using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public struct IntRectangle
	{
		public IntRectangle(int x, int y, int width, int height)
			: this()
		{
			this.Left = x;
			this.Top = y;
			this.Width = width;
			this.Height = height;
		}

		public IntRectangle(IntVector location, IntVector size)
			: this(location.X, location.Y, size.X, size.Y) { }

		public int Width { get; set; }
		public int Height { get; set; }

		public int Left { get; set; }

		public int Top
		{
			get;
			set;
		}

		public int Right
		{
			get { return Left + Width; }
			set { Left = value - Width; }
		}

		public int Bottom
		{
			get { return Top + Height; }
			set { Top = value - Height; }
		}

		public IntVector Location
		{
			get { return new IntVector(Left, Top); }
			set { Left = value.X; Top = value.Y; }
		}

		public IntVector Size
		{
			get { return new IntVector(Width, Height); }
			set { Width = value.X; Height = value.Y; }
		}


		public bool Intersects(IntRectangle other)
		{
			return !(Left > other.Right || Top > other.Bottom || Right < other.Left || Bottom < other.Top);
		}

		public static implicit operator Rectangle(IntRectangle r) { return new Rectangle(r.Left, r.Top, r.Width, r.Height); }

		public override string ToString()
		{
			return String.Format("IntRectangle: [({0},{1}), ({2},{3})]", Left, Top, Width, Height);
		}
	}
}
