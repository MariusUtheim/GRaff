using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class Keyboard
	{
		private static readonly int _keyCount = Enum.GetNames(typeof(Key)).Length;
		private static bool[] _pressed = new bool[_keyCount];
		private static bool[] _down = new bool[_keyCount];
		private static bool[] _released = new bool[_keyCount];

		internal static void Update()
		{
			for (int i = 0; i < _keyCount; i++)
				_pressed[i] = _released[i] = false;
		}

		internal static void Press(Key key)
		{
			if (!IsDown(key))
			{
				_pressed[(int)key] = true;
				_down[(int)key] = true;
			}
		}

		internal static void Release(Key key)
		{
			if (IsDown(key))
			{
				_down[(int)key] = false;
				_released[(int)key] = true;
			}
		}


		public static IEnumerable<Key> Down
		{
			get 
			{
				for (int i = 0; i < _down.Length; i++)
					if (_down[i])
						yield return (Key)i;
			}
		}

		public static IEnumerable<Key> Pressed
		{
			get
			{
				for (int i = 0; i < _pressed.Length; i++)
					if (_pressed[i])
						yield return (Key)i;
			}
		}

		public static IEnumerable<Key> Released
		{
			get
			{
				for (int i = 0; i < _released.Length; i++)
					if (_released[i])
						yield return (Key)i;
			}
		}

		public static bool IsDown(Key key)
		{
			return _down[(int)key];
		}

		public static bool IsPressed(Key key)
		{
			return _pressed[(int)key];
		}

		public static bool IsReleased(Key key)
		{
			return _released[(int)key];
		}
	}
}
