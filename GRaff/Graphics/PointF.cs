using System;
using System.Runtime.InteropServices;


namespace GRaff.Graphics
{
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

		public static implicit operator Point(PointF value)
		{
			return new Point(value.X, value.Y);
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
