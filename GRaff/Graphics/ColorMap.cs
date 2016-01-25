using System;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif

namespace GRaff.Graphics
{
	public static class ColorMap
	{
		
		public static BlendMode BlendMode
		{
			get
			{
				return new BlendMode((BlendEquation)GL.GetInteger(GetPName.BlendEquationRgb), (BlendingFactor)GL.GetInteger(GetPName.BlendSrc), (BlendingFactor)GL.GetInteger(GetPName.BlendDst));
			}

			set
			{
				GL.BlendFunc((BlendingFactorSrc)value.SourceFactor, (BlendingFactorDest)value.DestinationFactor);
				GL.BlendEquation((BlendEquationMode)value.Equation);
			}
		}

		public static BlendEquation BlendEquation
		{
			get
			{
				return (BlendEquation)GL.GetInteger(GetPName.BlendEquationRgb);
			}

			set
			{
				GL.BlendEquation((BlendEquationMode)value);
			}
		}
		
		
		
	}
}
