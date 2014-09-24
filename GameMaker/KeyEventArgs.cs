using System;

namespace GameMaker
{
	/// <summary>
	/// Defines the event data for a keyboard event.
	/// </summary>
	public class KeyEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the GameMaker.KeyEventArgs class using the specified GameMaker.Key.
		/// </summary>
		/// <param name="button">The GameMaker.Key associated with the event.</param>
		public KeyEventArgs(Key key)
		{
			this.Key = key;
		}

		/// <summary>
		/// Gets the GameMaker.Key associated with the event.
		/// </summary>
		public Key Key { get; private set; }
	}
}