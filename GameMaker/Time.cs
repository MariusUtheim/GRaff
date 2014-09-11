using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class Time
	{
		public static void Frame()
		{
			LoopCount++;
			_updateFps();
		}

		public static int LoopCount { get; private set; }

		public static int MachineTime
		{
			get { return Environment.TickCount; }
		}

		public static int StartTime { get; internal set; }

		public static int GameTime
		{
			get { return MachineTime - StartTime; }
		}



		private static int _fps = 0;
		private static double _fpsSeconds = 0;
		private static int _previousTick;
		private static void _updateFps()
		{
			int tick = Environment.TickCount;
			if (tick - _previousTick > 1000)
			{
				FPS = 0;
				_fpsSeconds = 0;
			}
			else
			{
				_fps++;
				_fpsSeconds += (tick - _previousTick) / 1000.0;
				if (_fpsSeconds > 1)
				{
					FPS = _fps;
					_fps = 0;
					_fpsSeconds %= 1;
				}
			}

			_previousTick = tick;
		}

		public static int FPS
		{
			get;
			private set;
		}
	}
}
