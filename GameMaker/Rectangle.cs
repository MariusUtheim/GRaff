using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRaff
{
	/// <summary>
	/// Represents a rectangle with real coordinates.
	/// </summary>
	public struct Rectangle(double x, double y, double width, double height)
	{

		/// <summary>
		/// Initializes a new instance of the GRaff.Rectangle class at the specified location and with the specified size.
		/// </summary>
		/// <param name="location">The location of the top-left corner.</param>
		/// <param name="size">The size of the rectangle.</param>
		public Rectangle(Point location, Vector size) : this(location.X, location.Y, size.X, size.Y) { }

		/// <summary>
		/// Gets the width of this GRaff.Rectangle.
		/// </summary>
		public double Width { get; } = width;

		/// <summary>
		/// Gets the height of this GRaff.Rectangle.
		/// </summary>
		public double Height { get; } = height;

		/// <summary>
		/// Gets the left coordinate of this GRaff.Rectangle.
		/// </summary>
		public double Left { get; } = x;

		/// <summary>
		/// Gets the top coordinate of this GRaff.Rectangle.
		/// </summary>
		public double Top { get; } = y;

		/// <summary>
		/// Gets the right coordinate of this GRaff.Rectangle.
		/// </summary>
		public double Right => Left + Width;

		/// <summary>
		/// Gets the bottom coordinate of this GRaff.Rectangle.
		/// </summary>
		public double Bottom => Top + Height;

		/// <summary>
		/// Gets the location of the top-left corner of this GRaff.Rectangle.
		/// </summary>
		public Point Location => new Point(Left, Top);

		/// <summary>
		/// Gets the size of this GRaff.Rectangle.
		/// </summary>
		public Vector Size => new Vector(Width, Height);

		/// <summary>
		/// Gets the location of the center of this GRaff.Rectangle.
		/// </summary>
		public Point Center => Location + Size / 2;

		/// <summary>
		/// Tests whether two GRaff.Rectangle structures intersect.
		/// </summary>
		/// <param name="other">The GRaff.Rectangle to test intersection with.</param>
		/// <returns>true if the two GRaff.Rectangle structures intersect.</returns>
		public bool Intersects(Rectangle other)
		{
			return !(Left > other.Right || Top > other.Bottom || Right < other.Left || Bottom < other.Top);
		}

		/// <summary>
		/// Finds the intersection of this and the other GRaff.Rectangle structures. That is, a new GRaff.Rectangle 
		/// representing the area they overlap.
		/// </summary>
		/// <param name="other">The other GRaff.Rectangle to find intersection with.</param>
		/// <returns>A GRaff.Rectangle representing the intersection of the two GRaff.Rectangle structures.</returns>
		public Rectangle Intersection(Rectangle other)
		{
			if (!this.Intersects(other))
				return new Rectangle(0, 0, 0, 0);

			throw new NotImplementedException();
			//return new Rectangle { Left = GMath.Max(this.Left, other.Left), Top = GMath.Max(this.Top, other.Top),
			//					   Right = GMath.Min(this.Right, other.Right), Bottom = GMath.Min(this.Bottom, other.Bottom) };
		}

		/// <summary>
		/// Tests whether this GRaff.Rectangle contains the specified GRaff.Point.
		/// </summary>
		/// <param name="pt">The GRaff.Point to test.</param>
		/// <returns>true if this GRaff.Rectangle contains pt.</returns>
		public bool ContainsPoint(Point pt)
		{
			return pt.X >= this.Left && pt.Y >= this.Top && pt.X < this.Right + this.Width && pt.Y < this.Bottom;
		}

		/// <summary>
		/// Converts this GRaff.Rectangle to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GRaff.Rectangle</returns>
		public override string ToString() => String.Format("{{Rectangle {0} by {1}}}", Location, Size);

		/// <summary>
		/// Specifies whether this GRaff.Rectangle contains the same location and size as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.Rectangle and has the same location and size as this GRaff.Rectangle.</returns>
		public override bool Equals(object obj) => (obj is Rectangle) ? (this == (Rectangle)obj) : base.Equals(obj);

		/// <summary>
		/// Returns a hash code for this GRaff.Rectangle.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.Rectangle.</returns>
		public override int GetHashCode() => Left.GetHashCode() ^ Top.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();

		/// <summary>
		/// Compares two GRaff.Rectangle objects. The result specifies whether their locations and sizes are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Rectangle to compare.</param>
		/// <param name="right">The second GRaff.Rectangle to compare.</param>
		/// <returns>true if the locations and sizes of the two GRaff.Rectangle structures are equal.</returns>
		public static bool operator ==(Rectangle left, Rectangle right) => (left.Left == right.Left && left.Top == right.Top && left.Width == right.Width && left.Height == right.Height);

		/// <summary>
		/// Compares two GRaff.Rectangle objects. The result specifies whether their locations and sizes are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.Rectangle to compare.</param>
		/// <param name="right">The second GRaff.Rectangle to compare.</param>
		/// <returns>true if the locations and sizes of the two GRaff.Rectangle structures are unequal.</returns>
		public static bool operator !=(Rectangle left, Rectangle right) => (left.Left != right.Left || left.Top != right.Top || left.Width == right.Width || left.Height == right.Height);


		/// <summary>
		/// Translates a GRaff.Rectangle by a given GRaff.Vector.
		/// </summary>
		/// <param name="r">The GRaff.Rectangle to translate.</param>
		/// <param name="v">The GRaff.Vector to translate by.</param>
		/// <returns>The translated GRaff.Rectangle.</returns>
		public static Rectangle operator +(Rectangle r, Vector v) => new Rectangle(r.Location + v, r.Size);
		

		/// <summary>
		/// Translates a GRaff.Rectangle by subtracting a given GRaff.Vector.
		/// </summary>
		/// <param name="r">The GRaff.Rectangle to translate.</param>
		/// <param name="v">The negative GRaff.Vector to translate by.</param>
		/// <returns>The translated GRaff.Rectangle.</returns>
		public static Rectangle operator -(Rectangle r, Vector v) => new Rectangle(r.Location - v, r.Size);
	}
}
