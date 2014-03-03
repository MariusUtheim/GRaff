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
		public static readonly Angle Zero = new Angle(0);

		public Angle(double radians)
		{
			this._radians = radians;
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
			return new Angle(d);
		}

		public static Angle operator +(Angle a, Angle b)
		{
			return new Angle(a._radians + b._radians);
		}
	}
}
