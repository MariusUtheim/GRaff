using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public enum AlarmState
	{
		Running, Paused, Stopped
	}

	public sealed class Alarm
	{
		private static List<Alarm> _alarms = new List<Alarm>();

		public Alarm()
			:this(null) { }

		public Alarm(GameObject target)
		{
			this.Target = target;
			this.Count = -1;
			this.State = AlarmState.Stopped;
			this.IsLooping = false;
			this.InitialCount = -1;
		}

		public static Alarm Start(int count, Action<Alarm, GameObject> callback)
		{
			var alarm = new Alarm();
			alarm.Callback = callback;

			alarm.Restart(count);
			return alarm;
		}

		public static Alarm Start(int count, Action<Alarm> callback)
		{
			var alarm = new Alarm();
			alarm.Callback = (a, obj) => { callback(a); };
			alarm.Restart(count);
			return alarm;
		}

		internal static void TickAll()
		{
			foreach (var a in _alarms)
				a.Tick();
		}

		public GameObject Target { get; set; }

		public int InitialCount { get; set; }

		public int Count { get; set; }

		public Action<Alarm, GameObject> Callback { get; set; }

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
			Callback.Invoke(this, this.Target);
		}

		/// <summary>
		/// Triggers this alarm. If IsLooping is set to true it will loop; otherwise the countdown will be set to zero and it is stopped.
		/// </summary>
		public void Complete()
		{
			Callback.Invoke(this, this.Target);
			if (IsLooping)
				Count = InitialCount;
		}
	}
}
