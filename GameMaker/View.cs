using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class View
	{
		public static IntRectangle RoomView
		{
			get;
			set;
		}

		public static IntRectangle ViewPort
		{
			get;
			set;
		}


		public static IntRectangle ActualView
		{
			get
			{
				IntRectangle actualRoomView = RoomView;

				if (actualRoomView.Width > Room.Current.Width)
				{
					actualRoomView.Left = 0;
					actualRoomView.Width = Room.Current.Width;
				}
				else
					actualRoomView.Left = GMath.Median(0, actualRoomView.Left, Room.Current.Width - actualRoomView.Width);

				if (actualRoomView.Height > Room.Current.Height)
				{
					actualRoomView.Top = 0;
					actualRoomView.Height = Room.Current.Height;
				}
				else
					actualRoomView.Top = GMath.Median(0, actualRoomView.Top, Room.Current.Height - actualRoomView.Height);

				return actualRoomView;
			}
		}

	}
}
