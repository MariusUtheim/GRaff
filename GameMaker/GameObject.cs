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
			Image = new Image(this.Sprite);
		}

		public double X { get; set; }
		public double Y { get; set; }

		public Point Location
		{
			get { return new Point(X, Y); }
			set { X = value.X; Y = value.Y; }
		}

		public Image Image { get; set; }

		public int Depth { get; set; } // TEMPORARY

		public void Destroy()
		{
			OnDestroy();
			Instance.Remove(this);
		}

		public virtual Rectangle BoundingBox
		{
			get { return new Rectangle(Location - Sprite.Origin, Sprite.Size); }
		}

		public bool Intersects(GameObject other)
		{
			return Mask.Intersects(other.Mask);
		}

		private Sprite _sprite;
		public Sprite Sprite
		{
			get { return _sprite; }
			set { Image.Sprite = _sprite = value; }
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

		public virtual void BeginStep() { }

		public virtual void Step() { }

		public virtual void EndStep() { }

		public virtual void OnDestroy() { }

		public virtual void OnDraw()
		{
			if (Sprite != null)
				Draw.Image(X, Y, Image);
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
