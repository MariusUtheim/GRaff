using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
#if OpenGL4
using coord = System.Double;
#else
using coord = System.Float;
#endif

namespace GRaff.Graphics
{
#warning Make public
    [StructLayout(LayoutKind.Sequential)]
	internal struct Quadrilateral
	{
		public readonly coord V1;
		public readonly coord V2;
		public readonly coord V3;
		public readonly coord V4;
		public readonly coord V5;
		public readonly coord V6;
		public readonly coord V7;
		public readonly coord V8;

		public Quadrilateral(coord v1, coord v2, coord v3, coord v4, coord v5, coord v6, coord v7, coord v8) : this()
		{
			this.V1 = v1;
			this.V2 = v2;
			this.V3 = v3;
			this.V4 = v4;
			this.V5 = v5;
			this.V6 = v6;
			this.V7 = v7;
			this.V8 = v8;
		}
	}
}
