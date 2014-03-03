using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public struct Real
	{
		private double _d;

		public Real(double d)
		{
			this._d = d;
		}

		public static implicit operator Real(double d) { return new Real(d); }
		public static implicit operator double(Real r) { return r._d; }
	}
}
