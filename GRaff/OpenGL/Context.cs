using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;

namespace GRaff.OpenGL
{
	public static class Context
	{

		public static bool IsAlive
		{
			get { return GraphicsContext.CurrentContext != null; }
		}
	}
}
