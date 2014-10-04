using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff;

namespace DrawingTests
{
	static class Sounds
	{
		static Sounds()
		{
			Starlight = new Sound(@"C:\test\testogg.ogg", true);
		}

		public static Sound Starlight { get; private set; }
	}
}
