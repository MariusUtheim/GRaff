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
			return Rectangle(-width / 2, -height / 2, width, height);
		}

		public static MaskShape Rectangle(Rectangle rectangle)
		{
			return Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
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
			return Circle(new Point(0, 0), radius, (int)(3 * GMath.Abs(radius)));
		}

		public static MaskShape Circle(double radius, int precision)
		{
			return Circle(new Point(0, 0), radius, precision);
		}

		public static MaskShape Circle(Point center, double radius)
		{
			return Circle(center, radius, (int)GMath.Ceiling(3 * GMath.Abs(radius)));
		}

		public static MaskShape Circle(Point center, double radius, int precision)
		{
			return new MaskShape(Polygon.EnumerateCircle(center, radius, precision).ToArray());
		}

		public static MaskShape Ellipse(double x, double y, double width, double height)
		{
			double w = width / 2, h = height / 2;
			return new MaskShape(Polygon.EnumerateEllipse(new Point(x + w, y + h), w, h).ToArray());
		}

		public static MaskShape Ellipse(double width, double height)
		{
			return Ellipse(-width / 2, -height / 2, width, height);
		}

		public static MaskShape Ellipse(Rectangle rectangle)
		{
			return Ellipse(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		/// <summary>
		/// Gets the special GameMaker.MaskShape that indicates no mask.
		/// GameMaker.Mask objects using this GameMaker.MaskShape will never intersect another GameMaker.Mask.
		/// </summary>
		public static MaskShape None { get; } = new MaskShape();

		/// <summary>
		/// Gets the special GameMaker.MaskShape that indicates a mask should use the MaskShape from the sprite of its referred object.
		/// This will always return the exact same instance; they can for example be compared with == operator, or Object.ReferenceEquals.
		/// Calling GameMaker.MaskShape.SameAsSprite.Polygon leads to undefined behavior.
		/// </summary>
		public static MaskShape SameAsSprite { get; } = new MaskShape();
	}
}
