using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public sealed class MaskShape
	{
		public MaskShape(Polygon polygon)
		{
			this.Polygon = polygon;
		}

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
			return Circle(new Point(0, 0), radius);
		}


		public static MaskShape Circle(Point center, double radius)
		{
			return new MaskShape(Polygon.Circle(center, radius));
		}

		public static MaskShape Ellipse(double x, double y, double width, double height)
		{
			double w = width / 2, h = height / 2;
			return new MaskShape(Polygon.Ellipse(new Point(x + w, y + h), w, h).Vertices.ToArray());
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
		/// Gets the special GRaff.MaskShape that indicates no mask.
		/// GRaff.Mask objects using this GRaff.MaskShape will never intersect another GRaff.Mask.
		/// </summary>
		public static MaskShape None { get; } = new MaskShape();

		/// <summary>
		/// Gets the special GRaff.MaskShape that indicates a mask should use the MaskShape from the sprite of its referred object.
		/// This will always return the exact same instance; they can for example be compared with == operator, or Object.ReferenceEquals.
		/// Calling GRaff.MaskShape.SameAsSprite.Polygon leads to undefined behavior.
		/// </summary>
		public static MaskShape Automatic { get; } = new MaskShape();
	}
}
