using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public sealed class Mask
	{
		private Point[] _pts;
		private Image _target;
		private bool _targetBased;

		internal Mask(GameObject owner)
		{
			_target = owner.Image;
			_targetBased = true;
			Transform = owner.Transform;
		}

		public Transform Transform
		{
			get;
			private set;
		}

		public MaskShape Shape
		{
			set { this._pts = value._pts; }
		}

		public bool ContainsPoint(Point pt)
		{
			/**
			 * If the polygon is convex then one can consider the polygon as a "path" from the first vertex. 
			 * A point is on the interior of this polygons if it is always on the same side of all the line segments making up the path.
			 *Given a line segment between P0 (x0,y0) and P1 (x1,y1), another point P (x,y) has the following relationship to the line segment:
			 *
			 * Compute (y - y0) (x1 - x0) - (x - x0) (y1 - y0) 
			 * if it is less than 0 then P is to the right of the line segment, 
			 * if greater than 0 it is to the left, if equal to 0 then it lies on the line segment.
			 * */
			foreach (Line L in Path)
				if (L.LeftNormal.DotProduct(pt - L.Origin) > 0)
					return false;

			return true;
		}

		public bool ContainsPoint(double x, double y)
		{
			return ContainsPoint(new Point(x, y));
		}

		public bool Intersects(Mask other)
		{
			return _Intersects(other) || other._Intersects(this);
		}


		private IEnumerable<Line> Path
		{
			get
			{
				if (_pts.Length == 0)
					yield break;

				for (int i = 0; i < _pts.Length - 1; i++)
				{
					Line theLine = Transform.Line(new Line(_pts[i], _pts[i + 1]));
					yield return theLine;
				}
				yield return Transform.Line(new Line(_pts.Last(), _pts.First() - _pts.Last()));
			}
		}

		private bool _Intersects(Mask other)
		{
			/**
			 * Using the separation axis theorem: 
			 * http://stackoverflow.com/questions/753140/how-do-i-determine-if-two-convex-polygons-intersect
			 * */

			foreach (var L in Path)
			{
				var n = L.LeftNormal;
				if (other._pts.Select(pt => other.Transform.Point(pt)).All(pt => n.DotProduct(pt - L.Origin) > 0))
					return false;
			}

			return true;
		}

		public void DrawOutline()
		{ 
			DrawOutline(Color.Black);
		}

		public void DrawOutline(Color color)
		{
			foreach (Line line in Path)
			{
				Draw.Line(Color.Red, line.Origin + 2 * line.Direction.Normal, line.Origin + 2 * (line.LeftNormal + line.Direction.Normal));
				Draw.Line(color, line.Origin, line.Destination);
			}
		}
	}




}
