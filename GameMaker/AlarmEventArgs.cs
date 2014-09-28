using System;

namespace GameMaker
{
	/// <summary>
	/// Defines event data for an alarm event.
	/// </summary>
	public class AlarmEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the GameMaker.AlarmEventArgs class using the specified GameMaker.Alarm.
		/// </summary>
		/// <param name="alarm">The alarm associated with the event.</param>
		public AlarmEventArgs(Alarm alarm)
		{
			this.Alarm = alarm;
		}

		/// <summary>
		/// Gets the GameMaker.Alarm associated with the event.
		/// </summary>
		public Alarm Alarm { get; private set; }
	}
}