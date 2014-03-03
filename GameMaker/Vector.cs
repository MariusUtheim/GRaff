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
				Direction = GMath.Atan2(y, x);
		}

		public Vector Normal
		{
			get { return Vector.FromPolar(1, this.Direction); }
		}

		public override string ToString()
		{
			return String.Format("[{0}, {1}]", X, Y);
		}

		public static implicit operator Point(Vector v) { return new Point(v.X, v.Y); }

		public static Vector operator +(Vector v1, Vector v2) { return new Vector(v1.X + v2.X, v1.Y + v2.Y); }
		// And so on...
		/// TEMPORARY /// Implement binary-, unary-, scalar*, dot product, +/- Angle
		/// 

		public static Vector operator +(Vector v, Angle a) { return new Vector(v.Magnitude, v.Direction + a); }

		public static Vector operator -(Vector v, Angle a) { return new Vector(v.Magnitude, v.Direction - a); }
	}
}
