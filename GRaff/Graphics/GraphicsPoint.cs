using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
#if OpenGL4
using coord = System.Double;
#else
using coord = System.Single;
#endif

namespace GRaff.Graphics
{
	/// <summary>
	/// Represents a point with the precision used by the current OpenGL version. For handheld devices, this is usually single-precision. Other devics use double-precision.
	/// This structure is used for optimization in OpenGL; for general purposes, use GRaff.Point instead.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct GraphicsPoint
	{
		public static GraphicsPoint Zero { get; } = new GraphicsPoint();
#if OpenGL4
		internal static readonly OpenTK.Graphics.OpenGL4.VertexAttribPointerType PointerType = OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Double;
#else
		internal static OpenTK.Graphics.ES30.VertexAttribPointerType PointerType = OpenTK.Graphics.ES30.VertexAttribPointerType.Float;
#endif
		internal static readonly int Size = Marshal.SizeOf<GraphicsPoint>();

        private readonly coord _x, _y;

		public GraphicsPoint(float x, float y)
		{
			this._x = (coord)x;
			this._y = (coord)y;
		}

		public GraphicsPoint(double x, double y)
		{
			this._x = (coord)x;
			this._y = (coord)y;
		}

		public double X => (double)_x;
		public double Y => (double)_y;

		public coord Xt => _x;
		public coord Yt => _y;

		public static GraphicsPoint operator *(Matrix m, GraphicsPoint p)
			=> m == null ? GraphicsPoint.Zero : new GraphicsPoint(m.M00 * p.X + m.M01 * p.Y + m.M02, m.M10 * p.X + m.M11 * p.Y + m.M12);


		public static GraphicsPoint operator *(double left, GraphicsPoint right)
			=> new GraphicsPoint(left * right.X, left * right.Y);

		public static GraphicsPoint operator +(GraphicsPoint left, GraphicsPoint right)
			=> new GraphicsPoint(left.X + right.X, left.Y + right.Y);

		public static GraphicsPoint operator +(GraphicsPoint left, Vector right)
			=> new GraphicsPoint(left.X + right.X, left.Y + right.Y);

		public static GraphicsPoint operator -(GraphicsPoint left, Vector right)
			=> new GraphicsPoint(left.X - right.X, left.Y - right.Y);

		public static Vector operator -(GraphicsPoint left, GraphicsPoint right)
			=> new Vector(left.X - right.X, left.Y - right.Y);

		public static implicit operator Point(GraphicsPoint value)
			=> new Point(value.X, value.Y);


		public static explicit operator GraphicsPoint(Point value)
		=> new GraphicsPoint(value.X, value.Y);


		public static explicit operator Vector(GraphicsPoint value)
			=> new Vector(value.X, value.Y);


		/// <summary>
		/// Converts this GRaff.Graphics.PointF to a human-readable string, showing the value of the coordinates.
		/// </summary>
		/// <returns>A string that represents this GRaff.Graphics.PointF.</returns>
		public override string ToString() => $"({X}, {Y})";
	}
}
