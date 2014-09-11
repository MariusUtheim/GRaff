using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public struct Angle
	{
		//private double _radians;
		private double _degrees;
		public static readonly Angle Zero = new Angle();

		public static Angle Rad(double radians)
		{
			return new Angle { Radians = radians };
		}

		public static Angle Deg(double degrees)
		{
			return new Angle { Degrees = degrees };
		}

		/// <summary>
		/// Returns the angle of the direction from the origin to (x, y)
		/// </summary>
		public static Angle Direction(double x, double y)
		{
			return Angle.Rad(Math.Atan2(y, x));
		}

		public static Angle Direction(double x1, double y1, double x2, double y2)
		{
			return Angle.Rad(Math.Atan2(y2 - y1, x2 - x1));
		}

		public double Radians
		{
			get { return _degrees * GMath.Tau / 360.0; }
			set { _degrees = value * 360.0 / GMath.Tau; }
		}

		public double Degrees
		{
			get { return _degrees; }
			set { _degrees = value; }
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public static implicit operator double(Angle a)
		{
			return a._degrees;
		}

		public static explicit operator Angle(double d)
		{
			return Angle.Deg(d);
		}

		public static Angle operator +(Angle a, Angle b)
		{
			return Angle.Deg(a._degrees + b._degrees);
		}

		public static Angle operator +(Angle a, double d)
		{
			return Angle.Deg(a._degrees + d);
		}

		public static Angle operator -(Angle a, double d)
		{
			return Angle.Deg(a._degrees - d);
		}

		public static Angle operator *(Angle a, double d)
		{
			return Angle.Deg(a._degrees * d);
		}

	}
}
