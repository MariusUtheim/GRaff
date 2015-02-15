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
			//Draw.FillCircle(Color, Location, Radius);
			Draw.FillCircle(Color, Color.Black, Location, Radius);

			var vertices = new PointF[obstacles.Count() * 12];
			var colors = new Color[obstacles.Count() * 12];

			//Parallel.ForEach( obstacles, (obstacle, loopState, index) => {
			using (new BlendModeContext(new BlendMode(BlendEquation.Add, BlendingFactor.Zero, BlendingFactor.SrcColor)))
			{
				foreach (var obstacle in obstacles)
				{
					var w1 = obstacle.Wall.Origin;
					var w2 = obstacle.Wall.Destination;

					double d = ((w1 - Location).Direction - (w2 - Location).Direction).Degrees;
					if (d < 180)
					{
						var tmp = w1;
						w1 = w2;
						w2 = tmp;
					}

					var d1 = (w1 - Location);
					var d2 = (w2 - Location);

					var p1 = (Location + Radius * (w1 - Location).UnitVector);
					var p2 = (Location + Radius * (w2 - Location).UnitVector);

					var s1 = p1 + (d1.Magnitude < Radius ? new Line(w1, p1).RightNormal : new Line(w1, p1).LeftNormal) * Radius;
					var s2 = p2 + (d2.Magnitude < Radius ? new Line(w2, p2).LeftNormal : new Line(w2, p2).RightNormal) * Radius;

					if ((w2 - Location).Magnitude > Radius && (w1 - Location).Magnitude > Radius)
					{
						Draw.FillTriangle(obstacle.Filter, w1, w2, s1);
					}
					else if ((w2 - Location).Magnitude > Radius)
					{
						Draw.FillTriangle(obstacle.Filter, w1, w2, p1);
						Draw.FillTriangle(obstacle.Filter, p1, w2, s1);
						Draw.FillTriangle(obstacle.Filter, w2, s1, s2);
					}
					else if ((w1 - Location).Magnitude > Radius)
					{
						Draw.FillTriangle(obstacle.Filter, w1, w2, p2);
						Draw.FillTriangle(obstacle.Filter, p2, w1, s2);
						Draw.FillTriangle(obstacle.Filter, w1, s1, s2);
					}
					else
					{
						Draw.FillTriangle(obstacle.Filter, w1, w2, p1);
						Draw.FillTriangle(obstacle.Filter, w2, p1, p2);
						Draw.FillTriangle(obstacle.Filter, p1, p2, s1);
						Draw.FillTriangle(obstacle.Filter, p2, s1, s2);
					}
					/*
					using (new BlendModeContext(BlendMode.AlphaBlend))
					{
						Draw.Triangle(Color.Red, Color.DarkRed, Color.Green, w1, w2, p1);
						Draw.Triangle(Color.DarkRed, Color.Green, Color.DarkGreen, w2, p1, p2);
						Draw.Triangle(Color.Green, Color.DarkGreen, Color.Violet, p1, p2, s1);
						Draw.Triangle(Color.DarkGreen, Color.Violet, Color.DarkViolet, p2, s1, s2);


						Draw.Line(Color.Aqua, w1, w2);

						Draw.FillCircle(Color.Red, w1, 4);
						Draw.FillCircle(Color.DarkRed, w2, 4);

						Draw.FillCircle(Color.Green, p1, 4);
						Draw.FillCircle(Color.DarkGreen, p2, 4);

						Draw.FillCircle(Color.Violet, s1, 4);
						Draw.FillCircle(Color.DarkViolet, s2, 4);
					}

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
			}


	//		_renderSystem.SetVertices(UsageHint.StreamDraw, vertices);
	//		_renderSystem.SetColors(UsageHint.StreamDraw, colors);

	//		using (new BlendModeContext(BlendMode.Subtractive))
	//			_renderSystem.Render(PrimitiveType.Triangles, vertices.Count());
		}
	}
}