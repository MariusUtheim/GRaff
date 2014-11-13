using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public struct Line
	{
		public Line(Point origin, Vector direction)
			: this()
		{
			this.Origin = origin;
			this.Direction = direction;
		}

		public Line(Point from, Point to)
			: this()
		{
			this.Origin = from;
			this.Destination = to;
		}

		public Point Origin { get; set; }

		public Vector Direction { get; set; }

		public Point Destination
		{
			get { return Origin + Direction; }
			set { Direction = value - Origin; }
		}

		public Vector LeftNormal { get { return new Vector(1, Direction.Direction - Angle.Deg(90)); } }


		public Vector RightNormal { get { return new Vector(1, Direction.Direction + Angle.Deg(90)); } }


		public override string ToString() { return String.Format("Line from {0} to {1}", Origin, Destination); }

		public override bool Equals(object obj) { return (obj is Line) ? (this == (Line)obj) : base.Equals(obj); }

		public override int GetHashCode() { return Origin.GetHashCode() ^ Direction.GetHashCode(); }

		/// <summary>
		/// Compares two GRaff.Line structures. The result specifies whether they are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Line to compare.</param>
		/// <param name="right">The second GRaff.Line to compare.</param>
		/// <returns>true if the two GRaff.Line structures are equal.</returns>
		/// <remarks>Since lines are directed and there is a distinction between the left and right normals, the line from point a to b is not equal to the line from point b to a.</remarks>
		public static bool operator ==(Line left, Line right) { return left.Origin == right.Origin && left.Direction == right.Direction; }

		public static bool operator !=(Line left, Line right) { return left.Origin != right.Origin || left.Direction != right.Direction; }

		public static Line operator +(Line l, Vector v) { return new Line(l.Origin + v, l.Direction); }

		public static Line operator -(Line l, Vector v) { return new Line(l.Origin - v, l.Direction); }


	}
}
