using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using WinMessageBox = System.Windows.Forms.MessageBox;

namespace GRaff.Forms
{
	public class MessageBox
	{
		public static void Show(string text)
		{
			WinMessageBox.Show(text);
		}
	}
}
