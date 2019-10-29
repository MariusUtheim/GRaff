using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public sealed class Mask
	{
		public Mask(Polygon polygon)
		{
			this.Polygon = polygon;
		}

		private Mask()
		{
			Polygon = Polygon.Empty;
		}

		private Mask(params Point[] pts)
		{
			Polygon = new Polygon(pts);
		}

		internal Polygon Polygon { get; private set; }

		public static Mask Rectangle(double x, double y, double width, double height) => Rectangle(new Rectangle(x, y, width, height));

		public static Mask Rectangle(double width, double height) => Rectangle(-width / 2, -height / 2, width, height);

		public static Mask Rectangle(Rectangle rectangle) => new Mask(rectangle.TopLeft, rectangle.TopRight, rectangle.BottomRight, rectangle.BottomLeft);


		public static Mask Diamond(double x, double y, double width, double height)
			=> new Mask(new Point(x + width / 2, y), new Point(x + width, y + height / 2), new Point(x + width / 2, y + height), new Point(x, y + height / 2) );

		public static Mask Diamond(double width, double height) => Diamond(-width / 2, -height / 2, width, height);

		public static Mask Diamond(Rectangle rectangle) => Diamond(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
 		

		public static Mask Circle(double radius) => Circle(new Point(0, 0), radius);

		public static Mask Circle(Point center, double radius) => new Mask(Polygon.Circle(center, radius));


		public static Mask Ellipse(double x, double y, double xRadius, double yRadius) => new Mask(Polygon.Ellipse(new Rectangle(x, y, xRadius, yRadius)));

		public static Mask Ellipse(Point center, double xRadius, double yRadius) => new Mask(Polygon.Ellipse(center, xRadius, yRadius));

		public static Mask Ellipse(double xRadius, double yRadius) => Ellipse(Point.Zero, xRadius, yRadius);
	
		public static Mask Ellipse(Rectangle rectangle) => new Mask(GRaff.Polygon.Ellipse(rectangle));

		/// <summary>
		/// Gets the special GRaff.MaskShape that indicates no mask.
		/// GRaff.Mask objects using this GRaff.MaskShape will never intersect another GRaff.Mask.
		/// </summary>
		public static Mask None { get; } = new Mask();

		/// <summary>
		/// Gets the special GRaff.MaskShape that indicates a mask should use the MaskShape from the sprite of its referred object.
		/// </summary>
		public static Mask Automatic { get; } = new Mask();
	}
}
