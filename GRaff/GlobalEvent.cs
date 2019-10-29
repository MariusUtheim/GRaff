using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public static class GlobalEvent
	{
		public static event Action? Step;
		public static event Action? BeginStep;
		public static event Action? EndStep;
		public static event Action? DrawBackground;
		public static event Action? DrawForeground;
		public static event Action<Key>? Key;
		public static event Action<Key>? KeyPressed;
		public static event Action<Key>? KeyReleased;
		public static event Action<MouseButton>? Mouse;
		public static event Action<MouseButton>? MousePressed;
		public static event Action<MouseButton>? MouseReleased;
        public static event Action<double>? MouseWheel;

		internal static void OnBeginStep() => BeginStep?.Invoke();

		internal static void OnStep() => Step?.Invoke();

		internal static void OnEndStep() => EndStep?.Invoke();

		internal static void OnKey(Key key) => Key?.Invoke(key);

		internal static void OnKeyPressed(Key key) => KeyPressed?.Invoke(key);

        internal static void OnKeyReleased(Key key) => KeyReleased?.Invoke(key);

		internal static void OnDrawBackground() => DrawBackground?.Invoke();
    
		internal static void OnDrawForeground() => DrawForeground?.Invoke();

		internal static void OnMouse(MouseButton button) => Mouse?.Invoke(button);

        internal static void OnMousePressed(MouseButton button) => MousePressed?.Invoke(button);

        internal static void OnMouseReleased(MouseButton button) => MouseReleased?.Invoke(button);

        internal static void OnMouseWheel(double delta) => MouseWheel?.Invoke(delta);

		private static void _ExitOnEscape(Key key)
		{
			if (key == GRaff.Key.Escape)
				Game.Quit();
		}

		private static bool _willExitOnEscape = false;
		public static bool ExitOnEscape
		{
			get { return _willExitOnEscape; }

			set
			{
				if (value && !_willExitOnEscape)
				{
					KeyPressed += _ExitOnEscape;
					_willExitOnEscape = true;
				}
				else if (!value && _willExitOnEscape)
				{
					KeyPressed -= _ExitOnEscape;
					_willExitOnEscape = false;
				}
			}
		}
	}
}
