using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public struct Complex
	{
		private double _r, _i;

		public Complex(double r, double i)
		{
			this._r = r;
			this._i = i;
		}

		public double Real
		{
			get { return _r; }
			set { _r = value; }
		}

		public double Imaginary
		{
			get { return _i; }
			set { _i = value; }
		}

		public static Complex Polar(double magnitude, double argument)
		{
			return new Complex(magnitude * GMath.Cos(argument), magnitude * GMath.Sin(argument));
		}


		public static Complex operator +(Complex z, Complex w)
		{
			return new Complex(z._r + w._r, z._i + w._i);
		}

		public static Complex operator -(Complex z, Complex w)
		{
			return new Complex(z._r - w._r, z._i - w._i);
		}

		public static Complex operator *(Complex z, Complex w)
		{
			return new Complex(z._r * w._r - z._i * w._i, z._r * w._i + z._i * w._r);
		}

		public static Complex operator /(Complex z, Complex w)
		{
			double m = 1 / (w._r * w._r + w._i * w._i);
			return new Complex(m * (z._r * w._r + z._i * w._i), m * (z._i * w._r - z._r * w._i));
		}

		public static implicit operator Complex(double d)
		{
			return new Complex(d, 0);
		}

		public static explicit operator Complex(Point p)
		{
			return new Complex(p.X, p.Y);
		}
	}
}
