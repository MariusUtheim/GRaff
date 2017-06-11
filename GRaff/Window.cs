using System;
using System.Drawing;
using OpenTK; 

namespace GRaff
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
			get { return (int)(Giraffe.Window.ClientSize.Width / DisplayScale.X); }
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("value", "Value must be greater than 0");
				Giraffe.Window.ClientSize = new Size((int)(value * DisplayScale.X), Giraffe.Window.ClientSize.Height); 
			}
		}

		/// <summary>
		/// Gets or sets the height of the game window.
		/// </summary>
		public static int Height
		{
			get { return (int)(Giraffe.Window.ClientSize.Height / DisplayScale.Y); }
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("value", "Must be greater than 0");
				Giraffe.Window.ClientSize = new Size(Giraffe.Window.ClientSize.Width, (int)(value * DisplayScale.Y));
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

        public static Vector DisplayScale { get; internal set; }

		/// <summary>
		/// Gets or sets whether the window border should be visible.
		/// </summary>
		public static bool IsBorderVisible
		{
			get { return Giraffe.Window.WindowBorder == WindowBorder.Fixed; }
			set { Giraffe.Window.WindowBorder = value ? WindowBorder.Fixed : WindowBorder.Hidden; }
		}

		/// <summary>
		/// Gets or sets the window title.
		/// </summary>
		public static string Title
		{
			get { return Giraffe.Window.Title; }
			set { Giraffe.Window.Title = value; }
		}

		public static bool IsFullscreen
		{
			get
			{
				return Giraffe.Window.WindowState == WindowState.Fullscreen;
			}
			set
			{
				Giraffe.Window.WindowState = WindowState.Fullscreen;
			}
		}

		public static int X
		{
			get
			{
				return Giraffe.Window.X;
			}
			set
			{
				Giraffe.Window.X = value;
			}
		}

		public static int Y
		{
			get
			{
				return Giraffe.Window.Y;
			}

			set
			{
				Giraffe.Window.Y = value;
			}
		}
	}
}
