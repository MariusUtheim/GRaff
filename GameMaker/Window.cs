using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GameMaker
{
	/// <summary>
	/// Provides methods for dealing with the game window.
	/// </summary>
	public static class Window
	{
		/// <summary>
		/// Gets or sets the width of the game window.
		/// </summary>
		public static int Width
		{
			get { return Game.Window.Width; }
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException(nameof(value), "Value must be greater than 0");
				Game.Window.Width = value; 
			}
		}

		/// <summary>
		/// Gets or sets the height of the game window.
		/// </summary>
		public static int Height
		{
			get { return Game.Window.Height; }
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException(nameof(value), "Must be greater than 0");
				Game.Window.Height = value;
			}
		}

		/// <summary>
		/// Gets or sets the size of the game window.
		/// </summary>
		public static IntVector Size
		{
			get { return new IntVector(Width, Height); }
			set { Width = value.X; Height = value.Y; }
		}

		/// <summary>
		/// Gets or sets whether the window border should be visible.
		/// </summary>
		public static bool IsBorderVisible
		{
			get { return Game.Window.WindowBorder == WindowBorder.Fixed; }
			set { Game.Window.WindowBorder = value ? WindowBorder.Fixed : WindowBorder.Hidden; }
		}

		/// <summary>
		/// Gets or sets the window title.
		/// </summary>
		public static string Title
		{
			get { return Game.Window.Title; }
			set { Game.Window.Title = value; }
		}
	}
}
