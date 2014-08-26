using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	/// <summary>
	/// Represents a point in space. Points cannot be added together, but they can have vectors added to them and their difference can be found as a vector.
	/// </summary>
	public struct Point
	{
		public static readonly Point Zero = new Point(0, 0);
		public double X { get; private set; }
		public double Y { get; private set; }

		public Point(double x, double y)
			: this()
		{
			this.X = x;
			this.Y = y;
		}

		public override string ToString()
		{
			return String.Format("({0}, {1})", X, Y);
		}


		public static Point operator +(Point p1, Point p2)
		{
			return new Point(p1.X + p2.X, p1.Y + p2.Y);
		}

		public static Point operator +(Point p, Vector v)
		{
			return new Point(p.X + v.X, p.Y + v.Y);
		}

		public static Point operator +(Point p, IntVector v)
		{
			return new Point(p.X + v.X, p.Y + v.Y);
		}

		public static Point operator -(Point p, Vector v)
		{
			return new Point(p.X - v.X, p.Y - v.Y);
		}

		public static Point operator -(Point p, IntVector v)
		{
			return new Point(p.X - v.X, p.Y - v.Y);
		}

		public static Vector operator -(Point p1, Point p2)
		{
			return new Vector(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static Point operator *(Point p, double scalar)
		{
			return new Point(p.X * scalar, p.Y * scalar);
		}

		public static Point operator /(Point p, double scalar)
		{
			return new Point(p.X / scalar, p.Y / scalar);
		}

		public static bool operator ==(Point p1, Point p2)
		{
			return p1.X == p2.X && p1.Y == p2.Y;
		}

		public static bool operator !=(Point p1, Point p2)
		{
			return p1.X != p2.X || p1.Y != p2.Y;
		}

		public static implicit operator Vector(Point p) { return new Vector(p.X, p.Y); }
	}
}
