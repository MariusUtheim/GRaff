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
	/// - Rotation
	/// - Translation (not performed on vectors)
	/// </remarks>
	public sealed class Transform
	{
		/// <summary>
		/// Initializes a new instance of the GRaff.Transform class with default values.
		/// </summary>
		public Transform()
		{
		}

        public Transform Clone()
            => new Transform { Scale = this.Scale, Shear = this.Shear, Rotation = this.Rotation, Location = this.Location };

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
            get => (X, Y);
            set => (X, Y) = value;
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
			get => (XScale, YScale);
            set => (XScale, YScale) = value;
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

        public Vector Shear
        {
            get => (XShear, YShear);
            set => (XShear, YShear) = value;
        }

		/// <summary>
		/// Gets an GRaff.AffineMatrix representing this GRaff.Transform.
		/// </summary>
		/// <returns>A GRaff.AffineMatrix representing the transformation.</returns>
		public Matrix GetMatrix()
		{
			double c = GMath.Cos(Rotation), s = GMath.Sin(Rotation);
			return new Matrix(
				XScale * (c - s * YShear), YScale * (-s + c * XShear), X,
				XScale * (s + c * YShear), YScale * (c + s * XShear), Y
			);
		}

		/// <summary>
		/// Transforms the specified GRaff.Point.
		/// </summary>
		/// <param name="p">The GRaff.Point to transform-</param>
		/// <returns>The resulting GRaff.Point.</returns>
		public Point Point(Point p) => GetMatrix() * p;

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
		/// Transforms the specified GRaff.Vector. Note that result is not affected by the translational components of this GRaff.Transform.
		/// </summary>
		/// <param name="v">The GRaff.Vector to transform.</param>
		/// <returns>The resulting GRaff.Vector.</returns>
		public Vector Vector(Vector v) => GetMatrix() * v;

		/// <summary>
		/// Transforms the point with the specified x- and y-coordinates. Note that result is not affected by the translational components of this GRaff.Transform.
		/// </summary>
		/// <param name="x">The x-coordinate of the transformed vector.</param>
		/// <param name="y">The y-coordinate of the transformed vector.</param>
		/// <returns>The resulting GRaff.Vector.</returns>
		public Vector Vector(double x, double y) => this.Vector(new GRaff.Vector(x, y));

		/// <summary>
		/// Transforms the specified GRaff.Line. This is equivalent to transforming its endpoints.
		/// </summary>
		/// <param name="line">The GRaff.Line to transform.</param>
		/// <returns>The resulting GRaff.Line.</returns>
		public Line Line(Line line) => new Line(this.Point(line.Origin), this.Vector(line.Direction));

		public Triangle Triangle(Triangle triangle)
			=> this.Triangle(triangle.V1, triangle.V2, triangle.V3);

		public Triangle Triangle(Point v1, Point v2, Point v3)
		{
			Matrix T = GetMatrix();
			return new Triangle(T * v1, T * v2, T * v3);
		}

		public Triangle Triangle(double x1, double y1, double x2, double y2, double x3, double y3)
			=> this.Triangle(new Point(x1, y1), new Point(x2, y2), new Point(x3, y3));
	


		/// <summary>
		/// Transforms the specified GRaff.Rectangle. This is equivalent to transforming its vertices.
		/// </summary>
		/// <param name="rect">The GRaff.Rectangle to transform.</param>
		/// <returns>A GRaff.Point[] containing the resulting vertices of the transformed rectangle.</returns>
		public Point[] Rectangle(Rectangle rect)
		{
			Matrix T = GetMatrix();
			return new Point[] {
				T * rect.TopLeft,
				T * rect.TopRight,
				T * rect.BottomRight,
				T * rect.BottomLeft
			};
		}

		/// <summary>
		/// Transforms the specified GRaff.Polygon. This is equivalent to transforming each of its vertices.
		/// </summary>
		/// <param name="polygon">The GRaff.Polygon to transform.</param>
		/// <returns>The resulting GRaff.Polygon.</returns>
		public Polygon Polygon(Polygon polygon)
		{
			if (polygon == null)
				return null;
			Matrix T = GetMatrix();
			return new GRaff.Polygon(polygon.Vertices.Select(v => T * v).ToArray());
		}
	}
}
