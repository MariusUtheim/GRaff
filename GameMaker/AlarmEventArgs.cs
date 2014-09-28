using System;

namespace GameMaker
{
	public class AlarmEventArgs : EventArgs
	{

		public AlarmEventArgs(Alarm alarm)
		{
			this.Alarm = alarm;
		}

		public Alarm Alarm { get; set; }
	}
}