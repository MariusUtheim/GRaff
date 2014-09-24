using System;

namespace GameMaker
{
	/// <summary>
	/// Defines the event data for a mouse event.
	/// </summary>
	public class MouseEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the GameMaker.MouseEventArgs class using the specified GameMaker.MouseButton.
		/// </summary>
		/// <param name="button">The GameMaker.MouseButton associated with the event.</param>
		public MouseEventArgs(MouseButton button)
		{
			this.Button = button;
		}

		/// <summary>
		/// Gets the GameMaker.MouseButton associated with the event.
		/// </summary>
		public MouseButton Button { get; private set; }
	}
}