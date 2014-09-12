using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	/// <summary>
	/// Provides static methods for interacting with the keyboard.
	/// </summary>
	public static class Keyboard
	{
		private static readonly int _keyCount = 255;//Enum.GetNames(typeof(Key)).Length;
		private static bool[] _pressed = new bool[_keyCount];
		private static bool[] _down = new bool[_keyCount];
		private static bool[] _released = new bool[_keyCount];

		/// <summary>
		/// Signals that a step has occurred, and the key states (held/pressed/released) should be updated.
		/// </summary>
		internal static void Update()
		{
			for (int i = 0; i < _keyCount; i++)
				_pressed[i] = _released[i] = false;
		}

		/// <summary>
		/// Signals that the specified key was pressed.
		/// </summary>
		/// <param name="key">The pressed key.</param>
		internal static void Press(Key key)
		{
			if (!IsDown(key))
			{
				_pressed[(int)key] = true;
				_down[(int)key] = true;
			}
		}

		/// <summary>
		/// Signals that the specified key was released.
		/// </summary>
		/// <param name="key">The released key.</param>
		internal static void Release(Key key)
		{
			if (IsDown(key))
			{
				_down[(int)key] = false;
				_released[(int)key] = true;
			}
		}

		/// <summary>
		/// Returns all keys that are currently held down.
		/// </summary>
		public static IEnumerable<Key> Down
		{
			get 
			{
				for (int i = 0; i < _down.Length; i++)
					if (_down[i])
						yield return (Key)i;
			}
		}

		/// <summary>
		/// Returns the keys that were pressed since the last step.
		/// </summary>
		public static IEnumerable<Key> Pressed
		{
			get
			{
				for (int i = 0; i < _pressed.Length; i++)
					if (_pressed[i])
						yield return (Key)i;
			}
		}

		/// <summary>
		/// Returns the keys that were released since the last step.
		/// </summary>
		public static IEnumerable<Key> Released
		{
			get
			{
				for (int i = 0; i < _released.Length; i++)
					if (_released[i])
						yield return (Key)i;
			}
		}

		/// <summary>
		/// Returns whether the specified key is currently held down.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>True if the key is currently held down.</returns>
		public static bool IsDown(Key key)
		{
			return _down[(int)key];
		}

		/// <summary>
		/// Returns whether the specified key was pressed since the previous step.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>True if the key was pressed since the previous step.</returns>
		public static bool IsPressed(Key key)
		{
			return _pressed[(int)key];
		}

		/// <summary>
		/// Returns whether the specified key was released since the previous step.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>True if the key was released since the previous step.</returns>
		public static bool IsReleased(Key key)
		{
			return _released[(int)key];
		}
	}
}
