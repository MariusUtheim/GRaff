using System;



namespace GRaff
{
	public sealed class Mask
	{
		private GameObject _owner;

		internal Mask(GameObject owner)
		{
			this._owner = owner;
			this.Shape = MaskShape.SameAsSprite;
		}
		Sprite mySprite = new Sprite("Assets/MySprite.png");

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
		/// Gets or sets the shape of this GRaff.Mask.
		/// If the shape is set to GRaff.MaskShape.SameAsSprite, it instead gets the maskshape of the underlying sprite, or GRaff.MaskShape.None if that sprite is null.
		/// This value cannot be set to null.
		/// </summary>
		/// <exception cref="ArgumentNullException">the value is null.</exception>
#warning DESIGN: What happens if this value is set to null?
		public MaskShape Shape
		{
			get
			{
				if (_maskShape == MaskShape.SameAsSprite)
					return (_owner.Sprite != null) ? _owner.Sprite.MaskShape : MaskShape.None; /*C#6.0*/
				else
					return _maskShape;	
			}

			set
			{
				if (value == null)
					throw new ArgumentNullException(String.Format("The value of {0}.{1}.{2} cannot be null. Consider using {0}.{3}.{4} or {0}.{3}.{5}.", "GRaff", "Mask", "Shape", "MaskShape", "MaskShape.None", "MaskShape.SameAsSprite"));
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
			if (other == null) throw new ArgumentNullException("other");
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
