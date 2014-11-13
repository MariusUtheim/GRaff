using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.OpenGL
{
	internal static class _Initializer
	{
		public static void Initialize()
		{
			ShaderProgram.Current = ShaderProgram.DefaultColored;
		}
	}
}
