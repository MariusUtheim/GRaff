using System;
using GRaff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class RectangleTest
	{
        Random rand = new Random();

        private static bool _rectEquals(Rectangle rect1, Rectangle rect2)
            => new Rectangle((Point)(rect1.TopLeft - rect2.TopLeft), rect1.Size - rect2.Size).Area <= GMath.MachineEpsilon;

        [TestMethod]
        public void Rectangle_Abs()
        {
            var (w, h) = (rand.Double(), rand.Double());
            var negRect = new Rectangle(0, 0, -w, -h);
            Assert.AreEqual(new Rectangle(-w, -h, w, h), negRect.Abs);
        }

        [TestMethod]
        public void Rectangle_ContainsPoint()
        {
            var (w, h) = (rand.Double(), rand.Double());
            var rect = new Rectangle(0, 0, w, h);

            Assert.IsTrue(rect.Contains((w / 2, h / 2)));
            Assert.IsTrue(rect.Contains((0, 0)));
            Assert.IsFalse(rect.Contains((w, 0)));
            Assert.IsFalse(rect.Contains((0, h)));
            Assert.IsFalse(rect.Contains((w, h)));

            var negRect = new Rectangle(0, 0, -w, -h);
            Assert.IsTrue(negRect.Contains((-w / 2, -h / 2)));
            Assert.IsTrue(negRect.Contains(Point.Zero));
            Assert.IsFalse(rect.Contains((-w, 0)));
            Assert.IsFalse(rect.Contains((0, -h)));
            Assert.IsFalse(negRect.Contains((-w, -h)));

            var (x, y) = (rand.Double(), rand.Double());
            Assert.IsTrue(new Rectangle(x, y, w, h).Contains((x, y)));
            Assert.IsFalse(new Rectangle(x, y, w, h).Contains((x + w, y + h)));
        }

        [TestMethod]
        public void Rectangle_Intersections()
        {
            var unitRect = new Rectangle(0, 0, 1, 1);

            var offsetRect = unitRect + new Vector(rand.Double(), rand.Double());
            Assert.IsTrue(unitRect.Intersects(offsetRect));

            var intersection = unitRect.Intersection(offsetRect).Value;
            Assert.AreEqual(new Rectangle(offsetRect.TopLeft, new Vector(1 - offsetRect.Left, 1 - offsetRect.Top)), intersection);
            
            var nonIntersection = new Rectangle(1 + rand.Double(), 1 + rand.Double(), 1, 1);
            Assert.IsFalse(unitRect.Intersects(nonIntersection));
            Assert.IsFalse(nonIntersection.Intersects(unitRect));
            Assert.IsNull(unitRect.Intersection(nonIntersection));
            Assert.IsNull(nonIntersection.Intersection(unitRect));
        }

        [TestMethod]
        public void Rectangle_BoundaryIntersections()
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
        public void Rectangle_NegativeIntersections()
        {
            var (dx, dy) = (rand.Double(), rand.Double());
            var unitRect = new Rectangle(0, 0, 1, 1);
            var negRect = new Rectangle(0, 0, -1, -1);

            Assert.AreEqual(new Rectangle(0, 0, 0, 0), unitRect.Intersection(negRect));
            Assert.IsNull(unitRect.Intersection(negRect + (2.0, 2.0)));

            Assert.IsTrue(_rectEquals(new Rectangle(0, 0, dx, dy), unitRect.Intersection(negRect + (dx, dy)).Value));
            Assert.IsTrue(_rectEquals(new Rectangle(dx, 0, 1 - dx, dy), unitRect.Intersection(negRect + (1 + dx, dy)).Value));
            Assert.IsTrue(_rectEquals(new Rectangle(0, dy, dx, 1 - dy), unitRect.Intersection(negRect + (dx, 1 + dy)).Value));
            Assert.IsTrue(_rectEquals(new Rectangle(dx, dy, 1 - dx, 1 - dy), unitRect.Intersection(negRect + (1 + dx, 1 + dy)).Value));


            Assert.AreEqual(new Rectangle(0, 0, 0, 0), negRect.Intersection(unitRect));
            Assert.IsNull(negRect.Intersection(unitRect - (2.0, 2.0)));

            Assert.IsTrue(_rectEquals(new Rectangle(dx, dy, -dx, -dy), (negRect + (dx, dy)).Intersection(unitRect).Value));
            Assert.IsTrue(_rectEquals(new Rectangle(dx, 0, 1 - dx, dy), unitRect.Intersection(negRect + (1 + dx, dy)).Value));
            Assert.IsTrue(_rectEquals(new Rectangle(0, dy, dx, 1 - dy), unitRect.Intersection(negRect + (dx, 1 + dy)).Value));
            Assert.IsTrue(_rectEquals(new Rectangle(dx, dy, 1 - dx, 1 - dy), unitRect.Intersection(negRect + (1 + dx, 1 + dy)).Value));
        }


    }
}
