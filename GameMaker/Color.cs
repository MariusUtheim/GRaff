using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public partial struct Color
	{

		public Color(int a, int r, int g, int b)
			:this()
		{
			this.A = a;
			this.R = r;
			this.G = g;
			this.B = b;
		}

		public Color(byte r, byte g, byte b)
			: this(255, r, g, b) { }

		public Color(int argb)
		{
			throw new NotImplementedException();
		}

		public int A { get; set; }
		public int R { get; set; }
		public int G { get; set; }
		public int B { get; set; }

		public int Argb
		{
			get
			{
				return A << 24
					 | R << 16
					 | G << 8
					 | B;
			}
		}
	}
}
