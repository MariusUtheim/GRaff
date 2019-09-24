using System;
using System.Linq;
using System.Text;
using Xunit;

namespace GRaff.UnitTesting
{
	public class PolygonTest
	{

		// An octagon with sidelength 3 (in superemum norm)
		private static Point[] expected = new Point[] {
			new Point(3, 1),
			new Point(5, 1),
			new Point(7, 3),
			new Point(7, 5),
			new Point(5, 7),
			new Point(3, 7),
			new Point(1, 5),
			new Point(1, 3)
		};
        private static Polygon thePolygon = new Polygon(expected);

		private static double[,] expectedMask = new double[,] {
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 0, 0, .5, .5, .5, 0, 0, 0 },
			{ 0, 0, .5, 1, 1, 1, .5, 0, 0 },
			{ 0, .5, 1, 1, 1, 1, 1, .5, 0 },
			{ 0, .5, 1, 1, 1, 1, 1, .5, 0 },
			{ 0, .5, 1, 1, 1, 1, 1, .5, 0 },
			{ 0, 0, .5, 1, 1, 1, .5, 0, 0 },
			{ 0, 0, 0, .5, .5, .5, 0, 0, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0 },
		};

		[Fact]
		public void Polygon_Vertices()
		{
			Point[] actual = thePolygon.Vertices.ToArray();

			Assert.Equal(expected.Length, actual.Length);
			for (int i = 0; i < expected.Length; i++)
				Assert.Equal(expected[i], actual[i]);

			for (int i = 0; i < expected.Length; i++)
			{
				Assert.Equal(expected[i], thePolygon.Vertex(i));
				Assert.Equal(expected[i], thePolygon.Vertex(i + expected.Length));
				Assert.Equal(expected[i], thePolygon.Vertex(i - expected.Length));
			}
		}

		[Fact]
		public void Polygon_Edges()
		{
			int i = 0;
			foreach (Line actual in thePolygon.Edges)
			{
				int c0 = i % expected.Length, c1 = (i + 1) % expected.Length;
				Assert.Equal(new Line(expected[c0], expected[c1]), actual);
				Assert.Equal(new Line(expected[c0], expected[c1]), thePolygon.Edge(i));
				i++;
			} 
		}

		[Fact]
		public void Polygon_ContainsPoint()
		{
			double[,] actualMask = (double[,])expectedMask.Clone();

			for (int x = 0; x < 9; x++)
				for (int y = 0; y < 9; y++)
					actualMask[x, y] -= thePolygon.ContainsPoint(x, y) ? 1 : 0;

			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
                    Assert.True(GMath.Abs(actualMask[x, y]) < 1, $"Element ({x}, {y}) is equal to {actualMask[x, y]}");
			}

