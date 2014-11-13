using System;


namespace GRaff
{
	/// <summary>
	/// Provides methods for dealing with time.
	/// </summary>
	public static class Time
	{
		/// <summary>
		/// Indicates that a frame has occurred, for the purpose of calculations such as FPS.
		/// </summary>
		public static void Frame()
		{
			_loopCount++;
			_updateFps();
		}


		private static int _loopCount = 0;
		/// <summary>
		/// Gets the number of steps that has occurred since the game started.
		/// </summary>
		public static int LoopCount()
		{
			return _loopCount;
		}

		/// <summary>
		/// Gets the number of milliseconds since the computer started.
		/// </summary>
		public static int MachineTime()
		{
			return Environment.TickCount;
		}

		/// <summary>
		/// Gets the machine time at the time the game was started.
		/// </summary>
		public static int StartTime { get; internal set; }

		/// <summary>
		/// Gets the number of milliseconds since the game started.
		/// </summary>
		public static int GameTime()
		{
			return MachineTime() - StartTime;
		}

		private static int _fps;
		private static int _currentFps = 0;
		private static double _fpsSeconds = 0;
		private static int _previousTick;
		private static void _updateFps()
		{
			int tick = Environment.TickCount;
			if (tick - _previousTick > 1000)
			{
				_fps = 0;
				_fpsSeconds = 0;
			}
			else
			{
				_currentFps++;
				_fpsSeconds += (tick - _previousTick) / 1000.0;
				if (_fpsSeconds > 1)
				{
					_fps = _currentFps;
					_currentFps = 0;
					_fpsSeconds %= 1;
				}
			}

			_previousTick = tick;
		}

		/// <summary>
		/// Gets the numer of frames per second at which the game is actually running.
		/// </summary>
		public static int FPS()
		{
			return _fps;
		}
	}
}
