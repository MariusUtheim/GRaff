using System;
using System.Diagnostics.Contracts;
using GRaff.Graphics;


namespace GRaff
{
	/// <summary>
	/// Represents a game element that has a spatial location in the room.
	/// </summary>
	public abstract class GameObject : GameElement
	{
		/// <summary>
		/// Initializes a new instance of the GRaff.GameObject class.
		/// </summary>
		protected GameObject()
			: this(0, 0) { }

		/// <summary>
		/// Initializes a new instance of the GRaff.GameObject class with the specified x- and y-coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		protected GameObject(double x, double y)
		{
            Transform = new Transform { X = x, Y = y };
        }

        /// <summary>
        /// Initializes a new instance of the GRaff.GameObject class at the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        protected GameObject(Point location)
            : this(location.X, location.Y) { }


        public Transform Transform { get; }

        public Sprite? Sprite { get; set; }


		/// <summary>
		/// Gets or sets the x-coordinate of this GRaff.GameObject.
		/// </summary>
		public double X
		{
            get => Transform.X;
            set => Transform.X = value;
		}

		/// <summary>
		/// Gets or sets the y-coordinate of this GRaff.GameObject.
		/// </summary>
		public double Y
		{
            get => Transform.Y;
            set => Transform.Y = value;
		}

		/// <summary>
		/// Gets or sets the location of this GRaff.GameObject.
		/// </summary>
		public Point Location
		{
			get => Transform.Location; 
			set => Transform.Location = value; 
		}
   
		/// <summary>
		/// An action that is performed at the beginning of each update loop.
		/// </summary>
		public virtual void OnBeginStep() { }

		/// <summary>
		/// An action that is performed at the end of each update loop.
		/// </summary>
		public virtual void OnEndStep() { }

		/// <summary>
		/// An action that is performed when the instance is drawn. This will draw the Sprite of this GRaff.GameObject at the correct position
		/// as specified by its Transform. This method can be overridden.
		/// </summary>
		public override void OnDraw()
		{
			if (Sprite != null)
			{
				Draw.Sprite(Sprite, ImageIndex, Transform, Blend);
				if (Animate())
					(this as IAnimationEndListener)?.AnimationEnd();
			}
		}

        #region Image section

        public Color Blend { get; set; }

        public double Alpha
        {
            get { return Blend.A / 255.0; }
            set { Blend = Blend.Transparent(value); }
        }

        private double _index;
        public double ImageIndex
        {
            get { return _index; }
            set { _index = GMath.Remainder(value, ImageCount); }
        }

        public double ImageSpeed
        {
            get;
            set;
        }

        public int ImageCount => Sprite?.AnimationStrip.ImageCount ?? 1;

        public double ImagePeriod
        {
            get
            {
                if (Sprite == null)
                    return Double.NaN;
                else if (ImageSpeed == 0)
                    return Double.PositiveInfinity;
                else
                    return GMath.Abs(Sprite.AnimationStrip.Duration / ImageSpeed);
            }
        }

        public SubTexture CurrentTexture
        {
            get
            {
                if (Sprite == null)
                    throw new InvalidOperationException($"The {nameof(GRaff)}.{nameof(GameObject)} has no sprite.");
                return Sprite.SubImage(ImageIndex);
            }
        }


        public bool Animate()
        {
            if (Sprite != null)
            {
                _index += ImageSpeed;
                if (_index >= ImageCount || _index < 0)
                {
                    _index = GMath.Remainder(_index, ImageCount);
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Mask section

        public Mask Mask { get; set; } = Mask.None;


        public Polygon MaskPolygon
        {
            get
            {
                if (Mask.ReferenceEquals(Mask, Mask.None))
                    return Polygon.Empty;
                else if (Mask.ReferenceEquals(Mask, Mask.Automatic))
                    return (Sprite != null) ? Transform.Polygon(Sprite.MaskShape.Polygon) : Polygon.Empty;
                else
                    return Transform.Polygon(Mask.Polygon);
            }
        }


        public Rectangle BoundingBox
        {
            get
            {
                Polygon _polygon = MaskPolygon;
                if (_polygon == null)
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

        public bool ContainsPoint(Point pt) => MaskPolygon?.ContainsPoint(pt) ?? false;

        public bool ContainsPoint(double x, double y) => ContainsPoint((x, y));

        public bool Intersects(GameObject other)
        {
            if (other == null)
                return false;
            return MaskPolygon?.Intersects(other.MaskPolygon) ?? false;
        }

        #endregion

    }
}
