using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	/// <summary>
	/// Specifies the state of an alarm
	/// </summary>
	public enum AlarmState
	{
		Running, Paused, Stopped
	}

	/// <summary>
	/// Ticks down every step, and fires an event when the 
	/// </summary>
	public sealed class Alarm
	{
		private static List<Alarm> _alarms = new List<Alarm>();

		public event EventHandler<AlarmEventArgs> Callback;

		public Alarm()
		{
		}

		public static Alarm Start(int count, EventHandler<AlarmEventArgs> action)
		{
			var alarm = new Alarm();
			alarm.Callback += action;

			alarm.Restart(count);
			return alarm;
		}


		internal static void TickAll()
		{
			foreach (var a in _alarms)
				a.Tick();
		}

		public int InitialCount { get; set; }

		public int Count { get; set; }


		public AlarmState State { get; private set; }

		public bool IsLooping { get; set; }

		public void Restart()
		{
			Count = InitialCount;
			Start();	
		}

		public void Restart(int count)
		{
			Count = InitialCount = count;
			Start();
		}

		public void Start()
		{
			if (Count > 0)
			{
				if (State != AlarmState.Running)
					_alarms.Add(this);
				State = AlarmState.Running;
			}
		}

		public void Stop()
		{
			if (State == AlarmState.Running)
				_alarms.Remove(this);
			Count = 0;
			State = AlarmState.Stopped;
		}


		public void Pause()
		{
			if (Count > 0)
			{
				State = AlarmState.Paused;
			}
		}

		/// <summary>
		/// Reduces the alarm by 1 if it is running.
		/// </summary>
		public void Tick()
		{
			if (State == AlarmState.Running)
			{
				Count--;
				if (Count == 0)
					Complete();
			}
		}

		/// <summary>
		/// Triggers this alarm once, without changing the countdown.
		/// </summary>
		public void Trigger()
		{
			Callback.Invoke(this, new AlarmEventArgs(this));
		}

		/// <summary>
		/// Triggers this alarm. If IsLooping is set to true it will loop; otherwise the countdown will be set to zero and it is stopped.
		/// </summary>
		public void Complete()
		{
			Callback.Invoke(this, new AlarmEventArgs(this));
			if (IsLooping)
				Count = InitialCount;
		}
	}
}
