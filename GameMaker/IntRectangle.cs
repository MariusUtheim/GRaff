using System;


namespace GameMaker
{
	/// <summary>
	/// Represents a rectangle with integer coordinates.
	/// </summary>
	public struct IntRectangle(int x, int y, int width, int height)
	{

		/// <summary>
		/// Initializes a new instance of the GameMaker.IntRectangle class at the specified location and with the specified size.
		/// </summary>
		/// <param name="location">The location of the top-left corner.</param>
		/// <param name="size">The size of the rectangle.</param>
		public IntRectangle(IntVector location, IntVector size)
			: this(location.X, location.Y, size.X, size.Y) { }

		/// <summary>
		/// Gets the width of this GameMaker.IntRectangle.
		/// </summary>
		public int Width { get; } = width;

		/// <summary>
		/// Gets the height of this GameMaker.IntRectangle.
		/// </summary>
		public int Height { get; } = height;

		/// <summary>
		/// Gets the left coordinate of this GameMaker.IntRectangle.
		/// </summary>
		public int Left { get; } = x;

		/// <summary>
		/// Gets the top coordinate of this GameMaker.IntRectangle.
		/// </summary>
		public int Top { get; } = y;

		/// <summary>
		/// Gets the right coordinate of this GameMaker.IntRectangle.
		/// </summary>
		public int Right => Left + Width;


		/// <summary>
		/// Gets the bottom coordinate of this GameMaker.IntRectangle.
		/// </summary>
		public int Bottom => Top + Height;


		/// <summary>
		/// Gets or sets the location of the top-left corner of this GameMaker.IntRectangle.
		/// </summary>
		public IntVector Location => new IntVector(Left, Top);


		/// <summary>
		/// Gets or sets the size of this GameMaker.IntRectangle.
		/// </summary>
		public IntVector Size => new IntVector(Width, Height);

		/// <summary>
		/// Tests whether two rectangles intersect.
		/// </summary>
		/// <param name="other">The GameMaker.IntRectangle to test intersection with.</param>
		/// <returns>True if the two rectangles intersect.</returns>
		public bool Intersects(IntRectangle other)
		{
			return !(Left > other.Right || Top > other.Bottom || Right < other.Left || Bottom < other.Top);
		}


		/// <summary>
		/// Converts this GameMaker.IntRectangle to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GameMaker.IntRectangle</returns>
		public override string ToString() => String.Format("IntRectangle: [({0},{1}), ({2},{3})]", Left, Top, Width, Height);


		/// <summary>
		/// Translates a GameMaker.IntRectangle by a given GameMaker.IntVector.
		/// </summary>
		/// <param name="r">The GameMaker.IntRectangle to translate.</param>
		/// <param name="v">The GameMaker.IntVector to translate by.</param>
		/// <returns>The translated GameMaker.IntRectangle.</returns>
		public static IntRectangle operator +(IntRectangle r, IntVector v) => new IntRectangle(r.Location + v, r.Size);
		

		/// <summary>
		/// Translates a GameMaker.IntRectangle by subtracting a given GameMaker.IntVector.
		/// </summary>
		/// <param name="r">The GameMaker.IntRectangle to translate.</param>
		/// <param name="v">The negative GameMaker.IntVector to translate by.</param>
		/// <returns>The translated GameMaker.IntRectangle.</returns>
		public static IntRectangle operator -(IntRectangle r, IntVector v) => new IntRectangle(r.Location - v, r.Size);


		/// <summary>
		/// Converts the specified GameMaker.IntRectangle to a GameMaker.Rectangle.
		/// </summary>
		/// <param name="r">The GameMaker.IntRectangle to be converted</param>
		/// <returns>The GameMaker.Rectangle that results from the conversion.</returns>
		public static implicit operator Rectangle(IntRectangle r) => new Rectangle(r.Left, r.Top, r.Width, r.Height);
	}
}
