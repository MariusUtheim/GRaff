using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinColor = System.Drawing.Color;
using WinKey = System.Windows.Forms.Keys;
using WinMouseButton = System.Windows.Forms.MouseButtons;

namespace GameMaker.Forms
{
	public static class GMFormsExtensions
	{
		public static WinColor ToFormsColor(this Color color)
		{
			return WinColor.FromArgb(GMath.Median(0, color.A, 255), GMath.Median(0, color.R, 255), GMath.Median(0, color.G, 255), GMath.Median(0, color.B, 255));
		}

		public static Color ToGMColor(this WinColor color)
		{
			return new Color(color.R, color.G, color.B);
		}

		public static MouseButton ToGMMouseButton(this WinMouseButton button)
		{
			return (MouseButton)((int)button / 0x100000);
		}

		public static Key ToGMKey(this WinKey key)
		{
			return (Key)key;
		}
	}
}
