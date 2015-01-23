using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES30;

namespace GRaff.Graphics
{
	/// <summary>
 /// Provides static methods for performing scissor testing. When scissor testing is active,
 /// pixels outside a specified region will not be drawn.
 /// </summary>
	public static class Scissor
	{
		public static bool IsEnabled
		{
			get
			{
				return GL.GetBoolean(GetPName.ScissorTest);
			}

			set
			{
				if (value)
					GL.Enable(EnableCap.ScissorTest);
				else
					GL.Disable(EnableCap.ScissorTest);
			}
		}

		public static IntRectangle Region
		{
			get
			{
				int[] scissorCoords = new int[4];
				GL.GetInteger(GetPName.ScissorBox, scissorCoords);
				return new IntRectangle(scissorCoords[0], Window.Height - scissorCoords[1], scissorCoords[2], scissorCoords[3]);
			}

			set
			{
				GL.Scissor(value.Left, Window.Height - value.Bottom, value.Width, value.Height);
            }
		}
	}
}
