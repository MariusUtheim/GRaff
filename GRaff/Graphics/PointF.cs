using System;
using System.Runtime.InteropServices;


namespace GRaff.Graphics
{
	/// <summary>
	/// Represents a point with single-precision floating point coordinates. This structure is used for optimization in OpenGL; for general purposes, use GRaff.Point instead.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PointF
	{
		public PointF(float x, float y)
			: this()
		{
			X = x;
			Y = y;
		}

		public float X { get; private set; }

		public float Y { get; private set; }

		public static PointF operator *(AffineMatrix m, PointF p) { return new PointF((float)(m.M00 * p.X + m.M01 * p.Y + m.M02), (float)(m.M10 * p.X + m.M11 * p.Y + m.M12)); }

		public static PointF operator *(PointF p, AffineMatrix m) { return new PointF((float)(m.M00 * p.X + m.M01 * p.Y + m.M02), (float)(m.M10 * p.X + m.M11 * p.Y + m.M12)); }

		public static PointF operator +(PointF left, PointF right) { return new PointF(left.X + right.X, left.Y + right.Y); }

		public static implicit operator Point(PointF value)
		{
			return new Point(value.X, value.Y);
		}

		public static explicit operator PointF(Point value)
		{
			return new PointF((float)value.X, (float)value.Y);
		}

		public static explicit operator Vector(PointF value)
		{
			return new Vector(value.X, value.Y);
		}

		/// <summary>
		/// Converts this GRaff.Graphics.PointF to a human-readable string, showing the value of the coordinates.
		/// </summary>
		/// <returns>A string that represents this GRaff.Graphics.PointF.</returns>
		public override string ToString() { return string.Format("({0}, {1})", X, Y); } /*C#6.0*/
	}
}
