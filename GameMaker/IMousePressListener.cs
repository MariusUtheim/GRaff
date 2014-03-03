using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public interface IMousePressListener
	{
		void OnMousePress(MouseButton button);
	}
}
