using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	/// <summary>
	/// Represents an ARGB color.
	/// </summary>
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public partial struct Color
	{
		byte _a;
		byte _r;
		byte _g;
		byte _b;

		/// <summary>
		/// Initializes a new instance of the GameMaker.Color class, using the specified RGBA values.
		/// </summary>
		/// <param name="r">The red channel.</param>
		/// <param name="g">The green channel.</param>
		/// <param name="b">The blue channel.</param>
		/// <param name="a">The alpha channel.</param>
		public Color(byte a, byte r, byte g, byte b)
			: this()
		{
			this._a = a;
			this._r = r;
			this._g = g;
			this._b = b;
		}

		/// <summary>
		/// Initializes a new instance of the GameMaker.Color class, using the specified ARGB values.
		/// </summary>
		/// <param name="r">The red channel.</param>
		/// <param name="g">The green channel.</param>
		/// <param name="b">The blue channel.</param>
		/// <param name="a">The alpha channel.</param>
		public Color(int r, int g, int b, int a)
			: this((byte)a, (byte)r, (byte)g, (byte)b) { }

		/// <summary>
		/// Initializes a new instance of the GameMaker.Color class, using the specified RGB values and an alpha value of 255.
		/// </summary>
		/// <param name="r">The red channel.</param>
		/// <param name="g">The green channel.</param>
		/// <param name="b">The blue channel.</param>
		public Color(byte r, byte g, byte b)
			: this(r, g, b, (byte)255) { }

		/// <summary>
		/// Initializes a new instance of the GameMaker.Color class, using the specified ARGB value in a 32-bit format.
		/// Colors can also be implicitly converted from ints.
		/// </summary>
		/// <param name="argb">The ARGB value of the created color.</param>
		public Color(int rgba)
			: this(rgba >> 24, (rgba >> 16) & 0xFF, (rgba >> 8) & 0xFF, rgba & 0xFF) { }

		/// <summary>
		/// Averages the specified colors, calculating the average of each channel separately.
		/// </summary>
		/// <param name="colors">An array of colors that will be merged.</param>
		/// <returns>The average of the specified colors.</returns>
		public static Color Merge(params Color[] colors)
		{
			int a = 0, r = 0, g = 0, b = 0;

			for (int i = 0; i < colors.Length; i++)
			{
				a += colors[i]._a;
				r += colors[i]._r;
				g += colors[i]._g;
				b += colors[i]._b;
			}

			return new Color(a / colors.Length, r / colors.Length, g / colors.Length, b / colors.Length);
		}

		/// <summary>
		/// Gets the alpha channel of this GameMaker.Color.
		/// </summary>
		public byte A { get { return _a; } }
		/// <summary>
		/// Gets the red channel of this GameMaker.Color.
		/// </summary>
		public byte R { get { return _r; } }
		/// <summary>
		/// Gets the green channel of this GameMaker.Color.
		/// </summary>
		public byte G { get { return _g; } }
		/// <summary>
		/// Gets the blue channel of this GameMaker.Color.
		/// </summary>
		public byte B { get { return _b; } }

		/// <summary>
		/// Returns this color as a 32-bit integer, in ARGB format.
		/// </summary>
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

		/// <summary>
		/// Creates a new GameMaker.Color, with the same color channels as this instance, but with the new specified alpha channel.
		/// </summary>
		/// <param name="alphaChannel">The alpha channel of the new color.</param>
		/// <returns>A new GameMaker.Color with the same color as this instance, but with the specified alpha channel.</returns>
		public Color Transparent(int alphaChannel)
		{
			return new Color(_r, _g, _b, (byte)alphaChannel);
		}

		/// <summary>
		/// Creates a new GameMaker.Color, with the same color channels as this instance, but iwth the new specified opacity.
		/// </summary>
		/// <param name="opacity">The opacity of the new color. 0.0 means it is completely transparent, and 1.0 means it is completely opaque.</param>
		/// <returns>A new GameMaker.Color with the same color as this instance, but with an alpha channel corresponding to the specified opacity.</returns>
		public Color Transparent(double opacity)
		{
			return new Color(_r, _g, _b, (byte)(255 * GMath.Median(0.0, opacity, 1.0)));
		}

		public static implicit operator Color(int argb)
		{
			return new Color(argb);
		}

		public bool Equals(Color color)
		{
			return _r == color._r && _g == color._g && _b == color._b && _a == color._a;
		}

		/// <summary>
		/// Specifies whether this GameMaker.Color contains the same ARGB value as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>True if obj is a GameMaker.Color and has the same ARGB value as this GameMaker.Color.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Color)
				return Equals((Color)obj);
			return base.Equals(obj);
		}

		/// <summary>
		/// Returns a hash code of this GameMaker.Color. The hash code is equal to the ARGB value.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GameMaker.Color.</returns>
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

		/// <summary>
		/// Converts this GameMaker.Color to a human-readable string, showing the value of each channel.
		/// </summary>
		/// <returns>A string that represents this GameMaker.Color</returns>
		public override string ToString()
		{
			return String.Format("Color ARGB=[{0}, {1}, {2}, {3}]", A, R, G, B);
		}

		internal OpenTK.Graphics.Color4 ToOpenGLColor()
		{
			return new OpenTK.Graphics.Color4(_r, _g, _b, _a);
		}
	}
}
