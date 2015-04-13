using System;
using System.Linq;

namespace GRaff.Forms
{
	public partial class DisplayObject : GameElement
	{
		public event EventHandler<MouseEventArgs> MouseDown;
		public event EventHandler<MouseEventArgs> MouseUp;
		public event EventHandler<FormEventArgs> MouseEnter;
		public event EventHandler<FormEventArgs> MouseLeave;

		internal bool onMousePress(object sender, MouseEventArgs e, Point location)
		{
			foreach (var child in _children.Where(c => c.Region.ContainsPoint(location)))
				if (child.onMousePress(sender, e, (Point)(location - child.Region.TopLeft)))
					return true;

			MouseDown?.Invoke(sender, e);

			return e.IsHandled;
		}

		internal bool onMouseRelease(object sender, MouseEventArgs e, Point location)
		{
			foreach (var child in _children.Where(c => c.Region.ContainsPoint(location)))
				if (child.onMouseRelease(sender, e, (Point)(location - child.Region.TopLeft)))
					return true;

			MouseUp?.Invoke(sender, e);

			return e.IsHandled;
		}

		internal bool onMouseMove(object sender, FormEventArgs e, Point location, Point previous)
		{
			var containsNow = Region.ContainsPoint(location);
			var containedPreviously = Region.ContainsPoint(previous);

			if (!containsNow && !containedPreviously)
				return false;

			foreach (var child in _children)
				if (child.onMouseMove(sender, e, PointToLocal(location), PointToLocal(previous)))
					return true;

			if (containsNow && !containedPreviously)
				MouseEnter?.Invoke(sender, e);
			else if (!containsNow)
				MouseLeave?.Invoke(sender, e);

			return e.IsHandled;
		}
	}
}