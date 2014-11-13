using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	/// <summary>
	/// Specifies the state of an alarm
	/// </summary>
	public enum AlarmState
	{
		Running, Paused, Stopped
	}

	/// <summary>
	/// Defines event data for an alarm event.
	/// </summary>
	public sealed class AlarmEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the GRaff.AlarmEventArgs class using the specified GRaff.Alarm.
		/// </summary>
		/// <param name="alarm">The alarm associated with the event.</param>
		public AlarmEventArgs(Alarm alarm)
		{
			this.Alarm = alarm;
		}

		/// <summary>
		/// Gets the GRaff.Alarm associated with the event.
		/// </summary>
		public Alarm Alarm { get; private set; }
	}

	/// <summary>
	/// Ticks down every step, and fires an event when the tick reaches zero.
	/// </summary>
	public sealed class Alarm : GameElement
	{
		public event EventHandler<AlarmEventArgs> Callback;

		public static Alarm Start(int count, bool isLooping, EventHandler<AlarmEventArgs> action)
		{
			var alarm = new Alarm();
			alarm.Callback += action;
			alarm.IsLooping = isLooping;
			alarm.Restart(count);
			return alarm;
		}

		/// <summary>
		/// Gets or sets the initial count of this GRaff.Alarm. This will contain the value passed to GRaff.Alarm.Restart(int)
		/// </summary>
		public int InitialCount { get; set; }

		/// <summary>
		/// Gets or sets the number of ticks remaining until this GRaff.Alarm fires the alarm event.
		/// </summary>
		public int Count { get; set; }

		/// <summary>
		/// Gets the state of this GRaff.Alarm.
		/// </summary>
		public AlarmState State { get; private set; }

		/// <summary>
		/// Gets or sets whether this GRaff.Alarm should be automatically loop.
		/// Whenever the countdown for this GRaff.Alarm reaches zero, if IsLooping is set to true, the countdown is reset to the value of InitialCount.
		/// </summary>
		public bool IsLooping { get; set; }

		/// <summary>
		/// Resets the countdown of this GRaff.Alarm to the value of InitialCount.
		/// If InitialCount is less than or equal to zero, this GRaff.Alarm is stopped instead.
		/// </summary>
		public void Restart()
		{
			Count = InitialCount;
			Start();	
		}

		/// <summary>
		/// Resets the countdown of this GRaff.Alarm to the specified initial count.
		/// If count is less than or equal to zero, this GRaff.Alarm is stopped instead.
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
				State = AlarmState.Running;
			}
		}

		public void Stop()
		{
			Count = 0;
			State = AlarmState.Stopped;
		}


		public void Pause()
		{
			if (Count > 0)
				State = AlarmState.Paused;
		}

		/// <summary>
		/// Reduces the countdown of this GRaff.Alarm if it is running, and fires the alarm event if countdown reaches zero.
		/// </summary>
		public override void OnStep()
		{
			if (State == AlarmState.Running)
			{
				Count--;
				if (Count == 0)
					Complete();
			}
		}

		/// <summary>
		/// Fires the alarm event of this GRaff.Alarm without changing the countdown.
		/// </summary>
		public void Trigger()
		{
			Callback.Invoke(this, new AlarmEventArgs(this));
		}

		/// <summary>
		/// Fires the alarm event of this GRaff.Alarm. If IsLooping is set to true, the countdown is reset to InitialValue; otherwise the countdown will be set to zero and it is stopped.
		/// </summary>
		public void Complete()
		{
			Callback.Invoke(this, new AlarmEventArgs(this));
			if (IsLooping)
				Count = InitialCount;
		}
	}
}
