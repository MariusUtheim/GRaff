using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	/// <summary>
	/// Provides static methods for interacting with the mouse.
	/// </summary>
	public static class Mouse
	{
		private static readonly int _buttonCount = Enum.GetNames(typeof(MouseButton)).Length;
		private static bool[] _pressed = new bool[_buttonCount];
		private static bool[] _down = new bool[_buttonCount];
		private static bool[] _released = new bool[_buttonCount];
		
		/// <summary>
		/// Signals that a step has occurred, and the button states (held/pressed/released) should be updated.
		/// </summary>
		internal static void Update()
		{
			XPrevious = X;
			YPrevious = Y;
			for (int i = 0; i < _buttonCount; i++)
				_pressed[i] = _released[i] = false;
            WheelDelta = 0;
		}

		/// <summary>
		/// Signals that the specified button was pressed.
		/// </summary>
		/// <param name="button">The pressed button.</param>
		internal static void Press(MouseButton button)
		{
			if (!IsDown(button))
			{
				_down[(int)button] = true;
				_pressed[(int)button] = true;
			}
		}

		/// <summary>
		/// Signals that the specified button was released.
		/// </summary>
		/// <param name="button">The released button.</param>
		internal static void Release(MouseButton button)
		{
			if (IsDown(button))
			{
				_down[(int)button] = false;
				_released[(int)button] = true;
			}
		}

        internal static void Wheel(float value, float delta)
        {

        }
	
		/// <summary>
		/// Gets the x-coordinate of the previous position of the cursor in the room. This value might be non-integer if the View is scaled.
		/// </summary>
		public static double XPrevious { get; private set; }

		/// <summary>
		/// Gets the y-coordinate of the previous position of the cursor in the room. This value might be non-integer if the View is scaled.
		/// </summary>
		public static double YPrevious { get; private set; }

        /// <summary>
        /// Gets the current value of the mouse wheel.
        /// </summary>
        public static double WheelValue { get; private set; }

        /// <summary>
        /// Gets the change in the mouse wheel value from the last step.
        /// </summary>
        public static double WheelDelta { get; private set; }

		/// <summary>
		/// Gets the previous location of the cursor in the room. The coordinates might be non-integer if the View is scaled.
		/// </summary>
		public static Point LocationPrevious
		{
			get
			{
				return new Point(XPrevious, YPrevious);
			}
		}

		/// <summary>
		/// Gets the x-coordinate of the cursor in the room. This value might be non-integer if the View is scaled.
		/// </summary>
		public static double X
		{
			get { return Location.X; }
		}

		/// <summary>
		/// Gets the y-coordinate of the cursor in the room. This value might be non-integer if the View is scaled.
		/// </summary>
		public static double Y
		{
			get { return Location.Y; }
		}

		/// <summary>
		/// Gets the location of the cursor in the room. The coordinates might be non-integer if the View is scaled.
		/// </summary>
		public static Point Location
		{
			get { return View.ScreenToRoom(WindowX, WindowY); }
		}

		/// <summary>
		/// Gets the x-coordinate of the cursor relative to the game window.
		/// </summary>
		public static int WindowX { get; internal set; }

		/// <summary>
		/// Gets the y-coordinate of the cursor relative to the game window.
		/// </summary>
		public static int WindowY { get; internal set; }

		/// <summary>
		/// Gets the location of the cursor relative to the game window.
		/// </summary>
		public static IntVector WindowLocation
		{
			get { return new IntVector(WindowX, WindowY); }
			internal set { WindowX = value.X; WindowY = value.Y; }
		}

		/// <summary>
		/// Returns all the keys buttons that are currently held down.
		/// </summary>
		public static IEnumerable<MouseButton> Down
		{
			get 
			{
				for (int i = 0; i < _buttonCount; i++)
					if (_down[i])
						yield return (MouseButton)i;
			}
		}

		/// <summary>
		/// Returns the buttons that were pressed since the last step.
		/// </summary>
		public static IEnumerable<MouseButton> Pressed
		{
			get
			{
				for (int i = 0; i < _buttonCount; i++)
					if (_pressed[i])
						yield return (MouseButton)i;
			}
		}

		/// <summary>
		/// Returns the buttons that were released since the last step.
		/// </summary>
		public static IEnumerable<MouseButton> Released
		{
			get
			{
				for (int i = 0; i < _buttonCount; i++)
					if (_released[i])
						yield return (MouseButton)i;
			}
		}

		/// <summary>
		/// Returns whether the specified button is currently held down.
		/// </summary>
		/// <param name="button">The button to check.</param>
		/// <returns>true if the button is currently held down.</returns>
		public static bool IsDown(MouseButton button)
		{
			return _down[(int)button];
		}

		/// <summary>
		/// Returns whether the specified button was pressed since the previous step.
		/// </summary>
		/// <param name="button">The button to check.</param>
		/// <returns>true if the button was pressed since the previous step.</returns>
		public static bool IsPressed(MouseButton button)
		{
			return _pressed[(int)button];
		}

		/// <summary>
		/// Returns whether the specified button was released since the previous step.
		/// </summary>
		/// <param name="button">The button to check.</param>
		/// <returns>true if the button was pressed since the previous step.</returns>
		public static bool IsReleased(MouseButton button)
		{
			return _released[(int)button];
		}

	}
}
