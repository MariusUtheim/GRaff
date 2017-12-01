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
        public static Vector DisplayScale { get; internal set; }

		/// <summary>
		/// Gets or sets whether the window border should be visible.
		/// </summary>
		public static bool IsBorderVisible
		{
			get => Giraffe.Window.WindowBorder == WindowBorder.Fixed; 
			set => Giraffe.Window.WindowBorder = value ? WindowBorder.Fixed : WindowBorder.Hidden; 
		}

		/// <summary>
		/// Gets or sets the window title.
		/// </summary>
		public static string Title
		{
			get => Giraffe.Window.Title;
			set => Giraffe.Window.Title = value;
		}

		public static bool IsFullscreen
		{
			get => Giraffe.Window.WindowState == WindowState.Fullscreen;
			set => Giraffe.Window.WindowState = WindowState.Fullscreen;
		}


        /// <summary>
        /// Gets or sets the width of the game window.
        /// </summary>
        public static int Width
        {
            get => Size.X;
            set => Size = (value, Height);
        }

        /// <summary>
        /// Gets or sets the height of the game window.
        /// </summary>
        public static int Height
        {
            get => Size.Y;
            set => Size = (Width, value);
        }

        /// <summary>
        /// Gets or sets the size of the game window.
        /// </summary>
        public static IntVector Size
        {
            get
            {
                return new IntVector((int)(Giraffe.Window.ClientSize.Width / DisplayScale.X), (int)(Giraffe.Window.ClientSize.Height / DisplayScale.Y));
            }
            set
            {
                if (value.X <= 0 || value.Y <= 0)
                    throw new ArgumentOutOfRangeException("value", "Both components must be greater than 0.");
                Giraffe.Window.ClientSize = new Size(value.X, value.Y);
                View.UpdateGLToScreenMatrix();
            }
        }



        public static int X
        {
            get => Giraffe.Window.X;
            set => Giraffe.Window.X = value;
        }

        public static int Y
        {
            get => Giraffe.Window.Y;
            set => Giraffe.Window.Y = value;
        }

        public static IntVector Location
        {
            get => (X, Y);
            set => (X, Y) = value;
        }

        public static Point Center => (Width / 2.0, Height / 2.0);

        public static IntRectangle ClientRectangle => new IntRectangle(0, 0, Width, Height);

    }
}
