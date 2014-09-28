using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	/// <summary>
	/// Represents a GameMaker.GameObject that moves around automatically and keeps track of its previous location.
	/// The motion happens in the begin step, and this method cannot be overridden.
	/// </summary>
	public abstract class MovingObject : GameObject
	{
		/// <summary>
		/// Initializes a new instance of the GameMaker.MovingObject class with the specified x- and y-coordinates.
		/// </summary>
		/// <param name="x">The x-coordinate.</param>
		/// <param name="y">The y-coordinate.</param>
		protected MovingObject(double x, double y)
			: base(x, y) { }

		/// <summary>
		/// Initializes a new instance of the GameMaker.MovingObject class at the specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		protected MovingObject(Point location)
			: base(location) { }

		/// <summary>
		/// Handles the motion of this GameMaker.MovingObject. This method should not be called directly.
		/// The method cannot be overridden by subclasses.
		/// </summary>
		public sealed override void OnBeginStep()
		{
			base.OnBeginStep();
			XPrevious = X;
			YPrevious = Y;

			Location += Velocity + 0.5 * Acceleration;
			Velocity += Acceleration;
			if (Speed <= Friction)
				Speed = 0;
			else
				Speed -= Friction;
		}

		/// <summary>
		/// Gets or sets the x-coordinate of the point representing the location of this GameMaker.MovingObject
		/// at the beginning of this step.
		/// </summary>
		public double XPrevious { get; set; }

		/// <summary>
		/// Gets or sets the y-coordinate of the point representing the location of this GameMaker.MovingObject
		/// at the beginning of this step.
		/// </summary>
		public double YPrevious { get; set; }

		/// <summary>
		/// Gets or sets the location of this GameMaker.MovingObject at the beginning of the step.
		/// </summary>
		public Point PreviousLocation
		{
			get { return new Point(XPrevious, YPrevious); }
			set { XPrevious = value.X; YPrevious = value.Y; }
		}

		/// <summary>
		/// Gets or sets the acceleration of this GameMaker.MovingObject.
		/// </summary>
		public Vector Acceleration { get; set; }

		/// <summary>
		/// Gets or sets the velocity of this GameMaker.MovingObject. 
		/// </summary>
		public Vector Velocity { get; set; }

		/// <summary>
		/// Gets or sets the horizontal component of the velocity of this GameMaker.MovingObject.
		/// </summary>
		public double HSpeed
		{
			get { return Velocity.X; }
			set { Velocity = new Vector(value, VSpeed); }
		}

		/// <summary>
		/// Gets or sets the vertical component of the velocity of this GameMaker.MovingObject.
		/// </summary>
		public double VSpeed
		{
			get { return Velocity.Y; }
			set { Velocity = new Vector(HSpeed, value); }
		}

		/// <summary>
		/// Gets or sets the speed of this GameMaker.MovingObject. That is, the magnitude of its velocity.
		/// </summary>
		public double Speed
		{
			get { return Velocity.Magnitude; }
			set { Velocity = new Vector(value, Direction); }
		}

		/// <summary>
		/// Gets or sets the direction in which this GameMaker.MovingObject is moving.
		/// </summary>
		public Angle Direction
		{
			get { return Velocity.Direction; }
			set { Velocity = new Vector(Speed, value); ; }
		}

		/// <summary>
		/// Gets or sets the friction of this GameMaker.MovingObject.
		/// Each step, the speed of this GameMaker.MovingObject is reduced by the friction value.
		/// </summary>
		public double Friction { get; set; }

	}
}
