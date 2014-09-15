using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class Transform
	{
		public Transform()
			: base()
		{
			XScale = 1;
			YScale = 1;
			Rotation = Angle.Zero;
		}

		public Transform(double xScale, double yScale, Angle rotation)
		{
			this.XScale = xScale;
			this.YScale = yScale;
			this.Rotation = rotation;
		}

		public double X { get; set; }

		public double Y { get; set; }

		public Point Location
		{
			get { return new Point(X, Y); }
			set { X = value.X; Y = value.Y; }
		}

		public double XScale { get; set; }

		public double YScale { get; set; }

		public Vector Scale
		{
			get { return new Vector(XScale, YScale); }
			set { XScale = value.X; YScale = value.Y; }
		}

		public Angle Rotation { get; set; }

		public double XShear { get; set; }

		public double YShear { get; set; }


#warning TODO: Cache instead of computing again every time
		public Matrix GetMatrix()
		{
			double c = GMath.Cos(Rotation), s = GMath.Sin(Rotation);
			return new Matrix(
				XScale * (c - s * YShear), YScale * (-s + c * XShear), X,
				XScale * (s + c * YShear), YScale * (c + s * XShear), Y
				);
		}

		public Point Point(Point pt)
		{
			//return this.Point(pt.X, pt.Y);
			return GetMatrix() * pt;
		}

		public Point Point(double x, double y)
		{
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

		public Line Line(Line line)
		{
			return new Line(Point(line.Origin), Point(line.Destination));
		}

		public Point[] Rectangle(Rectangle rect)
		{
			return new Point[] {
				this.Point(rect.Left, rect.Top),
				this.Point(rect.Right, rect.Top),
				this.Point(rect.Right, rect.Bottom),
				this.Point(rect.Left, rect.Bottom)
			};

		}

	}
}
