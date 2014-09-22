using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class MaskShape
	{
		private MaskShape(params Point[] pts)
		{
			Polygon = new Polygon(pts);
		}

		public Polygon Polygon { get; private set; }

		public static MaskShape Rectangle(double x, double y, double width, double height)
		{
			return new MaskShape(
				new Point(x, y),
				new Point(x + width, y),
				new Point(x + width, y + height),
				new Point(x, y + height)
			);
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
			return new MaskShape(
				new Point(x + width / 2, y),
				new Point(x + width, y + height / 2),
				new Point(x + width / 2, y + height),
				new Point(x, y + height / 2)
			);
		}

		public static MaskShape Diamond(double width, double height)
		{
			return MaskShape.Diamond(-width / 2, -height / 2, width, height);
		}

		public static MaskShape Diamond(Rectangle rectangle)
		{
			return MaskShape.Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
 		}

		public static MaskShape Circle(double radius)
		{
			return Circle(radius, (int)(3 * GMath.Abs(radius)));
		}

		public static MaskShape Circle(double radius, int precision)
		{
			if (precision <= 0) throw new ArgumentException("Must be greater than 0", "precision");
			if (precision == 1) return new MaskShape(new Point(0, 0));

			Point[] pts = new Point[precision];
			double dt = GMath.Tau / precision;
			for (int i = 0; i < precision; i++)
				pts[i] = (Point)new Vector(radius, Angle.Rad(dt * i));
			return new MaskShape(pts);
		}

		public static MaskShape Circle(Point center, double radius)
		{
			return Circle(center, radius, (int)GMath.Ceiling(3 * GMath.Abs(radius)));
		}

#warning TODO: Optimize this
		public static MaskShape Circle(Point center, double radius, int precision)
		{
			if (precision <= 0) throw new ArgumentException("Must be greater than 0", "precision");
			if (precision == 1) return new MaskShape(center);

			Point[] pts = new Point[precision];
			double dt = GMath.Tau / precision;
			for (int i = 0; i < precision; i++)
				pts[i] = center + new Vector(radius, Angle.Rad(dt * i));

			return new MaskShape(pts);
		}

		public static MaskShape Ellipse(double x, double y, double width, double height)
		{
			int precision = (int)GMath.Ceiling(1.5 * (width + height));

			width /= 2;
			height /= 2;
			x += width;
			y += height;
			
			Point[] pts = new Point[precision];
			double dt = GMath.Tau / precision;
			double t = 0;
			for (int i = 0; i < precision; i++)
				pts[i] = new Point(x + width * GMath.Cos(i * dt), y + height * GMath.Sin(i * dt));

			return new MaskShape(pts);
		}

		public static MaskShape Ellipse(double width, double height)
		{
			return Ellipse(-width / 2, -height / 2, width, height);
		}

		public static MaskShape Ellipse(Rectangle rectangle)
		{
			return Ellipse(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public static MaskShape None { get; } = new MaskShape();

#warning GameMaker.MaskShape.SameAsSprite.Polygon causes undefined behavior.
		private static MaskShape _sameAsSprite = new MaskShape();
		public static MaskShape SameAsSprite
		{
			get
			{
				return _sameAsSprite;
			}
		}
	}
}
