using System;


namespace GRaff.Forms
{
	public class Form : DisplayObject, IGlobalMousePressListener
	{

		public void OnGlobalMousePress(MouseButton button)
		{
			var globalLocation = Mouse.Location;
			if (Region.ContainsPoint(globalLocation))
				onMousePress(this, new MouseEventArgs(button, globalLocation), ToLocal(globalLocation));
		}

		public override void OnPaint()
		{
			Draw.FillRectangle(Color.Teal, Region);
		}
	}
}