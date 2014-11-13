using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRaff
{
	/// <summary>
	/// Defines an action that is performed each step while a mouse button is held down and the mouse is hovering over the instance.
	/// Only subclasses of the GRaff.GameObject class can listen to this event, as GRaff.GameElement does not define a collision mask.
	/// </summary>
	public interface IMouseListener
	{
		/// <summary>
		/// An action that is performed for each mouse button that is being held down and the mouse is hovering over this instance.
		/// </summary>
		/// <param name="button">The button that is being held.</param>
		void OnMouse(MouseButton button);
	}

	/// <summary>
	/// Defines an action that is performed whenever a mouse button becomes pressed while hovering over the instance.
	/// Only subclasses of the GRaff.GameObject class can listen to this event, as GRaff.GameElement does not define a collision mask.
	/// </summary>
	public interface IMousePressListener
	{
		/// <summary>
		/// An action that is performed once for each mouse button that was pressed while hovering over this instance since last step. 
		/// </summary>
		/// <param name="button">The mouse button that was pressed.</param>
		void OnMousePress(MouseButton button);
	}

	/// <summary>
	/// Defines an action that is performed whenever a mouse button becomes released while hovering over the instance.
	/// Only subclasses of the GRaff.GameObject class can listen to this event, as GRaff.GameElement does not define a collision mask.
	/// </summary>
	public interface IMouseReleaseListener
	{
		/// <summary>
		/// An action that is performed once for each mouse button that was released while hovering over this instance since last step. 
		/// </summary>
		/// <param name="button">The mouse button that was released.</param>
		void OnMouseRelease(MouseButton button);
	}

	/// <summary>
	/// Defines an action that is performed each step while a mouse button is being held down.
	/// </summary>
	public interface IGlobalMouseListener
	{
		/// <summary>
		/// An action that is performed once for each mouse button that is being held. 
		/// </summary>
		/// <param name="button">The mouse button that is being held.</param>
		void OnGlobalMouse(MouseButton button);
	}

	/// <summary>
	/// Defines an action that is performed whenever a mouse button becomes pressed.
	/// </summary>
	public interface IGlobalMousePressListener
	{
		/// <summary>
		/// An action that is performed once for each mouse button that was pressed since last step. 
		/// </summary>
		/// <param name="button">The mouse button that was pressed.</param>
		void OnGlobalMousePress(MouseButton button);
	}

	/// <summary>
	/// Defines an action that is performed whenever a mouse button becomes released.
	/// </summary>
	public interface IGlobalMouseReleaseListener
	{
		/// <summary>
		/// An action that is performed once for each mouse button that was released since last step. 
		/// </summary>
		/// <param name="button">The mouse button that was released.</param>
		void OnGlobalMouseRelease(MouseButton button);
	}
}
