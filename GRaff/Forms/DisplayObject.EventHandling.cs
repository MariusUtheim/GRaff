using System;
using System.Linq;

namespace GRaff.Forms
{
	public partial class DisplayObject : GameElement
	{
		public event EventHandler<MouseEventArgs> MouseDown;

		internal bool onMousePress(object sender, MouseEventArgs e, Point location)
		{
			foreach (var child in _children.Where(c => c.Region.ContainsPoint(location)))
				if (child.onMousePress(sender, e, (Point)(location - child.Region.TopLeft)))
					return true;

			if (MouseDown != null)
				MouseDown.Invoke(sender, e); /*C#6.0*/

			return e.IsHandled;
		}


	}
}