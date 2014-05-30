using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public abstract class GameObject
	{
		public GameObject()
		{
			Instance.Add(this);
			Image = new Image(this);
		}

		public GameObject(double x, double y)
			: this()
		{
			X = x;
			Y = y;
		}

		public GameObject(Point location)
			: this(location.X, location.Y) { }

		public double X { get; set; }
		public double Y { get; set; }

		public Point Location
		{
			get { return new Point(X, Y); }
			set { X = value.X; Y = value.Y; }
		}

		public Image Image { get; set; }

		public virtual Sprite Sprite
		{
			get;
			set;
		}

		private int _depth;
		public int Depth 
		{
			get { return _depth; }
			set { _depth = value; Instance.Sort(); }
		}

		public void Destroy()
		{
			OnDestroy();
			Instance.Remove(this);
		}

		public virtual Rectangle BoundingBox
		{
			get 
			{
				if (Sprite == null)
					return new Rectangle(Location, Vector.Cartesian(1, 1));
				else
					return new Rectangle(Location - Sprite.Origin, Sprite.Size); }
		}

		public bool Intersects(GameObject other)
		{
			return Mask.Intersects(other.Mask);
		}

		public Transform Transform
		{
			get
			{
				return new Transform(Image.XScale, Image.YScale, Image.Rotation, Location);
			}
		}

		private Mask _mask;
		public Mask Mask
		{
			get
			{
				return new Mask(Transform.Rectangle(BoundingBox));
			}

			set { _mask = value; }
		}

		public virtual void OnBeginStep() { }

		public virtual void OnStep() { }

		public virtual void OnEndStep() { }

		public virtual void OnDestroy() { }

		public virtual void OnDraw()
		{
			if (Sprite != null)
			{
				Draw.Image(X, Y, Image);
				Image.Animate();
			}
		}

		public double Width
		{
#warning Temporary
			get { return Sprite.Width; } ///TEMPORARY///
		}

		public double Height
		{
#warning Temporary
			get { return Sprite.Height; } ///TEMPORARY///
		}
	}
}
