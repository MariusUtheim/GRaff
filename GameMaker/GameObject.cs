using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	/// <summary>
	/// Represents the most general game object that is handled in the game. Most instances would inherit from this class.
	/// </summary>
	public abstract class GameObject
	{
		/// <summary>
		/// Initializes a new instance of the GameMaker.GameObject class with the specified x- and y-coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		protected GameObject(double x, double y)
		{
			Instance.Add(this);
			Transform = new Transform();
			X = x;
			Y = y;
			Image = new Image(this);
			Mask = new Mask(this);
		}

		/// <summary>
		/// Initializes a new instance of the GameMaker.GameObject class at the specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		protected GameObject(Point location)
			: this(location.X, location.Y) { }

		/// <summary>
		/// Gets or sets the x-coordinate of this GameMaker.GameObject.
		/// </summary>
		public double X
		{
			get { return Transform.X; }
			set { Transform.X = value; }
		}

		/// <summary>
		/// Gets or sets the y-coordinate of this GameMaker.GameObject.
		/// </summary>
		public double Y
		{
			get { return Transform.Y; }
			set { Transform.Y = value; }
		}

		/// <summary>
		/// Gets or sets the location of this GameMaker.GameObject.
		/// </summary>
		public Point Location
		{
			get { return new Point(Transform.X, Transform.Y); }
			set { Transform.X = value.X; Transform.Y = value.Y; }
		}

		private int _depth;
		/// <summary>
		/// Gets or sets the depth of this GameMaker.GameObject.
		/// Instances with higher depth take actions before and are drawn behind instances with lower depth.
		/// Changes to depth value are not reflected in the game before a new frame is drawn.
		/// </summary>
		public int Depth
		{
			get { return _depth; }
			set { _depth = value; Instance.NeedsSort = true; }
		}

		public Image Image { get; private set; }

		/// <summary>
		/// Destroys the instance of this GameMaker.GameObject, removing it from the game.
		/// The instance will stop performing automatic actions such as Step and Draw,
		/// but the C# object is not garbage collected while it is still being referenced.
		/// </summary>
		public void Destroy()
		{
			OnDestroy();
			Instance.Remove(this);
		}

		private Sprite _sprite;
		public Sprite Sprite
		{
			get { return _sprite; }
			set { _sprite = value; }
		}

		public Transform Transform { get; private set; }

		public bool Intersects(GameObject other)
		{
			if (other == null) return false;
			return Mask.Intersects(other.Mask);
		}

		public Mask Mask
		{
			get;
			private set;
		}

		public Rectangle BoundingBox
		{
			get { return Mask.BoundingBox; }
		}

		/// <summary>
		/// An action that is performed at the beginning of each step.
		/// </summary>
		public virtual void OnBeginStep() { }

		/// <summary>
		/// An action that is performed each step.
		/// </summary>
		public virtual void OnStep() { }

		/// <summary>
		/// An action that is performed at the end of each step.
		/// </summary>
		public virtual void OnEndStep() { }

		/// <summary>
		/// An action that is performed just before the instance is destroyed.
		/// </summary>
		public virtual void OnDestroy() { }

		/// <summary>
		/// An action that is performed when the instance is drawn. Calls to methods in the static classes GameMaker.Draw and
		/// GameMaker.Fill will generally not have any effect unless called inside the draw event.
		/// </summary>
		public virtual void OnDraw()
		{
			if (Image.Sprite != null)
			{
				Draw.Image(Image);
				if (Image.Animate())
					(this as IAnimationEndListener)?.AnimationEnd();
			}
		}
	}
}
