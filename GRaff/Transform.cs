using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	/// <summary>
	/// Represents a set of affine transformations.
	/// </summary>
	/// <remarks>
	/// Transformation of a point is performed in the following order:
	/// - Scaling
	/// - Shearing
	/// - Rotation (around origin)
	/// - Translation
	/// </remarks>
	public sealed class Transform
	{
		/// <summary>
		/// Initializes a new instance of the GRaff.Transform class with default values.
		/// </summary>
		public Transform()
		{
		}

		/// <summary>
		/// Gets or sets the translation in the x-direction of this GRaff.Transform.
		/// </summary>
		public double X { get; set; } = 0;

		/// <summary>
		/// Gets or sets the translation in the y-direction of this GRaff.Transform.
		/// </summary>
		public double Y { get; set; } = 0;

		/// <summary>
		/// Gets or sets the translation of this GRaff.Transform.
		/// </summary>
		public Point Location
		{
			get { return new Point(X, Y); }
			set { X = value.X; Y = value.Y; }
		}

		/// <summary>
		/// Gets or sets the horizontal scale of this GRaff.Transform.
		/// </summary>
		public double XScale { get; set; } = 1;

		/// <summary>
		/// Gets or sets the vertical scale of this GRaff.Transform.
		/// </summary>
		public double YScale { get; set; } = 1;

		/// <summary>
		/// Gets or sets the scale of this GRaff.Transform.
		/// </summary>
		public Vector Scale
		{
			get { return new Vector(XScale, YScale); }
			set { XScale = value.X; YScale = value.Y; }
		}

		/// <summary>
		/// Gets or sets the rotation of this GRaff.Transform.
		/// </summary>
		public Angle Rotation { get; set; } = Angle.Zero;

		/// <summary>
		/// Gets or sets the horizontal shear of this GRaff.Transform.
		/// </summary>
		public double XShear { get; set; } = 0;

		/// <summary>
		/// Gets or sets the vertical shear of this GRaff.Transform.
		/// </summary>
		public double YShear { get; set; } = 0;

		/// <summary>
		/// Gets an GRaff.AffineMatrix representing this GRaff.Transform.
		/// </summary>
		/// <returns>A GRaff.AffineMatrix representing the transformation.</returns>
		public AffineMatrix GetMatrix()
		{
			double c = GMath.Cos(Rotation), s = GMath.Sin(Rotation);
			return new AffineMatrix(
				XScale * (c - s * YShear), YScale * (-s + c * XShear), X,
				XScale * (s + c * YShear), YScale * (c + s * XShear), Y
				);
		}

		/// <summary>
		/// Transforms the specified GRaff.Point.
		/// </summary>
		/// <param name="p">The GRaff.Point to transform</param>
		/// <returns>The resulting GRaff.Point.</returns>
		public Point Point(Point p) { return GetMatrix() * p; }

		/// <summary>
		/// Transforms the point with the specified x- and y-coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate of the transformed point.</param>
		/// <param name="y">The y-coordinate of the transformed point.</param>
		/// <returns>The resulting GRaff.Point.</returns>
		public Point Point(double x, double y)
		{
			// We're keeping this code around in case we want to change the GetMatrix() method. It is easier to debug this one and check that each step works individually.
			double tx, ty;
			double c = GMath.Cos(Rotation), s = GMath.Sin(Rotation);

			x *= XScale;
			y *= YScale;
			tx = x + XShear * y;
			ty = y + YShear * x;
			x = tx; y = ty;

			tx = x * c - y * s;
			ty = y * c + x * s;
			tx += X;
			ty += Y;

			return new Point(tx, ty);
		}

		/// <summary>
		/// Transforms the specified GRaff.Line. This is equivalent to transforming its endpoints.
		/// </summary>
		/// <param name="line">The GRaff.Line to transform.</param>
		/// <returns>The resulting GRaff.Line.</returns>
		public Line Line(Line line) { return new Line(this.Point(line.Origin), this.Point(line.Destination)); }

		/// <summary>
		/// Transforms the specified GRaff.Rectangle. This is equivalent to transforming its vertices.
		/// </summary>
		/// <param name="rect">The GRaff.Rectangle to transform.</param>
		/// <returns>A GRaff.Point[] containing the resulting vertices of the transformed rectangle.</returns>
		public Point[] Rectangle(Rectangle rect)
		{
			AffineMatrix T = GetMatrix();
			return new Point[] {
				T * this.Point(rect.Left, rect.Top),
				T * this.Point(rect.Right, rect.Top),
				T * this.Point(rect.Right, rect.Bottom),
				T * this.Point(rect.Left, rect.Bottom)
			};
		}

		/// <summary>
		/// Transforms the specified GRaff.Polygon. This is equivalent to transforming each of its vertices.
		/// </summary>
		/// <param name="polygon">The GRaff.Polygon to transform.</param>
		/// <returns>The resulting GRaff.Polygon.</returns>
		public Polygon Polygon(Polygon polygon)
		{
			if (polygon == null) return null;
			AffineMatrix T = GetMatrix();
			return new GRaff.Polygon(polygon.Vertices.Select(v => T * v));
		}
	}
}
