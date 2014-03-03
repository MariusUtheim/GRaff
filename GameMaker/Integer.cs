using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public struct Integer
	{
		private int _i;

		public Integer(Int32 i)
		{
			this._i = i;
		}

		public static implicit operator Integer(int i) { return new Integer(i); }
		public static implicit operator int(Integer i) { return i._i; }
	}
}
