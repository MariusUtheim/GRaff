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

		private MaskShape()
		{
			Polygon = null;
		}

		private MaskShape(params Point[] pts)
		{
			Polygon = new Polygon(pts, Unit._);
		}

		public Polygon Polygon { get; private set; }


		public static MaskShape Rectangle(double x, double y, double width, double height) => Rectangle(new Rectangle(x, y, width, height));

		public static MaskShape Rectangle(double width, double height) => Rectangle(-width / 2, -height / 2, width, height);

		public static MaskShape Rectangle(Rectangle rectangle) => new MaskShape(rectangle.TopLeft, rectangle.TopRight, rectangle.BottomRight, rectangle.BottomLeft);


		public static MaskShape Diamond(double x, double y, double width, double height)
			=> new MaskShape(new Point(x + width / 2, y), new Point(x + width, y + height / 2), new Point(x + width / 2, y + height), new Point(x, y + height / 2) );

		public static MaskShape Diamond(double width, double height) => Diamond(-width / 2, -height / 2, width, height);

		public static MaskShape Diamond(Rectangle rectangle) => Diamond(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
 		

		public static MaskShape Circle(double radius) => Circle(new Point(0, 0), radius);

		public static MaskShape Circle(Point center, double radius) => new MaskShape(Polygon.Circle(center, radius));


		public static MaskShape Ellipse(double x, double y, double xRadius, double yRadius) => new MaskShape(Polygon.Ellipse(new Rectangle(x, y, xRadius, yRadius)));

		public static MaskShape Ellipse(Point center, double xRadius, double yRadius) => new MaskShape(Polygon.Ellipse(center, xRadius, yRadius));

		public static MaskShape Ellipse(double xRadius, double yRadius) => Ellipse(Point.Zero, xRadius, yRadius);
	
		public static MaskShape Ellipse(Rectangle rectangle) => new MaskShape(GRaff.Polygon.Ellipse(rectangle));

		/// <summary>
		/// Gets the special GRaff.MaskShape that indicates no mask.
		/// GRaff.Mask objects using this GRaff.MaskShape will never intersect another GRaff.Mask.
		/// </summary>
		public static MaskShape None { get; } = new MaskShape();

		/// <summary>
		/// Gets the special GRaff.MaskShape that indicates a mask should use the MaskShape from the sprite of its referred object.
		/// </summary>
		public static MaskShape Automatic { get; } = new MaskShape();
	}
}
