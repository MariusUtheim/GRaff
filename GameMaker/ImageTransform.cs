using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class ImageTransform
	{

		public ImageTransform(double xScale, double yScale, Angle rotation, Vector origin)
		{
			// TODO: Complete member initialization
			this.XScale = xScale;
			this.YScale = yScale;
			this.Rotation = rotation;
			this.Origin = origin;
		}
		public double XScale { get; set; }
		public double YScale { get; set; }
		public Angle Rotation { get; set; }
		public Vector Origin { get; set; }

		public Point Point(Point pt)
		{
			return Point(pt.X, pt.Y);
		}

		public Point Point(double x, double y)
		{
			Vector result = Vector.Cartesian(x, y);
			result -= Origin;
			result.X *= XScale;
			result.Y *= YScale;
			result.Direction += Rotation;
			result += Origin;
			return (Point)result;
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
