using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	public abstract class GameObject
	{
		public GameObject(double x, double y)
		{
			Instance.Add(this);
			Transform = new Transform();
			X = x;
			Y = y;
			Image = new Image(this);
			Mask = new Mask(this);
		}

		public GameObject(Point location)
			: this(location.X, location.Y) { }

		public double X
		{
			get { return Transform.X; }
			set { Transform.X = value; }
		}

		public double Y
		{
			get { return Transform.Y; }
			set { Transform.Y = value; }
		}

		public Point Location
		{
			get { return new Point(Transform.X, Transform.Y); }
			set { Transform.X = value.X; Transform.Y = value.Y; }
		}

		private int _depth;
		public int Depth
		{
			get { return _depth; }
			set { _depth = value; Instance.Sort(); }
		}

		public Image Image { get; private set; }

		public void Destroy()
		{
			OnDestroy();
			Instance.Remove(this);
		}

		private Sprite _sprite;
		public Sprite Sprite
		{
			get { return _sprite; }
			set { _sprite = value; Mask.Update(); }
		}

		public Transform Transform { get; private set; }

		public bool Intersects(GameObject other)
		{
			return Mask.Intersects(other.Mask);
		}

		public Mask Mask
		{
			get;
			private set;
		}

		public Rectangle BoundingBox
		{
			get { 
#warning TODO: Check implementation
				return Mask.BoundingBox; }
		}

		public virtual void OnBeginStep() { }

		public virtual void OnStep() { }

		public virtual void OnEndStep() { }

		public virtual void OnDestroy() { }

		public virtual void OnDraw()
		{
			if (Image.Sprite != null)
			{
				Draw.Image(Image);
				if (Image.Animate() && this is IAnimationEndListener)
					(this as IAnimationEndListener).AnimationEnd();
			}
		}


		public override string ToString()
		{
			return String.Format("{0} at {1}", GetType().Name, Location);
		}
	}
}
