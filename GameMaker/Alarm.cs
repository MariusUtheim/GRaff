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
	/// Ticks down every step, and fires an event when the tick reaches zero.
	/// </summary>
	public sealed class Alarm
	{
		private static List<Alarm> _alarms = new List<Alarm>();

		public event EventHandler<AlarmEventArgs> Callback;

		public Alarm()
		{
			_alarms.Add(this);
		}

		~Alarm()
		{
			_alarms.Remove(this);
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

		/// <summary>
		/// Gets or sets the initial count of this GameMaker.Alarm. This will contain the value passed to GameMaker.Alarm.Restart(int)
		/// </summary>
		public int InitialCount { get; set; }

		/// <summary>
		/// Gets or sets the number of ticks remaining until this GameMaker.Alarm fires the alarm event.
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// Gets the state of this GameMaker.Alarm.
		/// </summary>
		public AlarmState State { get; private set; }

		/// <summary>
		/// Gets or sets whether this GameMaker.Alarm should be automatically loop.
		/// Whenever the countdown for this GameMaker.Alarm reaches zero, if IsLooping is set to true, the countdown is reset to the value of InitialCount.
		/// </summary>
		public bool IsLooping { get; set; }

		/// <summary>
		/// Resets the countdown of this GameMaker.Alarm to the value of InitialCount.
		/// If InitialCount is less than or equal to zero, this GameMaker.Alarm is stopped instead.
		/// </summary>
		public void Restart()
		{
			Count = InitialCount;
			Start();	
		}

		/// <summary>
		/// Resets the countdown of this GameMaker.Alarm to the specified initial count.
		/// If count is less than or equal to zero, this GameMaker.Alarm is stopped instead.
		/// </summary>
		/// <param name="count">The initial countdown.</param>
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
		/// Reduces the countdown of this GameMaker.Alarm if it is running, and fires the alarm event if countdown reaches zero.
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
		/// Fires the alarm event of this GameMaker.Alarm without changing the countdown.
		/// </summary>
		public void Trigger()
		{
			Callback.Invoke(this, new AlarmEventArgs(this));
		}

		/// <summary>
		/// Fires the alarm event of this GameMaker.Alarm. If IsLooping is set to true, the countdown is reset to InitialValue; otherwise the countdown will be set to zero and it is stopped.
		/// </summary>
		public void Complete()
		{
			Callback.Invoke(this, new AlarmEventArgs(this));
			if (IsLooping)
				Count = InitialCount;
		}
	}
}
