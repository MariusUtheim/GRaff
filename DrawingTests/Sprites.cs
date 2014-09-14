using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMaker;

namespace DrawingTests
{
	static class Sprites
	{
		static Sprites()
		{
			Xujia = new Sprite("Assets/xujia.jpg", 2, null, true);
		}

		public static Sprite Xujia { get; private set; }
	}
}
