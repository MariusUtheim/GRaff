using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameMaker
{
	/// <summary>
	/// Represents a rectangle with real coordinates.
	/// </summary>
	public struct Rectangle
	{
		/// <summary>
		/// Initializes a new instance of the GameMaker.Rectangle class with the specified coordinates, width and height.
		/// </summary>
		/// <param name="x">The x-coordinate of the top-left corner.</param>
		/// <param name="y">The y-coordinate of the top-left corner.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public Rectangle(double x, double y, double width, double height)
			: this()
		{
			this.Left = x;
			this.Top = y;
			this.Width = width;
			this.Height = height;
		}

		/// <summary>
		/// Initializes a new instance of the GameMaker.Rectangle class at the specified location and with the specified size.
		/// </summary>
		/// <param name="location">The location of the top-left corner.</param>
		/// <param name="size">The size of the rectangle.</param>
		public Rectangle(Point location, Vector size)
			: this(location.X, location.Y, size.X, size.Y) { }

		/// <summary>
		/// Gets or sets the width of this GameMaker.Rectangle.
		/// </summary>
		public double Width { get; set; }

		/// <summary>
		/// Gets or sets the height of this GameMaker.Rectangle.
		/// </summary>
		public double Height { get; set; }

		/// <summary>
		/// Gets or sets the left coordinate of this GameMaker.IntRectangle.
		/// </summary>
		public double Left
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the top coordinate of this GameMaker.IntRectangle.
		/// </summary>
		public double Top
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the right coordinate of this GameMaker.IntRectangle.
		/// </summary>
		public double Right
		{
			get { return Left + Width; }
			set	{ Left = value - Width; }
		}

		/// <summary>
		/// Gets or sets the bottom coordinate of this GameMaker.IntRectangle.
		/// </summary>
		public double Bottom
		{
			get { return Top + Height;  }
			set { Top = value - Height; }
		}

		/// <summary>
		/// Gets or sets the location of the top-left corner of this GameMaker.IntRectangle.
		/// </summary>
		public Point Location
		{
			get { return new Point(Left, Top); }
			set { Left = value.X; Top = value.Y; }
		}

		/// <summary>
		/// Gets or sets the size of this GameMaker.IntRectangle.
		/// </summary>
		public Vector Size
		{
			get { return new Vector(Width, Height); }
			set { Width = value.X; Height = value.Y; }
		}

		/// <summary>
		/// Gets or sets the location of the center of this GameMaker.Rectangle.
		/// </summary>
		public Point Center
		{
			get { return Location + Size / 2; }
			set { Location = value - Size / 2; }
		}

		/// <summary>
		/// Tests whether two rectangles intersect.
		/// </summary>
		/// <param name="other">The GameMaker.Rectangle to test intersection with.</param>
		/// <returns>True if the two rectangles intersect.</returns>
		public bool Intersects(Rectangle other)
		{
			return !(Left > other.Right || Top > other.Bottom || Right < other.Left || Bottom < other.Top);
		}

		/// <summary>
		/// Converts this GameMaker.IntRectangle to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GameMaker.IntRectangle</returns>
		public override string ToString()
		{
			return String.Format("{{Rectangle {0} by {1}}}", Location, Size);
		}

		/// <summary>
		/// Translates a GameMaker.Rectangle by a given GameMaker.Vector.
		/// </summary>
		/// <param name="r">The GameMaker.Rectangle to translate.</param>
		/// <param name="v">The GameMaker.Vector to translate by.</param>
		/// <returns>The translated GameMaker.Rectangle.</returns>
		public static Rectangle operator +(Rectangle r, Vector v)
		{
			return new Rectangle(r.Location + v, r.Size);
		}

		/// <summary>
		/// Translates a GameMaker.Rectangle by subtracting a given GameMaker.Vector.
		/// </summary>
		/// <param name="r">The GameMaker.Rectangle to translate.</param>
		/// <param name="v">The negative GameMaker.Vector to translate by.</param>
		/// <returns>The translated GameMaker.Rectangle.</returns>
		public static Rectangle operator -(Rectangle r, Vector v)
		{
			return new Rectangle(r.Location - v, r.Size);
		}
	}
}
