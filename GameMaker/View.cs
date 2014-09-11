using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class View
	{

		public static double X { get; set; }
		public static double Y { get; set; }
		public static double Width { get; set; }
		public static double Height { get; set; }

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

		public static Point Center
		{
			get { return new Point(X + Width / 2, Y + Height / 2); }
			set { X = value.X - Width / 2; Y = value.Y - Height / 2; }
		}

		public static IntRectangle ViewPort
		{
			get;
			set;
		}

		public static Rectangle ActualView
		{
			get
			{
				Rectangle actualRoomView = RoomView;

				if (actualRoomView.Width > Room.Width)
				{
					actualRoomView.Left = 0;
					actualRoomView.Width = Room.Width;
				}
				else
					actualRoomView.Left = GMath.Median(0, actualRoomView.Left, Room.Width - actualRoomView.Width);

				if (actualRoomView.Height > Room.Height)
				{
					actualRoomView.Top = 0;
					actualRoomView.Height = Room.Height;
				}
				else
					actualRoomView.Top = GMath.Median(0, actualRoomView.Top, Room.Height - actualRoomView.Height);

				return actualRoomView;
			}
		}

		public static double HZoom
		{
			get { return ActualView.Width / (double)Room.Width; }
		}

		public static double VZoom
		{
			get { return ActualView.Height / (double)Room.Height; }
		}
	}
}
