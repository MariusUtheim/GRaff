using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public static class GlobalEvent
	{
		public static event EventHandler Step;
		public static event EventHandler BeginStep;
		public static event EventHandler EndStep;
		public static event EventHandler DrawBackground;
		public static event EventHandler DrawForeground;
		public static event EventHandler<KeyEventArgs> Key;
		public static event EventHandler<KeyEventArgs> KeyPressed;
		public static event EventHandler<KeyEventArgs> KeyReleased;
		public static event EventHandler<MouseEventArgs> Mouse;
		public static event EventHandler<MouseEventArgs> MousePressed;
		public static event EventHandler<MouseEventArgs> MouseReleased;

		internal static void OnBeginStep()
		{
			if (BeginStep != null)
				BeginStep.Invoke(null, new EventArgs());
		}

		internal static void OnStep()
		{
			if (Step != null)
				Step.Invoke(null, new EventArgs());
		}

		internal static void OnEndStep()
		{
			if (EndStep != null)
				EndStep.Invoke(null, new EventArgs());
		}


		internal static void OnKey(Key key)
		{
			if (Key != null)
				Key.Invoke(null, new KeyEventArgs(key));
		}

		internal static void OnKeyPressed(Key key)
		{
			if (KeyPressed != null)
				KeyPressed.Invoke(null, new KeyEventArgs(key));
		}

		internal static void OnKeyReleased(Key key)
		{
			if (KeyReleased != null)
				KeyReleased.Invoke(null, new KeyEventArgs(key));
		}

		internal static void OnDrawBackground()
		{
			if (DrawBackground != null)
				DrawBackground(null, new EventArgs());
		}

		internal static void OnDrawForeground()
		{
			if (DrawForeground != null)
				DrawForeground(null, new EventArgs());
		}

		internal static void OnMouse(MouseButton button)
		{
			if (Mouse != null)
				Mouse(null, new MouseEventArgs(button));
		}

		internal static void OnMousePressed(MouseButton button)
		{
			if (MousePressed != null)
				MousePressed(null, new MouseEventArgs(button));
		}

		internal static void OnMouseReleased(MouseButton button)
		{
			if (MouseReleased != null)
				MouseReleased(null, new MouseEventArgs(button));
		}

		private static void _ExitOnEscape(object sender, KeyEventArgs e)
		{
			if (e.Key == GRaff.Key.Escape)
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


		private static Exception _asyncException = null;

		internal static void OnAsyncException()
		{
			if (_asyncException != null)
				throw _asyncException;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="innerException"></param>
		public static void ThrowAsyncException(Exception innerException)
		{
			lock (_asyncException)
			{
				if (_asyncException == null)
					_asyncException = innerException;
			}
		}
	}
}
