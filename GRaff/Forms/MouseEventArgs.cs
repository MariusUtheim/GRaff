using System;

namespace GRaff.Forms
{
	public class MouseEventArgs : EventArgs
	{
		public MouseEventArgs(MouseButton button, Point location)
		{
			this.Button = button;
			this.Location = location;
		}

		public MouseButton Button { get; private set; }
		public Point Location { get; private set; }
		public bool IsHandled { get; set; } = false;
	}
}