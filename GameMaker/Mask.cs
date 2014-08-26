using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public sealed class Mask
	{
		private Point[] _pts;
		private GameObject _owner;
		private Shape _shape;

		private enum Shape
		{
			Custom, Rectangle, Diamond, Ellipse
		}

#warning TODO: Generate mask from sprite by default

		internal Mask(GameObject owner)
		{
			this._owner = owner;
			this._pts = new Point[0];
			this._shape = Shape.Rectangle;
			Transform = owner.Transform;
		}

		public Transform Transform
		{
			get;
			private set;
		}

		internal void Update()
		{
			if (_shape != Shape.Custom)
			{
				if (_owner.Image.Sprite == null)
					_pts = new Point[0];
				else
				{
#warning TODO: Make each sprite define its own mask?
					Rectangle region = new Rectangle((Point)(-_owner.Image.Sprite.Origin), _owner.Image.Sprite.Size);
					switch (_shape)
					{
						case Shape.Diamond: Diamond(region.Left, region.Top, region.Width, region.Height); break;
						case Shape.Ellipse: Ellipse(region.Left, region.Top, region.Width, region.Height); break;
						case Shape.Rectangle: Rectangle(region.Left, region.Top, region.Width, region.Height); break;
					}
				}
			}
		}

		public void Rectangle(double x, double y, double width, double height)
		{
			_pts = new Point[] {
				new Point(x, y),
				new Point(x + width, y),
				new Point(x + width, y + height),
				new Point(x, y + height)
			};
		}

		public void Rectangle(double width, double height)
		{
			Rectangle(-width / 2, -height / 2, width, height);
		}

		public void Rectangle(Rectangle rectangle)
		{
			Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		public void Diamond(double x, double y, double width, double height)
		{
			_pts = new Point[] {
				new Point(x + width / 2, y),
				new Point(x + width, y + height / 2),
				new Point(x + width / 2, y + height),
				new Point(x, y + height / 2)
			};
		}

		public void Circle(double radius)
		{
			Circle(radius, (int)(1 + 2 * GMath.Sqrt(radius)));
		}

		public void Circle(double radius, int precision)
		{
			_pts = new Point[precision];
			double dt = GMath.Tau / precision;
			for (int i = 0; i < precision; i++)
				_pts[i] = (Point)Vector.Polar(radius, Angle.Rad(dt * i));
		}

		public void Ellipse(double width, double height)
		{
			throw new NotImplementedException();
		}

		public void Ellipse(double x, double y, double width, double height)
		{
			throw new NotImplementedException();
		}

		public void Polygon(Point[] pts)
		{
			_pts = pts.Clone() as Point[];
		}

		public void None()
		{
			_pts = new Point[0];
		}

		public Rectangle BoundingBox
		{
			get
			{
				if (this._pts.Length == 0)
					return new Rectangle(0, 0, 0, 0);

				double left, right, top, bottom;
				left = right = Transform.Point(_pts[0]).X;
				top = bottom = Transform.Point(_pts[0]).Y;

				for (int i = 1; i < _pts.Length; i++)
				{
					Point pt = Transform.Point(_pts[i]);
					if (pt.X < left) left = pt.X;
					if (pt.X > right) right = pt.X;
					if (pt.Y < top) top = pt.Y;
					if (pt.Y > bottom) bottom = pt.Y;
				}

				return new Rectangle(left, top, right - left, bottom - top);
			}
		}

		public bool ContainsPoint(Point pt)
		{
			/**
			 * If the polygon is convex then one can consider the polygon as a "path" from the first vertex. 
			 * A point is on the interior of this polygons if it is always on the same side of all the line segments making up the path.
			 *Given a line segment between P0 (x0,y0) and P1 (x1,y1), another point P (x,y) has the following relationship to the line segment:
			 *
			 * Compute (y - y0) (x1 - x0) - (x - x0) (y1 - y0) 
			 * if it is less than 0 then P is to the right of the line segment, 
			 * if greater than 0 it is to the left, if equal to 0 then it lies on the line segment.
			 * */
			foreach (Line L in Path)
				if (L.LeftNormal.DotProduct(pt - L.Origin) > 0)
					return false;

			return true;
		}

		public bool ContainsPoint(double x, double y)
		{
			return ContainsPoint(new Point(x, y));
		}

		public bool Intersects(Mask other)
		{
			return _Intersects(other) && other._Intersects(this);
		}


		private IEnumerable<Line> Path
		{
			get
			{
				if (_pts.Length == 0)
					yield break;

				for (int i = 0; i < _pts.Length - 1; i++)
				{
					Line theLine = Transform.Line(new Line(_pts[i], _pts[i + 1]));
					yield return theLine;
				}
				yield return Transform.Line(new Line(_pts.Last(), _pts.First() - _pts.Last()));
			}
		}

		private bool _Intersects(Mask other)
		{
			/**
			 * Using the separation axis theorem: 
			 * http://stackoverflow.com/questions/753140/how-do-i-determine-if-two-convex-polygons-intersect
			 * */

			foreach (var L in Path)
			{
				var n = L.LeftNormal;
				if (other._pts.Select(pt => other.Transform.Point(pt)).All(pt => n.DotProduct(pt - L.Origin) > 0))
					return false;
			}

			return true;
		}

		public void DrawOutline()
		{ 
			DrawOutline(Color.Black);
		}

		public void DrawOutline(Color color)
		{
			Draw.Rectangle(Color.Black, BoundingBox);

		}
	}




}
