using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public class MaskShape
	{
		private Point[] _pts;
		private MaskShape.Type _type;

		private MaskShape(params Point[] pts)
		{
			_pts = pts.Clone() as Point[];
			_type = Type.Custom;
		}

		private MaskShape(MaskShape.Type type)
		{
			this._type = type;
		}
 

		private enum Type
		{
			Custom, Rectangle, Diamond, Ellipse
		}

		internal void UpdateSprite(Sprite sprite)
		{
			if (!sprite.IsLoaded && _type != Type.Custom)
				_pts = new Point[0];

			switch (_type)
			{
				case Type.Custom:
					break;

				case Type.Rectangle:
					_pts = new Point[] {
						new Point(0, 0),
						new Point(sprite.Width, 0),
						new Point(sprite.Width, sprite.Height),
						new Point(0, sprite.Height)
					};
					break;
			}
		}


		public static MaskShape Rectangle()
		{
			throw new NotImplementedException();
		}

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
			throw new NotImplementedException();
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

		public static MaskShape Circle(double radius, int precision)
		{
			Point[] pts = new Point[precision];
			double dt = GMath.Tau / precision;
			for (int i = 0; i < precision; i++)
				pts[i] = (Point)new Vector(radius, Angle.Rad(dt * i));
			return new MaskShape(pts);
		}

		public static MaskShape Ellipse()
		{
			throw new NotImplementedException();
		}

		public static MaskShape Ellipse(double x, double y, double width, double height)
		{
			throw new NotImplementedException();
		}

		public static MaskShape None()
		{
			return new MaskShape();
		}

		public static MaskShape Polygon(Point[] pts)
		{
			return new MaskShape(pts.Clone() as Point[]);
		}
	}
}
