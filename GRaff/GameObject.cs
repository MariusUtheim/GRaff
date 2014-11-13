using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRaff
{
	/// <summary>
	/// Represents the most general game object that is handled in the game. Most instances would inherit from this class.
	/// </summary>
	public abstract class GameObject : GameElement
	{
		/// <summary>
		/// Initializes a new instance of the GRaff.GameObject class with the specified x- and y-coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		protected GameObject(double x, double y)
		{
			Transform = new Transform();
			X = x;
			Y = y;
			Image = new Image(this);
			Mask = new Mask(this);
		}

		/// <summary>
		/// Initializes a new instance of the GRaff.GameObject class at the specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		protected GameObject(Point location)
			: this(location.X, location.Y) { }

		/// <summary>
		/// Gets or sets the x-coordinate of this GRaff.GameObject.
		/// </summary>
		public double X
		{
			get { return Transform.X; }
			set { Transform.X = value; }
		}

		/// <summary>
		/// Gets or sets the y-coordinate of this GRaff.GameObject.
		/// </summary>
		public double Y
		{
			get { return Transform.Y; }
			set { Transform.Y = value; }
		}

		/// <summary>
		/// Gets or sets the location of this GRaff.GameObject.
		/// </summary>
		public Point Location
		{
			get { return new Point(Transform.X, Transform.Y); }
			set { Transform.X = value.X; Transform.Y = value.Y; }
		}


		public Image Image { get; private set; }


		public Sprite Sprite
		{
			get { return Image.Sprite; }
			set { Image.Sprite = value; }
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
		/// An action that is performed at the end of each step.
		/// </summary>
		public virtual void OnEndStep() { }

		/// <summary>
		/// An action that is performed when the instance is drawn. Calls to methods in the static classes GRaff.Draw and
		/// GRaff.Fill will generally not have any effect unless called inside the draw event.
		/// </summary>
		public override void OnDraw()
		{
			if (Image.Sprite != null)
			{
				Draw.Image(Image);
				if (Image.Animate() && this is IAnimationEndListener)
					(this as IAnimationEndListener).AnimationEnd();
			}
		}
	}
}
