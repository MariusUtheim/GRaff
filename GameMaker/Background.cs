using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class Background
	{
		static Background()
		{
			Color = Color.LightGray;
		}

		public static Color Color { get; set; }
	}
}
