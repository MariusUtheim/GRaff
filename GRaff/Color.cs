using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GRaff
{
	/// <summary>
	/// Represents an ARGB color. Note that colors can be cast from uint structures on the form 0xAARRGGBB.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Color : IEquatable<Color>
	{
		/// <summary>
		/// Initializes a new instance of the GRaff.Color structure using the specifed alpha, red, green and blue values.
		/// </summary>
		/// <param name="a">The alpha channel.</param>
		/// <param name="r">The red channel.</param>
		/// <param name="g">The green channel.</param>
		/// <param name="b">The blue channel.</param>
		public Color(byte a, byte r, byte g, byte b)
			: this()
		{
			A = a;
			R = r;
			G = g;
			B = b;
		}

		/// <summary>
		/// Gets the value of the red channel of this GRaff.Color.
		/// </summary>
		public byte R { get; private set; }

		/// <summary>
		/// Gets the value of the green channel of this GRaff.Color.
		/// </summary>
		public byte G { get; private set; }

		/// <summary>
		/// Gets the value of the blue channel of this GRaff.Color.
		/// </summary>
		public byte B { get; private set; }

		/// <summary>
		/// Gets the value of the alpha channel of this GRaff.Color.
		/// </summary>
		public byte A { get; private set; }

		/// <summary>
		/// Initializes a new instance of the GRaff.Color class, using the specified ARGB values.
		/// </summary>
		/// <param name="a">The alpha channel.</param>
		/// <param name="r">The red channel.</param>
		/// <param name="g">The green channel.</param>
		/// <param name="b">The blue channel.</param>
		public Color(int a, int r, int g, int b) : this((byte)a, (byte)r, (byte)g, (byte)b) { }

		/// <summary>
		/// Initializes a new instance of the GRaff.Color class, using the specified RGB values and an alpha value of 255.
		/// </summary>
		/// <param name="r">The red channel.</param>
		/// <param name="g">The green channel.</param>
		/// <param name="b">The blue channel.</param>
		public Color(byte r, byte g, byte b) : this((byte)255, r, g, b) { }

		/// <summary>
		/// Initializes a new instance of the GRaff.Color class, using the specified ARGB value in a 32-bit format.
		/// Colors can also be implicitly converted from ints.
		/// </summary>
		/// <param name="argb">The ARGB value of the created color.</param>
		public Color(uint argb) : this((byte)(argb >> 24), (byte)(argb >> 16), (byte)(argb >> 8), (byte)argb) { }


		/// <summary>
		/// Gets the value of this color as a 32-bit integer in ARGB format.
		/// </summary>
		public uint Argb => (uint)((A << 24) | (R << 16) | (G << 8) | B);
	
		/// <summary>
		/// Gets the inverse of this GRaff.Color. The alpha channel is unchanged while the other channels are inverted.
		/// </summary>
		public Color Inverse => new Color(A, 255 - R, 255 - G, 255 - B);


		/// <summary>
		/// Finds the weighted average of the two GRaff.Color structures, calculating the average of each channel separately.
		/// </summary>
		/// <param name="c">The GRaff.Color to merge with.</param>
		/// <param name="amount">A parameter specifying the weights of the two colors. If it is 0, this GRaff.Color is unchange, and if this is 1, c is returned.</param>
		/// <returns>The weighted average of the two colors.</returns>
		public Color Merge(Color c, double amount)
		{
			double b = 1 - amount;
			return new Color((int)(A * b + c.A * amount), (int)(R * b + c.R * amount), (int)(G * b + c.G * amount), (int)(B * b + c.B * amount));
		}

		/// <summary>
		/// Creates a new GRaff.Color, with the same color channels as this instance, but with the new specified alpha channel.
		/// </summary>
		/// <param name="alphaChannel">The alpha channel of the new color.</param>
		/// <returns>A new GRaff.Color with the same color as this instance, but with the specified alpha channel.</returns>
		public Color Transparent(int alphaChannel) => new Color((byte)alphaChannel, R, G, B);


		/// <summary>
		/// Creates a new GRaff.Color, with the same color channels as this instance, but iwth the new specified opacity.
		/// </summary>
		/// <param name="opacity">The opacity of the new color. 0.0 means it is completely transparent, and 1.0 means it is completely opaque.</param>
		/// <returns>A new GRaff.Color with the same color as this instance, but with an alpha channel corresponding to the specified opacity.</returns>
		public Color Transparent(double opacity) => new Color((byte)GMath.Round(255.0 * GMath.Median(0.0, opacity, 1.0)), R, G, B);

		/// <summary>
		/// Converts this GRaff.Color to an OpenTK.Graphics.Color4 object.
		/// </summary>
		/// <returns>The OpenTK.Graphics.Color4 that results from the conversion.</returns>
		internal OpenTK.Graphics.Color4 ToOpenGLColor()
		 => new OpenTK.Graphics.Color4(R, G, B, A);


		/// <summary>
		/// Converts this GRaff.Color to a human-readable string, showing the value of each channel.
		/// </summary>
		/// <returns>A string that represents this GRaff.Color</returns>
		public override string ToString() => "\{nameof(Color)}=0x\{Argb : X}";

		public bool Equals(Color other) => A == other.A && R == other.R && G == other.G && B == other.B;


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
		public override int GetHashCode() => (int)Argb;

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


		/// <summary>
		/// Converts the specified integer in an ARGB format to a GRaff.Color
		/// </summary>
		/// <param name="argb">The System.Uint32 to be converted.</param>
		/// <returns>The GRaff.Color resulting from the conversion.</returns>
		public static implicit operator Color(uint argb) => new Color(argb);
	}
}
