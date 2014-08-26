using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public struct Rectangle
	{
		public Rectangle(double x, double y, double width, double height)
			: this()
		{
			this.Left = x;
			this.Top = y;
			this.Width = width;
			this.Height = height;
		}

		public Rectangle(Point location, Vector size)
			: this(location.X, location.Y, size.X, size.Y) { }

		public double Width { get; set; }
		public double Height { get; set; }

		public double Left
		{
			get;
			set;
		}

		public double Top
		{
			get;
			set;
		}

		public double Right
		{
			get { return Left + Width; }
			set	{ Left = value - Width; }
		}

		public double Bottom
		{
			get { return Top + Height;  }
			set { Top = value - Height; }
		}

		public Point Location
		{
			get { return new Point(Left, Top); }
			set { Left = value.X; Top = value.Y; }
		}

		public Vector Size
		{
			get { return new Vector(Width, Height); }
			set { Width = value.X; Height = value.Y; }
		}

		public Point Center
		{
			get { return Location + Size / 2; }
		}

		public bool Intersects(Rectangle other)
		{
			return !(Left > other.Right || Top > other.Bottom || Right < other.Left || Bottom < other.Top);
		}

		public override string ToString()
		{
			return String.Format("{{Rectangle at {0} by {1}}}", Location, Size);
		}

		public static Rectangle operator +(Rectangle r, Vector v)
		{
			return new Rectangle(r.Location + v, r.Size);
		}

		public static Rectangle operator -(Rectangle r, Vector v)
		{
			return new Rectangle(r.Location - v, r.Size);
		}
	}
}
