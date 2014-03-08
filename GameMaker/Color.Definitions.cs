using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public partial struct Color
	{
		static Color()
		{
			Red = new Color(255, 0, 0);
			Green = new Color(0, 255, 0);
			Blue = new Color(0, 0, 255);
			Black = new Color(0, 0, 0);
			White = new Color(255, 255, 255);
			Gray = new Color(128, 128, 128);
			LightGray = new Color(196, 196, 196);
			Yellow = new Color(255, 255, 0);
		}

		public static Color Red { get; private set; }
		
		public static Color Green { get; private set; }
		
		public static Color Blue { get; private set; }
		
		public static Color Black { get; private set; }
		
		public static Color White { get; private set; }
		
		public static Color Gray { get; private set; }

		public static Color LightGray { get; private set; }

		public static Color Yellow { get; private set; }

	}
}
