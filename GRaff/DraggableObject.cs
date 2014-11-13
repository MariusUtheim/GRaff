using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public abstract class DraggableObject : GameObject, IMousePressListener
	{
		private static DraggableObject _draggedObject = null;
		private static Vector _draggedOffset;
		private static MouseButton _dragButton = MouseButton.Left;

		static DraggableObject()
		{
			
			GlobalEvent.MouseReleased += (sender, e) => { if (e.Button == _dragButton) _draggedObject = null; };
			GlobalEvent.Step += delegate
			{
				if (_draggedObject != null)
				{
					Point previousLocation = _draggedObject.Location, newLocation = Mouse.Location + _draggedOffset;
					_draggedObject.Location = newLocation;
					_draggedObject.OnDrag(previousLocation, newLocation);
				}
			};
		}

		protected DraggableObject(double x, double y)
			: base(x, y) { }

		protected DraggableObject(Point location)
			: base(location) { }

		public virtual void OnDrag(Point from, Point to)
		{
		}

		public void OnMousePress(MouseButton button)
		{
			_draggedObject = this;
			_draggedOffset = this.Location - Mouse.Location;
		}
	}
}
