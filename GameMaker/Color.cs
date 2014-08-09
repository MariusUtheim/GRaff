using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Color
	{
		byte _r;
		byte _g;
		byte _b;
		byte _a;

		public Color(byte r, byte g, byte b, byte a)
			: this()
		{
			this._a = a;
			this._r = r;
			this._g = g;
			this._b = b;
		}

		public Color(int r, int g, int b, int a)
			: this((byte)r, (byte)g, (byte)b, (byte)a) { }

		public Color(byte r, byte g, byte b)
			: this(r, g, b, (byte)255) { }

		public Color(int rgba)
			: this(rgba >> 24, (rgba >> 16) & 0xFF,(rgba >> 8) & 0xFF, rgba & 0xFF
			) { }

		public Color(int alpha, Color baseColor)
			: this(baseColor.R, baseColor.G, baseColor.B, alpha) { }
		
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

		public int A { get { return _a; } }
		public int R { get { return _r; } }
		public int G { get { return _g; } }
		public int B { get { return _b; } }

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

		public bool Equals(Color color)
		{
			return _r == color._r && _g == color._g && _b == color._b && _a == color._a;
		}

		public override bool Equals(object obj)
		{
			if (obj is Color)
				return Equals((Color)obj);
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return Argb;
		}

		public static bool operator ==(Color c1, Color c2)
		{
			return c1.Equals(c2);
		}

		public static bool operator !=(Color c1, Color c2)
		{
			return !c1.Equals(c2);
		}

		public override string ToString()
		{
			return String.Format("Color RGB=[{0}, {1}, {2}]", R, G, B);
		}

		internal OpenTK.Graphics.Color4 ToOpenGLColor()
		{
			return new OpenTK.Graphics.Color4(_r, _g, _b, _a);
		}
	}
}
