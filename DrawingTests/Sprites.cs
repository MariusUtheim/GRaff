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
			Xujia = new Sprite("Assets/xujia.jpg");
		}

		public static Sprite Xujia { get; private set; }
	}
}
