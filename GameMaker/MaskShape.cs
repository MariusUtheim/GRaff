using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public class MaskShape
	{
		internal Point[] _pts;

		private MaskShape() { }



		public static MaskShape Rectangle(double x, double y, double width, double height)
		{
			return new MaskShape {
				_pts = new Point[] {
					new Point(x, y),
					new Point(x + width, y),
					new Point(x + width, y + height),
					new Point(x, y + height)
				}
			};
		}

		public static MaskShape Rectangle(double width, double height)
		{
			return MaskShape.Rectangle(-width / 2, -height / 2, width, height);
		}

		public static MaskShape Rectangle(Rectangle rectangle)
		{
			return MaskShape.Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static MaskShape Diamond(double x, double y, double width, double height)
		{
			return new MaskShape {
				_pts = new Point[] {
					new Point(x + width / 2, y),
					new Point(x + width, y + height / 2),
					new Point(x + width / 2, y + height),
					new Point(x, y + height / 2)
				}
			};
		}

		public static MaskShape Circle(double radius)
		{
			return Circle(radius, (int)(1 + 2 * GMath.Sqrt(radius)));
		}

		public static MaskShape Circle(double radius, int precision)
		{
			Point[] pts = new Point[precision];
			double dt = GMath.Tau / precision;
			for (int i = 0; i < precision; i++)
				pts[i] = (Point)Vector.Polar(radius, Angle.Rad(dt * i));
			return new MaskShape { _pts = pts };
		}

		public static MaskShape Polygon(Point[] pts)
		{
			return new MaskShape {
				_pts = pts.Clone() as Point[]
			};
		}

		public static MaskShape None()
		{
			return new MaskShape {
				_pts = new Point[0]
			};
		}

	}
}
