using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public struct Angle
	{
		private double _radians;
		public static readonly Angle Zero = new Angle();

		public static Angle FromRadians(double radians)
		{
			return new Angle { Radians = radians };
		}

		public static Angle FromDegrees(double degrees)
		{
			return new Angle { Degrees = degrees };
		}

		/// <summary>
		/// Returns the angle of the direction from the origin to (x, y)
		/// </summary>
		public static Angle Direction(double x, double y)
		{
			return Angle.FromRadians(Math.Atan2(y, x));
		}

		public static Angle Direction(double x1, double y1, double x2, double y2)
		{
			return Angle.FromRadians(Math.Atan2(y2 - y1, x2 - x1));
		}

		public double Radians
		{
			get { return _radians; }
			set { _radians = value % GMath.Tau; }
		}

		public double Degrees
		{
			get { return _radians * 360 / GMath.Tau; }
			set { _radians = (value % 360.0) * GMath.Tau / 360; }
		}

		public static implicit operator double(Angle a)
		{
			return a._radians;
		}

		public static explicit operator Angle(double d)
		{
			return Angle.FromRadians(d);
		}

		public static Angle operator +(Angle a, Angle b)
		{
			return Angle.FromRadians(a._radians + b._radians);
		}

		public static Angle operator +(Angle a, double d)
		{
			return Angle.FromRadians(a._radians + d);
		}

		public static Angle operator -(Angle a, double d)
		{
			return Angle.FromRadians(a._radians - d);
		}

		public static Angle operator *(Angle a, double d)
		{
			return Angle.FromRadians(a._radians * d);
		}

	}
}
