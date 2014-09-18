using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
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
	public class Transform
	{
		public Transform()
		{
		}

		public Transform(double xScale, double yScale, Angle rotation)
		{
			this.XScale = xScale;
			this.YScale = yScale;
			this.Rotation = rotation;
		}

		public double X { get; set; } = 1;

		public double Y { get; set; } = 1;

		public Point Location
		{
			get { return new Point(X, Y); }
			set { X = value.X; Y = value.Y; }
		}

		public double XScale { get; set; } = 1;

		public double YScale { get; set; } = 1;

		public Vector Scale
		{
			get { return new Vector(XScale, YScale); }
			set { XScale = value.X; YScale = value.Y; }
		}

		public Angle Rotation { get; set; } = Angle.Zero;

		public double XShear { get; set; } = 0;

		public double YShear { get; set; } = 0;


#warning TODO: Cache instead of computing again every time
		public Matrix GetMatrix()
		{
			double c = GMath.Cos(Rotation), s = GMath.Sin(Rotation);
			return new Matrix(
				XScale * (c - s * YShear), YScale * (-s + c * XShear), X,
				XScale * (s + c * YShear), YScale * (c + s * XShear), Y
				);
		}


		public Point Point(Point pt) => GetMatrix() * pt;


		public Point Point(double x, double y)
		{
#warning We're keeping this code around in case we want to change the GetMatrix() method. It is easier to debug this one and check that each step works individually.
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


		public Line Line(Line line) => new Line(this.Point(line.Origin), this.Point(line.Destination));


		public Point[] Rectangle(Rectangle rect)
			=> new Point[] {
				this.Point(rect.Left, rect.Top),
				this.Point(rect.Right, rect.Top),
				this.Point(rect.Right, rect.Bottom),
				this.Point(rect.Left, rect.Bottom)
			};

	}
}
