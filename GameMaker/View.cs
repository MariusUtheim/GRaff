using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class View
	{
		public View(int width, int height)
		{
			_roomView = new IntRectangle(0, 0, width, height);
			_port = new IntRectangle(0, 0, width, height);
		}

		public View(IntRectangle roomView, IntRectangle port)
		{
			this._roomView = roomView;
			this._port = port;
		}

		private IntRectangle _roomView;
		public IntRectangle RoomView
		{
			get { return _roomView; }
			set { _roomView = value; }
		}

		public IntVector Location
		{
			get { return _roomView.Location; }
			set { _roomView.Location = value; }
		}

		public IntVector Size
		{
			get { return _roomView.Size; }
			set { _roomView.Size = value; }
		}

		public IntVector Center
		{
			get { return _roomView.Location + _roomView.Size / 2; }
			set { _roomView.Location = value - _roomView.Size / 2; }
		}

		public IntRectangle ActualRoomView
		{
			get
			{
				IntRectangle actualRoomView = _roomView;

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

		private IntRectangle _port;
		public IntRectangle Port
		{
			get { return _port; }
			set { _port = value; }
		}

		internal void Redraw()
		{
			throw new NotImplementedException();//Draw.CurrentSurface.Blit(Draw.DefaultSurface, this.ActualRoomView, this.Port);
		}
	}
}
