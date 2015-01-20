using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Graphics
{
	[Pure]
	public unsafe sealed class ColorMatrix
	{
		private float* _r0;
		private float* _r1;
		private float* _r2;
		private float* _r3;


		
		public float M00 { get { return _r1[0]; } set { _r1[0] = value; } }
		public float M01 { get { return _r1[1]; } set { _r1[1] = value; } }
		public float M02 { get { return _r1[2]; } set { _r1[2] = value; } }
		public float M03 { get { return _r1[3]; } set { _r1[3] = value; } }
		public float M10 { get { return _r2[0]; } set { _r2[0] = value; } }
		public float M11 { get { return _r2[1]; } set { _r2[1] = value; } }
		public float M12 { get { return _r2[2]; } set { _r2[2] = value; } }
		public float M13 { get { return _r2[3]; } set { _r2[3] = value; } }
		public float M20 { get { return _r3[0]; } set { _r3[0] = value; } }
		public float M21 { get { return _r3[1]; } set { _r3[1] = value; } }
		public float M22 { get { return _r3[2]; } set { _r3[2] = value; } }
		public float M23 { get { return _r3[3]; } set { _r3[3] = value; } }
		
		public static Color operator *(ColorMatrix m, Color c)
		{
			return new Color(c.A,
				(int)(m.M00 * c.R + m.M01 * c.G + m.M02 * c.B + m.M03),
				(int)(m.M10 * c.R + m.M11 * c.G + m.M12 * c.B + m.M13),
				(int)(m.M20 * c.R + m.M21 * c.G + m.M22 * c.B + m.M23));
		}
	}
}
