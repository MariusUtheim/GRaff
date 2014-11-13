using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	/// <summary>
	/// Defines an action that is performed each step while a key is held down.
	/// </summary>
	public interface IKeyListener
	{
		/// <summary>
		/// An action that is performed for each key that is being held down.
		/// </summary>
		/// <param name="key">The key that is being held.</param>
		void OnKey(Key key);
	}

	/// <summary>
	/// Defines an action that is performed whenever a key becomes pressed.
	/// </summary>
	public interface IKeyPressListener
	{
		/// <summary>
		/// An action that is performed once for each key that was pressed since the last step.
		/// </summary>
		/// <param name="key">The key that was pressed.</param>
		void OnKeyPress(Key key);
	}

	/// <summary>
	/// Defines an action that is performed whenever a key becomes released.
	/// </summary>
	public interface IKeyReleaseListener
	{
		/// <summary>
		/// An action that is performed once for each key that was released since the last step.
		/// </summary>
		/// <param name="key">The key that was released.</param>
		void OnKeyRelease(Key key);
	}
}
