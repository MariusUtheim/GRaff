using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLColor = System.Drawing.Color;
using GLKey = OpenTK.Input.Key;
using GLMouseButton = OpenTK.Input.MouseButton;

namespace GameMaker.OpenGL
{
	public static class OpenGLExtensions
	{
		public static GLColor ToGLColor(this Color color)
		{
			return GLColor.FromArgb(color.A, color.R, color.G, color.B);
		}

		public static MouseButton ToGMMouseButton(this GLMouseButton button)
		{
			return (MouseButton)((int)button / 0x100000);
		}

		public static Key ToGMKey(this GLKey key)
		{
			return (Key)key;
		}
	}
}
