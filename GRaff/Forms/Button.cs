using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Forms
{
	public class Button : Quad
	{
		private bool _isHovering, _isPressed;
		public event EventHandler<FormEventArgs> Click;

		public Button()
		{
			Color = Colors.LightGray;
			MouseEnter += (_, e) => { _isHovering = true; };
			MouseLeave += (_, e) => { _isHovering = false; };
			MouseDown += (_, e) => { if (e.Button == MouseButton.Left) _isPressed = true; };
			MouseUp += (sender, e) => {
				if (e.Button == MouseButton.Left && _isPressed)
				{
					Console.WriteLine(_isPressed);
					Console.WriteLine(_isHovering);
                    if (Click != null)
						Click.Invoke(sender, e);
				}
			};
		}
	}
}
