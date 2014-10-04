using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff;

namespace DrawingTests
{
	static class Sprites
	{
		static Sprites()
		{
			Xujia = new Sprite("Assets/xujia.jpg", 1, null, true);
		}

		public static Sprite Xujia { get; private set; }
	}
}
