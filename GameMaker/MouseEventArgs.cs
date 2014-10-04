using System;

namespace GRaff
{
	/// <summary>
	/// Defines the event data for a mouse event.
	/// </summary>
	public class MouseEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the GRaff.MouseEventArgs class using the specified GRaff.MouseButton.
		/// </summary>
		/// <param name="button">The GRaff.MouseButton associated with the event.</param>
		public MouseEventArgs(MouseButton button)
		{
			this.Button = button;
		}

		/// <summary>
		/// Gets the GRaff.MouseButton associated with the event.
		/// </summary>
		public MouseButton Button { get; private set; }
	}
}