using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public partial struct Color
	{
		public Color(int a, int r, int g, int b)
			: this()
		{
			this.A = a;
			this.R = r;
			this.G = g;
			this.B = b;
		}

		public Color(byte r, byte g, byte b)
			: this(255, r, g, b) { }

		public Color(int argb)
			: this()
		{
			this.A = (argb >> 24) & 0xFF;
			this.R = (argb >> 16) & 0xFF;
			this.G = (argb >> 8) & 0xFF;
			this.B = argb & 0xFF;
		}
		
		public static Color Merge(params Color[] colors)
		{
			int a = 0, r = 0, g = 0, b = 0;
			
			for (int i = 0; i < colors.Length; i++)
			{
				a += colors[i].A;
				r += colors[i].R;
				g += colors[i].G;
				b += colors[i].B;
			}

			return new Color(a / colors.Length, r / colors.Length, g / colors.Length, b / colors.Length);
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
