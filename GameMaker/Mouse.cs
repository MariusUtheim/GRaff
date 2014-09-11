using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class Mouse
	{
		private static readonly int _buttonCount = Enum.GetNames(typeof(MouseButton)).Length;
		public static bool[] _pressed = new bool[_buttonCount];
		public static bool[] _down = new bool[_buttonCount];
		public static bool[] _released = new bool[_buttonCount];

		internal static void Update()
		{
			for (int i = 0; i < _buttonCount; i++)
				_pressed[i] = _released[i] = false;
		}

		internal static void Press(MouseButton button)
		{
			if (!IsDown(button))
			{
				_down[(int)button] = true;
				_pressed[(int)button] = true;
			}
		}

		internal static void Release(MouseButton button)
		{
			if (IsDown(button))
			{
				_down[(int)button] = false;
				_released[(int)button] = true;
			}
		}
	
		public static double X
		{
			get { return View.X + WindowX / View.HZoom; }
		}

		public static double Y
		{
			get { return View.Y + WindowY / View.VZoom; }
		}

		public static Point Location
		{
			get { return new Point(X, Y); }
		}

		public static int WindowX { get; internal set; }

		public static int WindowY { get; internal set; }

		public static IntVector WindowLocation
		{
			get { return new IntVector(WindowX, WindowY); }
			set { WindowX = value.X; WindowY = value.Y; }
		}

		public static IEnumerable<MouseButton> Down
		{
			get 
			{
				for (int i = 0; i < _buttonCount; i++)
					if (_down[i])
						yield return (MouseButton)i;
			}
		}

		public static IEnumerable<MouseButton> Pressed
		{
			get
			{
				for (int i = 0; i < _buttonCount; i++)
					if (_pressed[i])
						yield return (MouseButton)i;
			}
		}

		public static IEnumerable<MouseButton> Released
		{
			get
			{
				for (int i = 0; i < _buttonCount; i++)
					if (_released[i])
						yield return (MouseButton)i;
			}
		}

		public static bool IsDown(MouseButton button)
		{
			return _down[(int)button];
		}

		public static bool IsPressed(MouseButton button)
		{
			return _pressed[(int)button];
		}

		public static bool IsReleased(MouseButton button)
		{
			return _released[(int)button];
		}

	}
}
