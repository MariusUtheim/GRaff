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
			this.Shape = MaskShape.SameAsSprite;
		}

		public Polygon GetPolygon()
		{
			return Transform.Polygon(Shape.Polygon);
		}


		private Transform Transform
		{
			get { return _owner.Transform; }
		}

		private MaskShape _maskShape;
		/// <summary>
		/// Gets or sets the shape of this GameMaker.Mask.
		/// If the shape is set to GameMaker.MaskShape.SameAsSprite, it instead gets the maskshape of the underlying sprite, or GameMaker.MaskShape.None if that sprite is null.
		/// This value cannot be set to null.
		/// </summary>
		public MaskShape Shape
		{
			get
			{
				if (_maskShape == MaskShape.SameAsSprite)
					return _owner?.Sprite.MaskShape ?? MaskShape.None;
				else
					return _maskShape;	
			}

			set
			{
#warning DESIGN: Should setting the value to null instead set it to MaskShape.None?
				if (value == null)
					throw new ArgumentNullException("The value of GameMaker.Mask.Shape cannot be null. Consider using GameMaker.MaskShape.None or GameMaker.MaskShape.SameAsSprite.");
				_maskShape = value;
			}
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
			Draw.Polygon(color, GetPolygon());
		}
	}




}
