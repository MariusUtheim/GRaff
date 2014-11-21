using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES30;

namespace GRaff.Graphics
{
	internal static class _Initializer
	{
		public static void Initialize()
		{
			ShaderProgram.Current = ShaderProgram.DefaultColored;
			ColorMap.BlendMode = BlendMode.AlphaBlend;
			GL.Enable(EnableCap.Blend);
		}
	}
}
