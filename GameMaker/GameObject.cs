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

		public int Depth { get; set; } ///TEMPORARY///

		public void Destroy()
		{
			OnDestroy();
			Instance.Remove(this);
		}

		public virtual Rectangle BoundingBox
		{
			get { return new Rectangle(Location - Sprite.Origin, Sprite.Size); }
		}

		public abstract Sprite Sprite { get; }

		public virtual bool Intersects(Rectangle rect)
		{
			return BoundingBox.Intersects(rect);
		}

		public virtual void BeginStep() { }

		public virtual void Step() { }

		public virtual void EndStep() { }

		public virtual void OnDestroy() { }

		public virtual void OnDraw()
		{
			if (Image != null)
				Draw.Image(X, Y, Image);
		}

		public double Width
		{
			get { return Sprite.Width; } ///TEMPORARY///
		}

		public double Height
		{
			get { return Sprite.Height; } ///TEMPORARY///
		}


		internal bool Intersects(GameObject other)
		{
			return Intersects(other.BoundingBox);
		}
	}
}
