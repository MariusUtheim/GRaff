using System;

namespace GRaff
{
	/// <summary>
	/// Provides methods for dealing with time.
	/// </summary>
	public static class Time
	{
        private static int _previousLoopTick = -1;
        internal static void Loop()
        {
            var time = Environment.TickCount;
            if (_previousLoopTick == -1)
                Delta = 0;
            else
                Delta = time - _previousLoopTick;
            _previousLoopTick = time;
            LoopCount++;
        }

		/// <summary>
		/// Gets the number of steps that has occurred since the game started.
		/// </summary>
		public static int LoopCount
		{
			get;
			private set;
		}

        /// <summary>
        /// Gets the number of milliseconds since the previous step. If the game is running at full fps and a 
        /// constant loop rate, this value should remain approximately constant. 
        /// </summary>
        public static int Delta { get; private set; }

		/// <summary>
		/// Gets the number of milliseconds since the computer started.
		/// </summary>
		public static int MachineTime { get { return Environment.TickCount; } }

		/// <summary>
		/// Gets the machine time at the time the game was started.
		/// </summary>
		public static int StartTime { get; internal set; }

		/// <summary>
		/// Gets the number of milliseconds since the game started.
		/// </summary>
		public static int GameTime { get { return MachineTime - StartTime; } }
		

		private static int _fps;
		private static int _currentFps = 0;
		private static double _fpsSeconds = 0;
		private static int _previousFrameTick;
		internal static void UpdateFps()
		{
			int tick = Environment.TickCount;
			if (tick - _previousFrameTick > 1000)
			{
				_fps = 0;
				_fpsSeconds = 0;
			}
			else
			{
				_currentFps++;
				_fpsSeconds += (tick - _previousFrameTick) / 1000.0;
				if (_fpsSeconds > 1)
				{
					_fps = _currentFps;
					_currentFps = 0;
					_fpsSeconds %= 1;
				}
			}

			_previousFrameTick = tick;
		}

		/// <summary>
		/// Gets the numer of frames per second at which the game is actually running.
		/// </summary>
		public static int FPS { get { return _fps; } }
	}
}
