using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
    [TestClass]
    public class IntRectangleTest
    {
        Random rand = new Random();

        [TestMethod]
        public void IntRectangle_Abs()
        {
            var (w, h) = (rand.Integer(), rand.Integer());
            var negRect = new IntRectangle(0, 0, -w, -h);
            Assert.AreEqual(new IntRectangle(-w, -h, w, h), negRect.Abs);
        }

        [TestMethod]
        public void IntRectangle_ContainsPoint()
        {
            var (w, h) = (rand.Integer(100), rand.Integer(100));
            var rect = new IntRectangle(0, 0, w, h);

            Assert.IsTrue(rect.Contains((w / 2, h / 2)));
            Assert.IsTrue(rect.Contains((0, 0)));
            Assert.IsFalse(rect.Contains((w, 0)));
            Assert.IsFalse(rect.Contains((0, h)));
            Assert.IsFalse(rect.Contains((w, h)));

            var negRect = new IntRectangle(0, 0, -w, -h);
            Assert.IsTrue(negRect.Contains((-w / 2, -h / 2)));
            Assert.IsTrue(negRect.Contains(IntVector.Zero));
            Assert.IsFalse(rect.Contains((-w, 0)));
            Assert.IsFalse(rect.Contains((0, -h)));
            Assert.IsFalse(negRect.Contains((-w, -h)));

            var (x, y) = (rand.Integer(100), rand.Integer(100));
            Assert.IsTrue(new IntRectangle(x, y, w, h).Contains((x, y)));
            Assert.IsFalse(new IntRectangle(x, y, w, h).Contains((x + w, y + h)));
        }

        [TestMethod]
        public void IntRectangle_Intersections()
        {
            var rect = new IntRectangle(0, 0, rand.Integer(2, 100), rand.Integer(2, 100));

            var offsetRect = rect + new IntVector(rand.Integer(rect.Width), rand.Integer(rect.Height));
            Assert.IsTrue(rect.Intersects(offsetRect));
            Assert.IsTrue(offsetRect.Intersects(rect));

            var intersection = rect.Intersection(offsetRect);
            Assert.AreEqual(new IntRectangle(offsetRect.Left, offsetRect.Top, rect.Width - offsetRect.Left, rect.Height - offsetRect.Top), intersection);

            var nonIntersection = new IntRectangle(rect.Right + rand.Integer(1, 10), rect.Bottom + rand.Integer(1, 10), rand.Integer(100), rand.Integer(100));
            Assert.IsFalse(rect.Intersects(nonIntersection));
            Assert.IsFalse(nonIntersection.Intersects(rect));
            Assert.IsNull(rect.Intersection(nonIntersection));
            Assert.IsNull(nonIntersection.Intersection(rect));
        }

        [TestMethod]
        public void IntRectangle_BoundaryIntersections()
        {
            var rect = new IntRectangle(0, 0, rand.Integer(2, 100), rand.Integer(2, 100));

            Assert.IsFalse(rect.Intersects(new IntRectangle(rect.Width, 0, 1, 1)));
            Assert.IsFalse(rect.Intersects(new IntRectangle(0, rect.Height, 1, 1)));

            Assert.IsFalse(rect.Contains(new IntVector(rect.Width, rand.Integer(rect.Height))));
            Assert.IsFalse(rect.Contains(new IntVector(rand.Integer(rect.Width), rect.Height)));
            Assert.IsTrue(rect.Contains(new IntVector(0, rand.Integer(rect.Height))));
            Assert.IsTrue(rect.Contains(new IntVector(rand.Integer(rect.Width), 0)));
        }

        [TestMethod]
        public void IntRectangle_NegativeIntersections()
        {
            var (dx, dy) = (rand.Integer(10), rand.Integer(10));
            var rect = new IntRectangle(0, 0, 10, 10);
            var negRect = new IntRectangle(0, 0, -10, -10);

            Assert.AreEqual(new IntRectangle(0, 0, 0, 0), rect.Intersection(negRect));
            Assert.IsNull(rect.Intersection(negRect + (20, 20)));

            Assert.AreEqual(new IntRectangle(0, 0, dx, dy), rect.Intersection(negRect + (dx, dy)));
            Assert.AreEqual(new IntRectangle(dx, 0, 10 - dx, dy), rect.Intersection(negRect + (10 + dx, dy)));
            Assert.AreEqual(new IntRectangle(0, dy, dx, 10 - dy), rect.Intersection(negRect + (dx, 10 + dy)));
            Assert.AreEqual(new IntRectangle(dx, dy, 10 - dx, 10 - dy), rect.Intersection(negRect + (10 + dx, 10 + dy)));


            Assert.AreEqual(new IntRectangle(0, 0, 0, 0), negRect.Intersection(rect));
            Assert.IsNull(negRect.Intersection(rect - (20, 20)));

            Assert.AreEqual(new IntRectangle(dx, dy, -dx, -dy), (negRect + (dx, dy)).Intersection(rect));
            Assert.AreEqual(new IntRectangle(dx, 0, 10 - dx, dy), rect.Intersection(negRect + (10 + dx, dy)));
            Assert.AreEqual(new IntRectangle(0, dy, dx, 10 - dy), rect.Intersection(negRect + (dx, 10 + dy)));
            Assert.AreEqual(new IntRectangle(dx, dy, 10 - dx, 10 - dy), rect.Intersection(negRect + (10 + dx, 10 + dy)));
        }

    }
}
