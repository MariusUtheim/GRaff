using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GameMaker
{
	public static class Window
	{
		public static int Width
		{
			get { return Game.Window.Width; }
			set { Game.Window.Width = value; }
		}

		public static int Height
		{
			get { return Game.Window.Height; }
			set { Game.Window.Height = value; }
		}

		public static IntVector Size
		{
			get { return new IntVector(Width, Height); }
			set { Game.Window.Width = value.X; Game.Window.Height = value.Y; }
		}

		public static bool IsBorderVisible
		{
			get { return Game.Window.WindowBorder == WindowBorder.Fixed; }
			set { Game.Window.WindowBorder = value ? WindowBorder.Fixed : WindowBorder.Hidden; }
		}

		public static string Title
		{
			get { return Game.Window.Title; }
			set { Game.Window.Title = value; }
		}
	}
}
