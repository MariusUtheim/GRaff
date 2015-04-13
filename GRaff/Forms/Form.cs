using System;


namespace GRaff.Forms
{
	public class Form : DisplayObject, IGlobalMousePressListener, IGlobalMouseReleaseListener
	{
		private Point _mouseLocation = Mouse.Location, _mousePrevious = Mouse.Location;

		public sealed override void OnStep()
		{
			_mousePrevious = _mouseLocation;
			_mouseLocation = Mouse.Location;

			if (_mouseLocation != _mousePrevious)
				onMouseMove(this, new FormEventArgs(), _mouseLocation, _mousePrevious);
		}

		public void OnGlobalMousePress(MouseButton button)
		{
			var globalLocation = Mouse.Location;
			if (Region.ContainsPoint(globalLocation))
				onMousePress(this, new MouseEventArgs(button, globalLocation), PointToLocal(globalLocation));
		}

		public void OnGlobalMouseRelease(MouseButton button)
		{
			var globalLocation = Mouse.Location;
			if (Region.ContainsPoint(globalLocation))
				onMouseRelease(this, new MouseEventArgs(button, globalLocation), PointToLocal(globalLocation));
		}

		public override void OnPaint()
		{
			Draw.FillRectangle(Color.Teal, Region);
		}
	}
}