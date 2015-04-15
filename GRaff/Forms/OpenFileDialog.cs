using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinOpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace GRaff.Forms
{
	public class OpenFileDialog
	{
		public static string Show()
		{
			var dialog = new WinOpenFileDialog();
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				return dialog.FileName;
			else
				return null;
			
		}

		public static string OpenImage()
		{
			string mask = "Image Files (*.bmp, *.gif, *.jpg, *.jpeg, *.png, *.tif, *.tiff)|*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.tif;*.tiff|All Files (*.*)|*.*";
			var dialog = new WinOpenFileDialog();
			dialog.Filter = mask;
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				return dialog.FileName;
			else
				return null;
		}
	}
}