            var unitSquare = new Polygon(new Point[] { (0, 0), (1, 0), (1, 1), (0, 1) });
            Assert.True(unitSquare.ContainsPoint((GRandom.Double(), GRandom.Double())));
        }

        [Fact]
        public void Polygon_DegenerateBehavior()
        {
            var unitSquare = new Polygon(new Point[] { (0, 0), (1, 0), (1, 1), (0, 1) });

            Assert.False(unitSquare.Intersects(new Polygon(new Point[] { (0, -0.1), (1, -0.1) })));
            Assert.True(unitSquare.Intersects(new Polygon(new Point[] { (0, 0), (1, 0) })));
            Assert.True(unitSquare.Intersects(new Polygon(new Point[] { (0, 0.5), (1, 0.5) })));
            Assert.True(unitSquare.Intersects(new Polygon(new Point[] { (0, 0), (1, 1) })));
            Assert.True(unitSquare.Intersects(new Polygon(new Point[] { (0, 1), (1, 1) })));
            Assert.False(unitSquare.Intersects(new Polygon(new Point[] { (0, 1.1), (1, 1.1) })));

            Assert.False(unitSquare.Intersects(new Polygon(new Point[] { (-0.1, 0), (-0.1, 1) })));
            Assert.True(unitSquare.Intersects(new Polygon(new Point[] { (0, 0), (0, 1) })));
            Assert.True(unitSquare.Intersects(new Polygon(new Point[] { (0.5, 0), (0.5, 1) })));
            Assert.True(unitSquare.Intersects(new Polygon(new Point[] { (1, 0), (0, 1) })));
            Assert.True(unitSquare.Intersects(new Polygon(new Point[] { (1, 0), (1, 1) })));
            Assert.False(unitSquare.Intersects(new Polygon(new Point[] { (1.1, 0), (1.1, 1) })));

            Assert.False(unitSquare.Intersects(new Polygon(new Point[] { (-0.5, 0.5) })));
            Assert.False(unitSquare.Intersects(new Polygon(new Point[] { (0.5, -0.5) })));
            Assert.False(unitSquare.Intersects(new Polygon(new Point[] { (1.5, 0.5) })));
            Assert.False(unitSquare.Intersects(new Polygon(new Point[] { (0.5, 1.5) })));
            Assert.True(unitSquare.Intersects(new Polygon(new Point[] { (0.5, 0.5) })));

            Assert.False(unitSquare.Intersects(new Polygon(new Point[0])));

            var line = new Polygon(new Point[] { (0, 0), (1, 0) });
            Assert.True(line.Intersects(new Polygon(new Point[] { (0.5, -0.5), (0.5, 0.5) })));
            Assert.False(line.Intersects(new Polygon(new Point[] { (0.5, 0) })));
            Assert.False(line.ContainsPoint(0.5, 0));
        }

        [Fact]
		public void Polygon_Intersects()
		{
			Polygon completelyInside = new Polygon(new[] { new Point(3, 3), new Point(5, 5), new Point(4, 7) });
			Polygon pointInside = new Polygon(new[] { new Point(4, 4), new Point(10, 6), new Point(8, 12) });
			Polygon edgeInside = new Polygon(new[] { new Point(1, 0), new Point(10, -1), new Point(8, 9) });
			Polygon completelyEnclosing = new Polygon(new[] { new Point(0, 0), new Point(10, 0), new Point(10, 10), new Point(0, 10) });

			Assert.True(thePolygon.Intersects(completelyInside), "Doesn't intersect internal polygon!");
			Assert.True(thePolygon.Intersects(pointInside), "Doesn't intersect internal vertex!");
			Assert.True(thePolygon.Intersects(edgeInside), "Doesn't intersect internal edge!");
			Assert.True(thePolygon.Intersects(completelyEnclosing), "Doesn't intersect enclosing polygon!");

			Polygon outside = new Polygon(new[] { new Point(-5, -5), new Point(2, -5), new Point(-2, 5) });
			Assert.False(thePolygon.Intersects(outside), "Does intersect non-intersecting polynomial!");
		}

		[Fact]
		public void Polygon_Constructors()
		{
			new Polygon(new[] { new Point(0, 0), new Point(0, 1), new Point(1, 0) });
			new Polygon(new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) });
			new Polygon(new[] { new Point(0, 0) });
		}

        [Fact]
        public void Polygon_SharedBordersDoNotIntersect()
        {
            var p1 = new Polygon(new Point[] { (0, 0), (1, 0), (1, 1), (0, 1) });
            var p2 = new Polygon(new Point[] { (1, 0), (2, 0), (2, 1), (1, 1) });

            Assert.False(p1.Intersects(p2));
        }

		[Fact]
		public void Polygon_SanityFailureCheck()
		{
			var invalidPoints = new[]
			{
				new Point(0, 0),
				new Point(1, 0),
				new Point(1, 1),
				new Point(0, 1),
				new Point(0, 0),
				new Point(1, 0),
				new Point(1, 1),
				new Point(0, 1)
			};

			Assert.Throws<ArgumentException>(() => new Polygon(invalidPoints));
		}

		[Fact]
		public void Polygon_FlippedPolygonConservesWinding()
		{
			var originalPolygon = Polygon.Rectangle(2, 1);
			var intersectingPolygon = Polygon.Rectangle(1, 2);

			Assert.True(originalPolygon.ContainsPoint(Point.Zero));
			Assert.True(originalPolygon.Intersects(intersectingPolygon));

			var flippedPolygon = new Transform { XScale = -1 }.Polygon(originalPolygon);

			Assert.True(originalPolygon.ContainsPoint(Point.Zero));
			Assert.True(originalPolygon.Intersects(intersectingPolygon));
		}

	}
}
