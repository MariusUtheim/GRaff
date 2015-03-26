using System;
using GRaff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameMaker.UnitTesting
{
	[TestClass]
	public class RectangleTest
	{
		[TestMethod]
		public void IntRectangleIntersection()
		{
			IntRectangle rec1, rec2;

			rec1 = new IntRectangle(0, 0, 100, 100);
			rec2 = new IntRectangle(60, 60, 100, 100);
			Assert.AreEqual(new IntRectangle(60, 60, 40, 40), rec1.Intersection(rec2));
			Assert.AreEqual(new IntRectangle(60, 60, 40, 40), rec2.Intersection(rec1));

			rec1 = new IntRectangle(0, 0, 100, 100);
			rec2 = new IntRectangle(25, 25, 50, 50);
			Assert.AreEqual(new IntRectangle(25, 25, 50, 50), rec1.Intersection(rec2));
			Assert.AreEqual(new IntRectangle(25, 25, 50, 50), rec2.Intersection(rec1));

			rec1 = new IntRectangle(0, 0, 40, 40);
			rec2 = new IntRectangle(50, 50, 40, 40);
			Assert.AreEqual(null, rec1.Intersection(rec2));
		}
	}
}
