using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class Window
	{
		public static bool IsBorderVisible
		{
			get { return GraphicsEngine.Current.IsBorderVisible; }
			set { GraphicsEngine.Current.IsBorderVisible = value; }
		}
	}
}
