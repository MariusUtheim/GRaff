using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public sealed class Mask
	{
		private GameObject _owner;
		private Polygon _polygon;

		internal Mask(GameObject owner)
		{
			this._owner = owner;
		}

		public Polygon GetPolygon()
		{
			throw new NotImplementedException();
		}


		private Transform Transform
		{
			get { return _owner.Transform; }
		}

		private MaskShape MaskShape
		{
			get { return _owner.Sprite.MaskShape; }
		}

		public Rectangle BoundingBox
		{
			get
			{
				Polygon _polygon = GetPolygon();
				if (_polygon.Length == 0)
					return new Rectangle(Transform.X, Transform.Y, 0, 0);

				Point vertex;
				double left, right, top, bottom;
				vertex = _polygon.Vertex(0);
				left = right = vertex.X;
				top = bottom = vertex.Y;

				for (int i = 1; i < _polygon.Length; i++)
				{
					vertex = _polygon.Vertex(i);
					if (vertex.X < left) left = vertex.X;
					if (vertex.X > right) right = vertex.X;
					if (vertex.Y < top) top = vertex.Y;
					if (vertex.Y > bottom) bottom = vertex.Y;
				}

				return new Rectangle(left, top, right - left, bottom - top);
			}
		}

		public bool ContainsPoint(Point pt)
		{
			return GetPolygon().ContainsPoint(pt);
		}

		public bool ContainsPoint(double x, double y)
		{
			return ContainsPoint(new Point(x, y));
		}

		public bool Intersects(Mask other)
		{
			return GetPolygon().Intersects(other.GetPolygon());
		}


		public void DrawOutline()
		{ 
			DrawOutline(Color.Black);
		}

		public void DrawOutline(Color color)
		{
			Draw.Rectangle(color, BoundingBox);

		}
	}




}
