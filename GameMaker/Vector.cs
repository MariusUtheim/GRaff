using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	/// <summary>
	/// Vectors are considerably slower than points, but their direction is always preserved.
	/// </summary>
	public struct Vector
	{

		public Vector(double x, double y)
			: this()
		{
			_setCartesian(x, y);
		}

		public Vector(double magnitude, Angle direction)
			: this()
		{
			this.Magnitude = magnitude;
			this.Direction = direction;
		}

		public static Vector FromPolar(double magnitude, Angle direction)
		{
			return new Vector { Magnitude = magnitude, Direction = direction };
		}

		public static Vector FromCartesian(double x, double y)
		{
			return new Vector { X = x, Y = y };
		}

		public double Magnitude { get; set; }
		public Angle Direction { get; set; }
		public double X
		{
			get { return Magnitude * Math.Cos(Direction); }
			set { _setCartesian(value, Y); }
		}
		public double Y
		{
			get { return Magnitude * Math.Sin(Direction); }
			set { _setCartesian(X, value); }
		}
		private void _setCartesian(double x, double y)
		{
			Magnitude = Math.Sqrt(x * x + y * y);
			if (x != 0 || y != 0)
				Direction = Angle.Direction(x, y);
		}

		public Vector Normal
		{
			get { return Vector.FromPolar(1, this.Direction); }
		}

		public double DotProduct(Vector other)
		{
			return Magnitude * other.Magnitude * Math.Cos((Direction - other.Direction).Radians);
		}

		public override string ToString()
		{
			return String.Format("[{0}, {1}]", X, Y);
		}

		public static implicit operator Point(Vector v) { return new Point(v.X, v.Y); }

		public static Vector operator +(Vector v1, Vector v2) { return new Vector(v1.X + v2.X, v1.Y + v2.Y); }
		public static Vector operator -(Vector v1, Vector v2) { return new Vector(v1.X - v2.X, v1.Y - v2.Y); }
		public static Vector operator *(Vector v, double d) { return Vector.FromPolar(v.Magnitude * d, v.Direction); }
		public static Vector operator *(double d, Vector v) { return Vector.FromPolar(d * v.Magnitude, v.Direction); }

		public static Vector operator +(Vector v, Angle a) { return new Vector(v.Magnitude, v.Direction + a); }

		public static Vector operator -(Vector v, Angle a) { return new Vector(v.Magnitude, v.Direction - a); }
	}
}
