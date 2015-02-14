/** using System;
using System.Collections.Generic;

namespace GRaff.Graphics.Lighting
{
	public class LightSource
	{
		public LightSource(Point location, double radius)
		{
			Location = location;
			Radius = radius;
		}

		public Point Location { get; set; }
		public double Radius { get; set; }

		internal void Render(IEnumerable<LightObstacle> obstacles)
		{
			Draw.FillCircle(Color.White, Location, Radius);

			foreach (var obstacle in obstacles)
			{
				var w1 = obstacle.Wall.Origin;
				var w2 = obstacle.Wall.Destination;

				double d = ((w1 - Location).Direction- (w2 - Location).Direction).Degrees;
			    if (d < 180)
				{
					var tmp = w1;
					w1 = w2;
					w2 = tmp;
				}

				var p1 = Location + Radius * (w1 - Location).UnitVector;
				var p2 = Location + Radius * (w2 - Location).UnitVector;

				var s1 = p1 + new Line(w1, p1).RightNormal * Radius;
				var s2 = p2 + new Line(w2, p2).LeftNormal * Radius;

				Draw.FillTriangle(Color.Black, w1, w2, p1);
				Draw.FillTriangle(Color.Black, w2, p1, p2);
			
				Draw.FillTriangle(Color.Black, p1, p2, s1);
				Draw.FillTriangle(Color.Black, p2, s1, s2);

				Draw.Line(Color.Red, w1, w2);
			}
		}
	}
}

/* */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GRaff.Graphics.Lighting
{
	public class LightSource
	{
		private ColoredRenderSystem _renderSystem = new ColoredRenderSystem();

		public Color Color { get; set; }

		public LightSource(Point location, double radius)
			: this(location, radius, Color.White)
		{ }

		public LightSource(Point location, double radius, Color color)
		{
			Location = location;
			Radius = radius;
			Color = color;
		}

		public Point Location { get; set; }
		public double Radius { get; set; }

		internal void Render(IEnumerable<LightObstacle> obstacles)
		{
			Draw.FillCircle(Color, Location, Radius);

			var vertices = new PointF[obstacles.Count() * 12];
			var colors = new Color[obstacles.Count() * 12];

			//Parallel.ForEach( obstacles, (obstacle, loopState, index) => {
			int index = 0;
			foreach (var obstacle in obstacles)
			{
				var w1 = obstacle.Wall.Origin;
				var w2 = obstacle.Wall.Destination;

				Draw.Circle(Color.Red, w1, 4);
				Draw.Circle(Color.Red, w2, 4);

				var d1 = w1 - Location;
				var d2 = w2 - Location;

				double d = ((w1 - Location).Direction - (w2 - Location).Direction).Degrees;
				if (d < 180)
				{
					var tmp = w1;
					w1 = w2;
					w2 = tmp;
				}

				var p1 = (Location + Radius * (w1 - Location).UnitVector);
				var p2 = (Location + Radius * (w2 - Location).UnitVector);

				var s1 = p1 + new Line(w1, p1).RightNormal * Radius;
				var s2 = p2 + new Line(w2, p2).LeftNormal * Radius;

				Draw.FillTriangle(obstacle.Color, w1, w2, p1);
				Draw.FillTriangle(obstacle.Color, w2, p1, p2);
				Draw.FillTriangle(obstacle.Color, p1, p2, s1);
				Draw.FillTriangle(obstacle.Color, p2, s1, s2);

			/*	var c = index * 12;
				vertices[c++] = (PointF)w1;
				vertices[c++] = (PointF)w2;
				vertices[c++] = (PointF)p1;
				vertices[c++] = (PointF)w2;
				vertices[c++] = (PointF)p1;
				vertices[c++] = (PointF)p2;
				vertices[c++] = (PointF)p1;
				vertices[c++] = (PointF)p2;
				vertices[c++] = (PointF)s1;
				vertices[c++] = (PointF)p2;
				vertices[c++] = (PointF)s1;
				vertices[c++] = (PointF)s2;

				for (var i = index * 12; i < (index + 1) * 12; i++)
					colors[i] = obstacle.Color.Inverse;

				index++;
				*/
			}

			Draw.Circle(Color.Aqua, Location, Radius);

	//		_renderSystem.SetVertices(UsageHint.StreamDraw, vertices);
	//		_renderSystem.SetColors(UsageHint.StreamDraw, colors);

	//		using (new BlendModeContext(BlendMode.Subtractive))
	//			_renderSystem.Render(PrimitiveType.Triangles, vertices.Count());
		}
	}
}