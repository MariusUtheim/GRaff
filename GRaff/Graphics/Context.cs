using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;

namespace GRaff.Graphics
{
	public static class Context
	{

		public static bool IsActive
		{
			get { return GraphicsContext.CurrentContext != null; }
		}
	}
}
