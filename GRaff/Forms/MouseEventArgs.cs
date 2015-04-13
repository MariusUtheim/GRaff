namespace GRaff.Forms
{
	public class MouseEventArgs : FormEventArgs
	{
		public MouseEventArgs(MouseButton button, Point location)
		{
			this.Button = button;
			this.Location = location;
		}

		public MouseButton Button { get; private set; }
		public Point Location { get; private set; }
	}
}