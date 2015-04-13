using System;

namespace GRaff.Forms
{
	public class FormEventArgs : EventArgs
	{
		public bool IsHandled { get; set; } = false;
	}
}