using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics
{
	public static class GLError
	{

		public static void Check()
		{
			var err = GL.GetError();
			if (err != ErrorCode.NoError)
				throw new Exception($"An OpenGL operation threw an exception with error code {Enum.GetName(typeof(ErrorCode), err)}");
        }
	}
}
