using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GRaff
{
	/// <summary>
	/// Represents an RGBA color. Note that colors can be cast from uint structures on the form 0xRRGGBBAA.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Color : IEquatable<Color>
	{
		private readonly byte _r, _g, _b, _a;

		/// <summary>
		/// Initializes a new instance of the GRaff.Color structure using the specifed alpha, red, green and blue values.
		/// </summary>
		/// <param name="a">The alpha channel.</param>
		/// <param name="r">The red channel.</param>
		/// <param name="g">The green channel.</param>
		/// <param name="b">The blue channel.</param>
		public Color(byte r, byte g, byte b, byte a)
			: this()
		{
			_a = a;
			_r = r;
			_g = g;
			_b = b;
		}
        

        /// <summary>
        /// Gets the value of the alpha channel of this GRaff.Color.
        /// </summary>
        public byte A => _a;

        /// <summary>
        /// Gets the value of the red channel of this GRaff.Color.
        /// </summary>
        public byte R => _r;

		/// <summary>
		/// Gets the value of the green channel of this GRaff.Color.
		/// </summary>
		public byte G => _g;

		/// <summary>
		/// Gets the value of the blue channel of this GRaff.Color.
		/// </summary>
		public byte B => _b;

        public static Color FromRgb(int r, int g, int b) => new Color((byte)r, (byte)g, (byte)b, 255);

        //public static Color FromRgb(int a, int r, int g, int b) => new Color((byte)a, (byte)r, (byte)g, (byte)b);

        public static Color FromRgba(double r, double g, double b, double a) => new Color((byte)(255 * r), (byte)(255 * g), (byte)(255 * b), (byte)(255 * a));

        public static Color FromRgba(uint rgba) => new Color((byte)((rgba >> 24) & 0xFF), (byte)((rgba >> 16) & 0xFF), (byte)((rgba >> 8) & 0xFF), (byte)((rgba) & 0xFF));

        public static Color FromGray(byte intensity) => new Color(intensity, intensity, intensity, (byte)255);
        public static Color FromGray(double intensity) => FromGray((byte)(255 * intensity));

        public static Color FromHsv(int a, Angle h, double s, double v)
        {
            var H = h.Degrees / 60;
            s = GMath.Median(0, s, 1);
            v = GMath.Median(0, v, 1);
            var C = v * s;
            var X = C * (1 - GMath.Abs(H % 2 - 1));
            var m = v - C;

            double R, G, B;
            switch ((int)H)
            {
                case 0: R = C; G = X; B = 0; break;
                case 1: R = X; G = C; B = 0; break;
                case 2: R = 0; G = C; B = X; break;
                case 3: R = 0; G = X; B = C; break;
                case 4: R = X; G = 0; B = C; break;
                default: R = C; G = 0; B = X; break;
            }

            return FromRgba((int)(255 * (R + m)), (int)(255 * (G + m)), (int)(255 * (B + m)), a);
        }
        public static Color FromHsv(Angle h, double s, double v) => FromHsv(255, h, s, v);


		///// <summary>
		/// Gets the value of this color as a 32-bit integer in ARGB format.
		/// </summary>
		//public uint ToArgb => (uint)((A << 24) | (R << 16) | (G << 8) | B);
        public uint Rgba => (uint)((_r << 24) | (_g << 16) | (_b << 8) | _a);

        public int Rgb => (_r << 16) | (_g << 8) | _b;


		/// <summary>
		/// Gets the inverse of this GRaff.Color. The alpha channel is unchanged while the other channels are inverted.
		/// </summary>
		public Color Inverse => new Color((byte)(255 - _r), (byte)(255 - _g), (byte)(255 - _b), _a);
        
		public static Color Merge(Color c1, Color c2, double amount) => c1.Merge(c2, amount);

		/// <summary>
		/// Finds the weighted average of the two GRaff.Color structures, calculating the average of each channel separately.
		/// </summary>
		/// <param name="c">The GRaff.Color to merge with.</param>
		/// <param name="amount">A parameter specifying the weights of the two colors. If it is 0, this GRaff.Color is unchanged, and if this is 1, c is returned.</param>
		/// <returns>The weighted average of the two colors.</returns>
		public Color Merge(Color c, double amount)
		{
			double b = 1 - amount;
			return new Color((byte)(_r * b + c._r * amount), (byte)(_g * b + c._g * amount), (byte)(_b * b + c._b * amount), (byte)(_a * b + c._a * amount));
		}

		/// <summary>
		/// Creates a new GRaff.Color, with the same color channels as this instance, but with the new specified alpha channel.
		/// </summary>
		/// <param name="alphaChannel">The alpha channel of the new color.</param>
		/// <returns>A new GRaff.Color with the same color as this instance, but with the specified alpha channel.</returns>
		public Color Transparent(byte alphaChannel) => new Color(_r, _g, _b, alphaChannel);


		/// <summary>
		/// Creates a new GRaff.Color, with the same color channels as this instance, but iwth the new specified opacity.
		/// </summary>
		/// <param name="opacity">The opacity of the new color. 0.0 means it is completely transparent, and 1.0 means it is completely opaque.</param>
		/// <returns>A new GRaff.Color with the same color as this instance, but with an alpha channel corresponding to the specified opacity.</returns>
		public Color Transparent(double opacity) => new Color(_r, _g, _b, (byte)GMath.Round(255.0 * GMath.Median(0.0, opacity, 1.0)));

		/// <summary>
		/// Converts this GRaff.Color to an OpenTK.Graphics.Color4 object.
		/// </summary>
		/// <returns>The OpenTK.Graphics.Color4 that results from the conversion.</returns>
		internal OpenTK.Graphics.Color4 ToOpenGLColor()
		 => new OpenTK.Graphics.Color4(_r, _g, _b, _a);


		/// <summary>
		/// Converts this GRaff.Color to a human-readable string, showing the value of each channel.
		/// </summary>
		/// <returns>A string that represents this GRaff.Color</returns>
		public override string ToString() => $"{nameof(Color)}=0x{Rgba:X}";

		public bool Equals(Color other) => _a == other._a && _r == other._r && _g == other._g && _b == other._b;


		/// <summary>
		/// Specifies whether this GRaff.Color contains the same ARGB value as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.Color and has the same ARGB value as this GRaff.Color.</returns>
		public override bool Equals(object obj) => (obj is Color) ? Equals((Color)obj) : base.Equals(obj);

		/// <summary>
		/// Returns a hash code of this GRaff.Color. The hash code is equal to the ARGB value.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.Color.</returns>
		public override int GetHashCode() => (int)Rgba;

		/// <summary>
		/// Compares two GRaff.Color objects. The results specifies whether their ARGB values are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Color to compare.</param>
		/// <param name="right">The second GRaff.Color to compare.</param>
		/// <returns>true if the ARGB values of the two GRaff.Color structures are equal.</returns>
		public static bool operator ==(Color left, Color right) => left.Equals(right);

		/// <summary>
		/// Compares two GRaff.Color objects. The results specifies whether their ARGB values are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.Color to compare.</param>
		/// <param name="right">The second GRaff.Color to compare.</param>
		/// <returns>true if the ARGB values of the two colors are unequal.</returns>
		public static bool operator !=(Color left, Color right) => !left.Equals(right);

		///// <summary>
		/// Converts the specified unsigned integer in an RGB format to a GRaff.Color.
		/// </summary>
		/// <param name="rgb">The System.Int32 to be converted.</param>
		/// <returns>The GRaff.Color resulting from the conversion.</returns>
		//public static implicit operator Color(int rgb) => Color.FromRgba(((uint)rgb << 8) | 0xFF);

		/// <summary>
		/// Converts the specified unsigned integer in an ARGB format to a GRaff.Color.
		/// </summary>
		/// <param name="argb">The System.Uint32 to be converted.</param>
		/// <returns>The GRaff.Color resulting from the conversion.</returns>
		public static implicit operator Color(uint rgba) => Color.FromRgba(rgba);
	}
}
