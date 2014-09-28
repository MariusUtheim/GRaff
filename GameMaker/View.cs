using OpenTK.Graphics.OpenGL;


namespace GameMaker
{
	/// <summary>
	/// Defines which part of the room is being drawn to the screen.
	/// </summary>
	public static class View
	{
		/// <summary>
		/// Gets or sets the x-coordinate of the top-left corner of the view in the room.
		/// </summary>
		public static double X { get; set; }

		/// <summary>
		/// Gets or sets the y-coordinate of the top-left corner of the view in the room.
		/// </summary>
		public static double Y { get; set; }

		/// <summary>
		/// Gets or sets the width of the view in the room.
		/// </summary>
		public static double Width { get; set; }

		/// <summary>
		/// Gets or sets the height of the view in the room.
		/// </summary>
		public static double Height { get; set; }
	
		/// <summary>
		/// Gets or sets the rotation of the view. The view is rotated around the center of the view.
		/// </summary>
		public static Angle Rotation { get; set; }

		/// <summary>
		/// Gets or sets a GameMaker.Rectangle representing the view in the room.
		/// </summary>
		public static Rectangle RoomView
		{
			get { return new Rectangle(X, Y, Width, Height); }
			set
			{
				X = value.Left;
				Y = value.Top;
				Width = value.Width;
				Height = value.Height;
			}
		}

		/// <summary>
		/// Gets or sets the center of the view in the room.
		/// </summary>
		public static Point Center
		{
			get { return new Point(X + Width / 2, Y + Height / 2); }
			set { X = value.X - Width / 2; Y = value.Y - Height / 2; }
		}


		internal static void LoadMatrix()
		{
			Rectangle rect = ActualView();
			double w = rect.Width / 2, h = rect.Height / 2;

			GL.LoadIdentity();
			GL.Ortho(-w, w, h, -h, 0.0, 1.0);
			GL.Rotate(View.Rotation.Degrees, 0, 0, 1);
			GL.Translate(rect.Left - w, rect.Top - h, 0);

		}


		/// <summary>
		/// Gets a GameMaker.Rectangle representing which part of the room is actually being drawn.
		/// This is equal to GameMaker.View.RoomView, unless part of that view is outside of the room.
		/// The actual view is translated to fit inside the room; if the dimensions of the view are larger
		/// than the dimensions of the room, the actual view will also be scaled down.
		/// </summary>
		public static Rectangle ActualView()
		{
#warning DESIGN Ensure that ActualView is always inside the room?
			return RoomView;
		/*	double x = RoomView.Left, y = RoomView.Top, w = RoomView.Width, h = RoomView.Height;

			if (w > Room.Width)
			{
				x = 0;
				w = Room.Width;
			}
			else
				x = GMath.Median(0, x, Room.Width - w);

			if (h > Room.Height)
			{
				y = 0;
				h = Room.Height;
			}
			else
				y = GMath.Median(0, y, Room.Height - h);

			return new Rectangle(x, y, w, h) ;
			*/
		}

		/// <summary>
		/// Gets a factor representing the actual horizontal zoom of the view.
		/// A factor of 1.0 means the view is not zoomed; larger values indicates the view is zoomed in.
		/// </summary>
		public static double HZoom
		{
			get { return ActualView().Width / (double)Room.Width; }
		}

		/// <summary>
		/// Gets a factor representing the actual vertical zoom of the view.
		/// A factor of 1.0 means the view is not zoomed; larger values indicates the view is zoomed in.
		/// </summary>
		public static double VZoom
		{
			get { return ActualView().Height / (double)Room.Height; }
		}
	}
}
