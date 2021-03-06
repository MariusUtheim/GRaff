﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRaff
{
	/// <summary>
	/// Represents a rectangle with real coordinates.
	/// </summary>
	public struct Rectangle : IEquatable<Rectangle>
	{
		/// <summary>
		/// Initializes a new instance of the GRaff.Rectangle structure at the specified (x, y) coordinates and with the specified width and height.
		/// </summary>
		/// <param name="x">The x-coordinate of the top left corner.</param>
		/// <param name="y">The y-coordinate of the top left corner.</param>
		/// <param name="width">The width of the rectangle.</param>
		/// <param name="height">The height of the rectangle.</param>
		public Rectangle(double x, double y, double width, double height)
			: this()
        {
			Left = x;
			Top = y;
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Initializes a new instance of the GRaff.Rectangle structure at the specified location and with the specified size.
		/// </summary>
		/// <param name="location">The location of the top-left corner.</param>
		/// <param name="size">The size of the rectangle.</param>
		public Rectangle(Point location, Vector size) : this(location.X, location.Y, size.X, size.Y) { }

		public static Rectangle Zero { get; } = new Rectangle();


		/// <summary>
		/// Gets the width of this GRaff.Rectangle.
		/// </summary>
		public double Width { get; private set; }

		/// <summary>
		/// Gets the height of this GRaff.Rectangle.
		/// </summary>
		public double Height { get; private set; }

		/// <summary>
		/// Gets the left coordinate of this GRaff.Rectangle.
		/// </summary>
		public double Left { get; private set; }

		/// <summary>
		/// Gets the top coordinate of this GRaff.Rectangle.
		/// </summary>
		public double Top { get; private set; }

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
		public Point TopLeft => new Point(Left, Top); 

		/// <summary>
		/// Gets the location of the top-right corner of this GRaff.Rectangle.
		/// </summary>
		public Point TopRight => new Point(Right, Top); 

		/// <summary>
		/// Gets the location of the bottom-left corner of this GRaff.Rectangle.
		/// </summary>	
		public Point BottomLeft => new Point(Left, Bottom); 

		/// <summary>
		/// Gets the location of the bottom-right corner of this GRaff.Rectangle.
		/// </summary>
		public Point BottomRight => new Point(Right, Bottom);

		/// <summary>
		/// Gets the area of this GRaff.Rectangle.
		/// </summary>
		public double Area => Width * Height;

		/// <summary>
		/// Gets the size of this GRaff.Rectangle.
		/// </summary>
		public Vector Size => new Vector(Width, Height); 

		/// <summary>
		/// Gets the location of the center of this GRaff.Rectangle.
		/// </summary>
		public Point Center => TopLeft + Size / 2;

        /// <summary>
        /// Creates a new GRaff.Rectangle from this GRaff.Rectangle where if Width and/or Height 
        /// are negative, they are flipped and the corresponding coordinates are also offset, so 
        /// that the returned Rectangle still describes the same region.
        /// </summary>
        public Rectangle Abs => new Rectangle(GMath.Min(Left, Right), GMath.Min(Top, Bottom), GMath.Abs(Width), GMath.Abs(Height));

        public IEnumerable<Point> Vertices => new[] { TopLeft, TopRight, BottomRight, BottomLeft };

        public IEnumerable<Line> Edges => new[] { new Line(TopLeft, TopRight), new Line(TopRight, BottomRight), new Line(BottomRight, BottomLeft), new Line(BottomLeft, TopLeft) };

        private bool _Intersects(Rectangle other)
            => !(Left >= other.Right || Top >= other.Bottom || Right <= other.Left || Bottom <= other.Top);

        /// <summary>
        /// Tests whether two GRaff.Rectangle structures intersect.
        /// </summary>
        /// <param name="other">The GRaff.Rectangle to test intersection with.</param>
        /// <returns>true if the two GRaff.Rectangle structures intersect.</returns>
        public bool Intersects(Rectangle other)
            => Abs._Intersects(other.Abs);
		
        private Rectangle? _intersectionAbs(Rectangle other)
        {
            Rectangle thisAbs = this.Abs, otherAbs = other.Abs;
            double l = GMath.Max(thisAbs.Left, otherAbs.Left), r = GMath.Min(thisAbs.Right, otherAbs.Right),
                   t = GMath.Max(thisAbs.Top, otherAbs.Top), b = GMath.Min(thisAbs.Bottom, otherAbs.Bottom);

            if (l > r || t > b)
                return null;
            else
                return new Rectangle(l, t, r - l, b - t);
        }

        public Rectangle? Intersection(Rectangle other)
		{
            var absRes = _intersectionAbs(other);

            if (absRes == null)
                return null;
            var res = absRes.Value;
            if (res.Width == 0 && (res.Left == Right || res.Left == other.Right))
                    return null;
            if (res.Height == 0 && (res.Top == Bottom || res.Top == other.Bottom))
                    return null;

            if (this.Width < 0)
                res = new Rectangle(res.Right, res.Top, -res.Width, res.Height);
            if (this.Height < 0)
                res = new Rectangle(res.Left, res.Bottom, res.Width, -res.Height);

            return res;
		}

        public Point Project(Point pt)
            => new Point(GMath.Median(Left, pt.X, Right), GMath.Median(Top, pt.Y, Bottom));

        /// <summary>
        /// Tests whether this GRaff.Rectangle contains the specified GRaff.PointD.
        /// </summary>
        /// <param name="pt">The GRaff.PointD to test.</param>
        /// <returns>true if this GRaff.Rectangle contains pt.</returns>
        public bool Contains(Point pt)
            => ((Width >= 0 && pt.X >= this.Left && pt.X < this.Right) || (Width < 0 && pt.X > this.Right && pt.X <= this.Left))
            && ((Height >= 0 && pt.Y >= this.Top && pt.Y < this.Bottom) || (Height < 0 && pt.Y > this.Bottom && pt.Y <= this.Top));

		/// <summary>
		/// Converts this GRaff.Rectangle to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GRaff.Rectangle</returns>
		public override string ToString() => $"Rectangle {TopLeft} by {Size}}}";

		public bool Equals(Rectangle other)
			=> Left == other.Left && Top == other.Top && Width == other.Width && Height == other.Height;

        public bool Equals(IntRectangle other)
            => Left == other.Left && Top == other.Top && Width == other.Width && Height == other.Height;

        /// <summary>
        /// Specifies whether this GRaff.Rectangle contans the same location and size as the specified System.Object.
        /// </summary>
        /// <param name="obj">The System.Object to compare to.</param>
        /// <returns>true if obj is a GRaff.Rectangle and has the same location and size as this GRaff.Rectangle.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is Rectangle rect)
                return Equals(rect);
            else if (obj is IntRectangle intRect)
                return Equals(intRect);
            else
                return false;
        }

        /// <summary>
        /// Returns a hash code for this GRaff.Rectangle.
        /// </summary>
        /// <returns>An integer value that specifies a hash value for this GRaff.Rectangle.</returns>
        public override int GetHashCode()
			=> GMath.HashCombine(Left.GetHashCode(), Top.GetHashCode(), Width.GetHashCode(), Height.GetHashCode());

		/// <summary>
		/// Compares two GRaff.Rectangle objects. The result specifies whether their locations and sizes are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Rectangle to compare.</param>
		/// <param name="right">The second GRaff.Rectangle to compare.</param>
		/// <returns>true if the locations and sizes of the two GRaff.Rectangle structures are equal.</returns>
		public static bool operator ==(Rectangle left, Rectangle right) => left.Equals(right);

		/// <summary>
		/// Compares two GRaff.Rectangle objects. The result specifies whether their locations and sizes are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.Rectangle to compare.</param>
		/// <param name="right">The second GRaff.Rectangle to compare.</param>
		/// <returns>true if the locations and sizes of the two GRaff.Rectangle structures are unequal.</returns>
		public static bool operator !=(Rectangle left, Rectangle right) => !left.Equals(right);

		/// <summary>
		/// Translates a GRaff.Rectangle by a given GRaff.Vector.
		/// </summary>
		/// <param name="r">The GRaff.Rectangle to translate.</param>
		/// <param name="v">The GRaff.Vector to translate by.</param>
		/// <returns>The translated GRaff.Rectangle.</returns>
		public static Rectangle operator +(Rectangle r, Vector v) 
			=> new Rectangle(r.TopLeft + v, r.Size);

		/// <summary>
		/// Translates a GRaff.Rectangle by subtracting a given GRaff.Vector.
		/// </summary>
		/// <param name="r">The GRaff.Rectangle to translate.</param>
		/// <param name="v">The negative GRaff.Vector to translate by.</param>
		/// <returns>The translated GRaff.Rectangle.</returns>
		public static Rectangle operator -(Rectangle r, Vector v) 
			=> new Rectangle(r.TopLeft - v, r.Size);

        public static implicit operator Rectangle((Point location, Vector size) r) => new Rectangle(r.location, r.size);
        public static implicit operator (Point location, Vector size) (Rectangle r) => (r.TopLeft, r.Size);


	}
}
