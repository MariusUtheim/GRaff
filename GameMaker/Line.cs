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

		public Point Origin { get; set; }

		public Vector Direction { get; set; }

		public Vector LeftNormal
		{
			get
			{
				return Vector.Polar(1, Direction.Direction - GMath.Tau / 4);
			}
		}

		public Vector RightNormal
		{
			get
			{
				return Vector.Polar(1, Direction.Direction + GMath.Tau / 4);
			}
		}

	}
}
