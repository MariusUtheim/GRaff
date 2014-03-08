using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public interface IMouseListener
	{
		void OnMouse(MouseButton button);
	}

	public interface IMousePressListener
	{
		void OnMousePress(MouseButton button);
	}

	public interface IMouseReleaseListener
	{
		void OnMouseRelease(MouseButton button);
	}

	public interface IGlobalMouseListener
	{
		void OnGlobalMouse(MouseButton button);
	}

	public interface IGlobalMousePressListener
	{
		void OnGlobalMousePress(MouseButton button);
	}

	public interface IGlobalMouseReleaseListener
	{
		void OnGlobalMouseRelease(MouseButton button);
	}
}
