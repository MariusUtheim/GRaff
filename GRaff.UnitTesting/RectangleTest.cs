using System;
using GRaff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class RectangleTest
	{
        Random rand = new Random();

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

        [TestMethod]
        public void RectangleIntersections()
        {
            var unitRect = new Rectangle(0, 0, 1, 1);

            var offsetRect = unitRect + new Vector(rand.Double(), rand.Double());
            Assert.IsTrue(unitRect.Intersects(offsetRect));

            var intersection = unitRect.Intersection(offsetRect).Value;
            Assert.AreEqual(new Rectangle(offsetRect.TopLeft, new Vector(1 - offsetRect.Left, 1 - offsetRect.Top)), intersection);
            
            var nonIntersection = new Rectangle(1 + rand.Double(), 1 + rand.Double(), 1, 1);
            Assert.IsFalse(unitRect.Intersects(nonIntersection));
            Assert.IsNull(unitRect.Intersection(nonIntersection));
        }

        [TestMethod]
        public void RectangleBoundaryIntersections()
        {
            var rect = new Rectangle(0, 0, rand.Double(), rand.Double());

            Assert.IsFalse(rect.Intersects(new Rectangle(rect.Width, 0, 1, 1)));
            Assert.IsFalse(rect.Intersects(new Rectangle(0, rect.Height, 1, 1)));

            Assert.IsFalse(rect.Contains(new Point(rect.Width, rand.Double(rect.Height))));
            Assert.IsFalse(rect.Contains(new Point(rand.Double(rect.Width), rect.Height)));
            Assert.IsTrue(rect.Contains(new Point(0, rand.Double(rect.Height))));
            Assert.IsTrue(rect.Contains(new Point(rand.Double(rect.Width), 0)));

        }

        [TestMethod]
        public void RectangleNegativeIntersections()
        {

        }
	}
}
