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
		public double XScale { get; set; }
		public double YScale { get; set; }
		public Angle Rotation { get; set; }

		public Point Point(Point pt)
		{
			return Point(pt.X, pt.Y);
		}

		public Point Point(double x, double y)
		{
			double tx, ty;
			double c = GMath.Cos(Rotation), s = GMath.Sin(Rotation);

			x *= XScale;
			y *= YScale;
			tx = x * c - y * s + X;
			ty = y * c + x * s + Y;
		//	tx = x * XScale * c + y *  + X;
		//	ty = y * YScale * GMath.Sin(Rotation.Radians) + Y;
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
