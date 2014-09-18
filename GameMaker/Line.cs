using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
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

		public Vector LeftNormal => new Vector(1, Direction.Direction - Angle.Deg(90));


		public Vector RightNormal => new Vector(1, Direction.Direction + Angle.Deg(90));


		public override string ToString() => String.Format("Line from {0} to {1}", Origin, Destination);

	}
}
