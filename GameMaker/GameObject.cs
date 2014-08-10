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
			Transform = new Transform();
			Image = new Image(this);
			Mask = new Mask(this);
		}

		public GameObject(double x, double y)
			: this()
		{
			X = x;
			Y = y;
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

		public Sprite Sprite
		{
			get;
			set;
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
			get { throw new NotImplementedException(); }
		}

		public virtual void OnBeginStep() { }

		public virtual void OnStep() { }

		public virtual void OnEndStep() { }

		public virtual void OnDestroy() { }

		public virtual void OnDraw()
		{
			if (Image.Sprite != null)
			{
				Draw.Image(X, Y, Image);
				if (Image.Animate() && this is IAnimationEndListener)
					(this as IAnimationEndListener).AnimationEnd();

				Mask.DrawOutline(Color.Red);
			}
		}
	}
}
