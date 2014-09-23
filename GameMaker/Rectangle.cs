using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	/// <summary>
	/// Represents a rectangle with real coordinates.
	/// </summary>
	public struct Rectangle(double x, double y, double width, double height)
	{

		/// <summary>
		/// Initializes a new instance of the GameMaker.Rectangle class at the specified location and with the specified size.
		/// </summary>
		/// <param name="location">The location of the top-left corner.</param>
		/// <param name="size">The size of the rectangle.</param>
		public Rectangle(Point location, Vector size) : this(location.X, location.Y, size.X, size.Y) { }

		/// <summary>
		/// Gets the width of this GameMaker.Rectangle.
		/// </summary>
		public double Width { get; } = width;

		/// <summary>
		/// Gets the height of this GameMaker.Rectangle.
		/// </summary>
		public double Height { get; } = height;

		/// <summary>
		/// Gets the left coordinate of this GameMaker.Rectangle.
		/// </summary>
		public double Left { get; } = x;

		/// <summary>
		/// Gets the top coordinate of this GameMaker.Rectangle.
		/// </summary>
		public double Top { get; } = y;

		/// <summary>
		/// Gets the right coordinate of this GameMaker.Rectangle.
		/// </summary>
		public double Right => Left + Width;

		/// <summary>
		/// Gets the bottom coordinate of this GameMaker.Rectangle.
		/// </summary>
		public double Bottom => Top + Height;

		/// <summary>
		/// Gets the location of the top-left corner of this GameMaker.Rectangle.
		/// </summary>
		public Point Location => new Point(Left, Top);

		/// <summary>
		/// Gets the size of this GameMaker.Rectangle.
		/// </summary>
		public Vector Size => new Vector(Width, Height);

		/// <summary>
		/// Gets the location of the center of this GameMaker.Rectangle.
		/// </summary>
		public Point Center => Location + Size / 2;

		/// <summary>
		/// Tests whether two GameMaker.Rectangle structures intersect.
		/// </summary>
		/// <param name="other">The GameMaker.Rectangle to test intersection with.</param>
		/// <returns>true if the two GameMaker.Rectangle structures intersect.</returns>
		public bool Intersects(Rectangle other)
		{
			return !(Left > other.Right || Top > other.Bottom || Right < other.Left || Bottom < other.Top);
		}

		/// <summary>
		/// Finds the intersection of this and the other GameMaker.Rectangle structures. That is, a new GameMaker.Rectangle 
		/// representing the area they overlap.
		/// </summary>
		/// <param name="other">The other GameMaker.Rectangle to find intersection with.</param>
		/// <returns>A GameMaker.Rectangle representing the intersection of the two GameMaker.Rectangle structures.</returns>
		public Rectangle Intersection(Rectangle other)
		{
			if (!this.Intersects(other))
				return new Rectangle(0, 0, 0, 0);

			throw new NotImplementedException();
			//return new Rectangle { Left = GMath.Max(this.Left, other.Left), Top = GMath.Max(this.Top, other.Top),
			//					   Right = GMath.Min(this.Right, other.Right), Bottom = GMath.Min(this.Bottom, other.Bottom) };
		}

		/// <summary>
		/// Tests whether this GameMaker.Rectangle contains the specified GameMaker.Point.
		/// </summary>
		/// <param name="pt">The GameMaker.Point to test.</param>
		/// <returns>true if this GameMaker.Rectangle contains pt.</returns>
		public bool ContainsPoint(Point pt)
		{
			return pt.X >= this.Left && pt.Y >= this.Top && pt.X < this.Right + this.Width && pt.Y < this.Bottom;
		}

		/// <summary>
		/// Converts this GameMaker.Rectangle to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GameMaker.Rectangle</returns>
		public override string ToString() => String.Format("{{Rectangle {0} by {1}}}", Location, Size);


		public override bool Equals(object obj) => (obj is Rectangle) ? (this == (Rectangle)obj) : base.Equals(obj);

		public override int GetHashCode() => Left.GetHashCode() ^ Top.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();

		public static bool operator ==(Rectangle left, Rectangle right) => (left.Left == right.Left && left.Top == right.Top && left.Width == right.Width && left.Height == right.Height);

		public static bool operator !=(Rectangle left, Rectangle right) => (left.Left != right.Left || left.Top != right.Top || left.Width == right.Width || left.Height == right.Height);


		/// <summary>
		/// Translates a GameMaker.Rectangle by a given GameMaker.Vector.
		/// </summary>
		/// <param name="r">The GameMaker.Rectangle to translate.</param>
		/// <param name="v">The GameMaker.Vector to translate by.</param>
		/// <returns>The translated GameMaker.Rectangle.</returns>
		public static Rectangle operator +(Rectangle r, Vector v) => new Rectangle(r.Location + v, r.Size);
		

		/// <summary>
		/// Translates a GameMaker.Rectangle by subtracting a given GameMaker.Vector.
		/// </summary>
		/// <param name="r">The GameMaker.Rectangle to translate.</param>
		/// <param name="v">The negative GameMaker.Vector to translate by.</param>
		/// <returns>The translated GameMaker.Rectangle.</returns>
		public static Rectangle operator -(Rectangle r, Vector v) => new Rectangle(r.Location - v, r.Size);
	}
}
