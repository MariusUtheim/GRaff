﻿using System;
using System.Runtime.InteropServices;


namespace GRaff.Graphics
{
	/// <summary>
	/// Represents a point with single-precision floating point coordinates. This structure is used for optimization in OpenGL; for general purposes, use GRaff.Point instead.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PointF
	{
		public static PointF Zero { get; } = new PointF();

		public PointF(float x, float y)
			: this()
		{
			X = x;
			Y = y;
		}

		public PointF(double x, double y)
			: this((float)x, (float)y)
		{ }

		public float X { get; private set; }

		public float Y { get; private set; }

		public static PointF operator *(AffineMatrix m, PointF p)
			=> new PointF((float)(m.M00 * p.X + m.M01 * p.Y + m.M02), (float)(m.M10 * p.X + m.M11 * p.Y + m.M12));


		public static PointF operator *(float left, PointF right)
			=> new PointF(left * right.X, left * right.Y);

		public static PointF operator +(PointF left, PointF right)
			=> new PointF(left.X + right.X, left.Y + right.Y);

		public static PointF operator +(PointF left, Vector right)
			=> new PointF((float)(left.X + right.X), (float)(left.Y + right.Y));

		public static PointF operator -(PointF left, Vector right)
			=> new PointF((float)(left.X - right.X), (float)(left.Y - right.Y));

		public static Vector operator -(PointF left, PointF right)
			=> new Vector(left.X - right.X, left.Y - right.Y);

		public static implicit operator Point(PointF value)
			=> new Point(value.X, value.Y);


		public static explicit operator PointF(Point value)
		=> new PointF((float)value.X, (float)value.Y);


		public static explicit operator Vector(PointF value)
			=> new Vector(value.X, value.Y);


		/// <summary>
		/// Converts this GRaff.Graphics.PointF to a human-readable string, showing the value of the coordinates.
		/// </summary>
		/// <returns>A string that represents this GRaff.Graphics.PointF.</returns>
		public override string ToString() => $"({X}, {Y})";
	}
}
