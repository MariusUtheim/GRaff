using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
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

		[TestMethod]
		public void Polygon_Vertices()
		{
			Point[] actual = thePolygon.Vertices.ToArray();

			Assert.AreEqual(expected.Length, actual.Length);
			for (int i = 0; i < expected.Length; i++)
				Assert.AreEqual(expected[i], actual[i]);

			for (int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], thePolygon.Vertex(i));
				Assert.AreEqual(expected[i], thePolygon.Vertex(i + expected.Length));
				Assert.AreEqual(expected[i], thePolygon.Vertex(i - expected.Length));
			}
		}

		[TestMethod]
		public void Polygon_Edges()
		{
			int i = 0;
			foreach (Line actual in thePolygon.Edges)
			{
				int c0 = i % expected.Length, c1 = (i + 1) % expected.Length;
				Assert.AreEqual(new Line(expected[c0], expected[c1]), actual);
				Assert.AreEqual(new Line(expected[c0], expected[c1]), thePolygon.Edge(i));
				i++;
			} 
		}

		[TestMethod]
		public void Polygon_ContainsPoint()
		{
			double[,] actualMask = (double[,])expectedMask.Clone();

			for (int x = 0; x < 9; x++)
				for (int y = 0; y < 9; y++)
					actualMask[x, y] -= thePolygon.ContainsPoint(x, y) ? 1 : 0;

			StringBuilder str = new StringBuilder();

			for (int x = 0; x < 9; x++)
			{
				for (int y = 0; y < 9; y++)
					Assert.IsTrue(GMath.Abs(actualMask[x, y]) < 1, "Element ({0}, {1}) is equal to {2}", x, y, actualMask[x, y]);
			}
		}

		[TestMethod]
		public void Polygon_Intersects()
		{
			Polygon completelyInside = new Polygon(new Point(3, 3), new Point(5, 5), new Point(4, 7));
			Polygon pointInside = new Polygon(new Point(4, 4), new Point(10, 6), new Point(8, 12));
			Polygon edgeInside = new Polygon(new Point(1, 0), new Point(10, -1), new Point(8, 9));
			Polygon completelyEnclosing = new Polygon(new Point(0, 0), new Point(10, 0), new Point(10, 10), new Point(0, 10));

			Assert.IsTrue(thePolygon.Intersects(completelyInside), "Doesn't intersect internal polygon!");
			Assert.IsTrue(thePolygon.Intersects(pointInside), "Doesn't intersect internal vertex!");
			Assert.IsTrue(thePolygon.Intersects(edgeInside), "Doesn't intersect internal edge!");
			Assert.IsTrue(thePolygon.Intersects(completelyEnclosing), "Doesn't intersect enclosing polygon!");

			Polygon outside = new Polygon(new Point(-5, -5), new Point(2, -5), new Point(-2, 5));
			Assert.IsFalse(thePolygon.Intersects(outside), "Does intersect non-intersecting polynomial!");
		}

		[TestMethod]
		public void Polygon_Constructors()
		{
			new Polygon(new[] { new Point(0, 0), new Point(0, 1), new Point(1, 0) });
			new Polygon(new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) });
			new Polygon(new[] { new Point(0, 0) });
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Polygon_ZeroPointConstructorFailure()
		{
			new Polygon(new Point[0]);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
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

			new Polygon(invalidPoints);
		}
	}
}
