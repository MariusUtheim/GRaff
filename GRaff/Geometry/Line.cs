using System;


namespace GRaff
{
	/// <summary>
	/// Represents a directed line segment between two points.
	/// </summary>
	public struct Line : IEquatable<Line>
	{
		public Line(double xOrigin, double yOrigin, double xDestination, double yDestination)
			: this(new Point(xOrigin, yOrigin), new Vector(xDestination - xOrigin, yDestination - yOrigin))
		{ }

		/// <summary>
		/// Creates a new instance of the GRaff.Line structure, using the specified origin point and direction.
		/// </summary>
		/// <param name="origin">One endpoint of the line.</param>
		/// <param name="direction">A GRaff.Vector specifying the direction and length of the line.</param>
		public Line(Point origin, Vector direction)
			: this()
		{
			this.Origin = origin;
			this.Direction = direction;
		}

		/// <summary>
		/// Creates a new instance of the GRaff.Line structure, with the specified endpoints.
		/// </summary>
		/// <param name="from">The origin endpoint.</param>
		/// <param name="to">The destination endpoint.</param>
		public Line(Point from, Point to)
			: this()
		{
			this.Origin = from;
			this.Destination = to;
		}

		/// <summary>
		/// Gets the origin endpoint of this GRaff.Line.
		/// </summary>
		public Point Origin { get; private set; }

		/// <summary>
		/// Gets the GRaff.Vector specifying the direction and length of this GRaff.Line.
		/// </summary>
		public Vector Direction { get; private set; }

		/// <summary>
		/// Gets the destination endpoint of this GRaff.Line.
		/// </summary>
		public Point Destination
		{
			get { return Origin + Direction; }
			private set { Direction = value - Origin; }
		}

        public bool IsDegenerate => Direction == Vector.Zero;

		/// <summary>
		/// Gets the left normal vector of this GRaff.Line.
		/// </summary>
		public Vector LeftNormal => new Vector(1, Direction.Direction - Angle.Deg(90));

		/// <summary>
		/// Gets the right normal vector of this GRaff.Line. 
		/// </summary>
		public Vector RightNormal => new Vector(1, Direction.Direction + Angle.Deg(90));

        public Vector Project(Vector v) =>
                        (v.Magnitude == 0 || Direction.Magnitude == 0)
                        ? Vector.Zero
                        : new Vector(v.Dot(Direction) / Direction.Magnitude, Direction.Direction);

        public Point Project(Point p) => Origin + Project(p - Origin);

        public Line Project(Line l) => new Line(Project(l.Origin), Project(l.Direction));

        private bool _isPointAdjacent(Point p)
        {
            var d = Direction.UnitVector.Dot(p - Origin);
            return d >= 0 && d <= Direction.Magnitude;
        }

        /// <summary>
        /// Returns whether this line intersects the other.
        /// Edge cases such as if the endpoint of one line lies on the other line 
        /// have no definite behaviour.
        /// </summary>
        public bool Intersects(Line other)
        {
            var n = LeftNormal;
            var h = n.Dot(other.Origin - Origin);

            if (h * n.Dot(other.Destination - Origin) >  0)
                return false;
            else if (h * n.Dot(other.Destination - Origin) == 0)
            {
                if ((Direction.Dot(other.Origin - Origin) < 0 && Direction.Dot(other.Destination - Origin) < 0)
                 || (Direction.Dot(other.Origin - Destination) > 0 && Direction.Dot(other.Destination - Destination) > 0))
                    return false;
                else
                    return true;
            }

            var angle = other.Direction.Angle(this.Direction);
        
            if (angle.Degrees == 0 || angle.Degrees == 180)
            {
                if (h != 0)
                    return false;
                return _isPointAdjacent(other.Origin) || _isPointAdjacent(other.Destination);
            }

            var sign = n.Dot(other.Direction) > 0 ? 1 : -1;
            var p = other.Origin - sign * new Vector(h / GMath.Sin(angle), angle);
            return _isPointAdjacent(p);
        }

		/// <summary>
		/// Converts this GRaff.Line to a human-readable string, indicating the location of the two endpoints.
		/// </summary>
		/// <returns>A string that represents this GRaff.Line.</returns>
		public override string ToString() => $"{Origin} -> {Destination}";

		public bool Equals(Line other)
			=> Origin == other.Origin && Direction == other.Direction;

		/// <summary>
		/// Specifies whether this GRaff.Line contains the same origin and destination endpoints as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.Line and has the same origin and destination endpoints as this GRaff.Line.</returns>
		/// <remarks>Since lines are directed and there is a distinction between the left and right normals, the line from point a to b is not equal to the line from point b to a.</remarks>
		public override bool Equals(object obj)
			=> (obj is Line) ? Equals((Line)obj) : base.Equals(obj);

		/// <summary>
		/// Returns a hash code for this GRaff.Line.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.Line.</returns>
		public override int GetHashCode()
			=> GMath.HashCombine(Origin.GetHashCode(), Direction.GetHashCode());

		/// <summary>
		/// Compares two GRaff.Line structures. The result specifies whether they are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Line to compare.</param>
		/// <param name="right">The second GRaff.Line to compare.</param>
		/// <returns>true if the two GRaff.Line structures are equal.</returns>
		public static bool operator ==(Line left, Line right) => left.Equals(right);

		/// <summary>
		/// Compares two GRaff.Line structures. The result specifies whether they are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.Line to compare.</param>
		/// <param name="right">The second GRaff.Line to compare.</param>
		/// <returns>true if the two GRaff.Line structures are unequal.</returns>
		public static bool operator !=(Line left, Line right) => !left.Equals(right);

		/// <summary>
		/// Translates the GRaff.Line by a specified GRaff.Vector.
		/// </summary>
		/// <param name="l">The GRaff.Line to be translated.</param>
		/// <param name="v">The GRaff.Vector to translate by.</param>
		/// <returns>The translated GRaff.Line.</returns>
		public static Line operator +(Line l, Vector v) 
			=> new Line(l.Origin + v, l.Direction);

		/// <summary>
		/// Translates the GRaff.Line by a specified GRaff.Vector.
		/// </summary>
		/// <param name="l">The GRaff.Point to be translated.</param>
		/// <param name="v">The negative GRaff.Vector to translate by.</param>
		/// <returns>The translated GRaff.Line.</returns>
		public static Line operator -(Line l, Vector v) 
			=> new Line(l.Origin - v, l.Direction);


	}
}
