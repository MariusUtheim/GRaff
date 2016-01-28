using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

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

		[ContractInvariantMethod]
		private void objectInvariants()
		{
			Contract.Invariant(Transform != null);
			Contract.Invariant(Image != null);
			Contract.Invariant(Mask != null);
		}

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
			=> other == null || Mask.Intersects(other.Mask);

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
			if (Image.Sprite != null)
			{
				Draw.Image(Image);
				if (Image.Animate())
					(this as IAnimationEndListener)?.AnimationEnd();
			}
		}
	}
}
