using System;

namespace GRaff
{
	/// <summary>
	/// Defines the event data for a keyboard event.
	/// </summary>
	public class KeyEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the GRaff.KeyEventArgs class using the specified GRaff.Key.
		/// </summary>
		/// <param name="button">The GRaff.Key associated with the event.</param>
		public KeyEventArgs(Key key)
		{
			this.Key = key;
		}

		/// <summary>
		/// Gets the GRaff.Key associated with the event.
		/// </summary>
		public Key Key { get; private set; }
	}
}