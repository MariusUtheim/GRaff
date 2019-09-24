using System;
using Xunit;

namespace GRaff.UnitTesting
{
    public class IntRectangleTest
    {
        Random rand = new Random();

        [Fact]
        public void IntRectangle_Abs()
        {
            var (w, h) = (rand.Integer(), rand.Integer());
            var negRect = new IntRectangle(0, 0, -w, -h);
            Assert.Equal(new IntRectangle(-w, -h, w, h), negRect.Abs);
        }

        [Fact]
        public void IntRectangle_ContainsPoint()
        {
            var (w, h) = (rand.Integer(100), rand.Integer(100));
            var rect = new IntRectangle(0, 0, w, h);

            Assert.True(rect.Contains((w / 2, h / 2)));
            Assert.True(rect.Contains((0, 0)));
            Assert.False(rect.Contains((w, 0)));
            Assert.False(rect.Contains((0, h)));
            Assert.False(rect.Contains((w, h)));

            var negRect = new IntRectangle(0, 0, -w, -h);
            Assert.True(negRect.Contains((-w / 2, -h / 2)));
            Assert.True(negRect.Contains(IntVector.Zero));
            Assert.False(rect.Contains((-w, 0)));
            Assert.False(rect.Contains((0, -h)));
            Assert.False(negRect.Contains((-w, -h)));

            var (x, y) = (rand.Integer(100), rand.Integer(100));
            Assert.True(new IntRectangle(x, y, w, h).Contains((x, y)));
            Assert.False(new IntRectangle(x, y, w, h).Contains((x + w, y + h)));
        }

        [Fact]
        public void IntRectangle_Intersections()
        {
            var rect = new IntRectangle(0, 0, rand.Integer(2, 100), rand.Integer(2, 100));

            var offsetRect = rect + new IntVector(rand.Integer(rect.Width), rand.Integer(rect.Height));
            Assert.True(rect.Intersects(offsetRect));
            Assert.True(offsetRect.Intersects(rect));

            var intersection = rect.Intersection(offsetRect);
            Assert.Equal(new IntRectangle(offsetRect.Left, offsetRect.Top, rect.Width - offsetRect.Left, rect.Height - offsetRect.Top), intersection);

            var nonIntersection = new IntRectangle(rect.Right + rand.Integer(1, 10), rect.Bottom + rand.Integer(1, 10), rand.Integer(100), rand.Integer(100));
            Assert.False(rect.Intersects(nonIntersection));
            Assert.False(nonIntersection.Intersects(rect));
            Assert.Null(rect.Intersection(nonIntersection));
            Assert.Null(nonIntersection.Intersection(rect));
        }

        [Fact]
        public void IntRectangle_BoundaryIntersections()
        {
            var rect = new IntRectangle(0, 0, rand.Integer(2, 100), rand.Integer(2, 100));

            Assert.False(rect.Intersects(new IntRectangle(rect.Width, 0, 1, 1)));
            Assert.False(rect.Intersects(new IntRectangle(0, rect.Height, 1, 1)));

            Assert.False(rect.Contains(new IntVector(rect.Width, rand.Integer(rect.Height))));
            Assert.False(rect.Contains(new IntVector(rand.Integer(rect.Width), rect.Height)));
            Assert.True(rect.Contains(new IntVector(0, rand.Integer(rect.Height))));
            Assert.True(rect.Contains(new IntVector(rand.Integer(rect.Width), 0)));
        }

        [Fact]
        public void IntRectangle_NegativeIntersections()
        {
            var (dx, dy) = (rand.Integer(10), rand.Integer(10));
            var rect = new IntRectangle(0, 0, 10, 10);
            var negRect = new IntRectangle(0, 0, -10, -10);

            Assert.Equal(new IntRectangle(0, 0, 0, 0), rect.Intersection(negRect));
            Assert.Null(rect.Intersection(negRect + (20, 20)));

            Assert.Equal(new IntRectangle(0, 0, dx, dy), rect.Intersection(negRect + (dx, dy)));
            Assert.Equal(new IntRectangle(dx, 0, 10 - dx, dy), rect.Intersection(negRect + (10 + dx, dy)));
            Assert.Equal(new IntRectangle(0, dy, dx, 10 - dy), rect.Intersection(negRect + (dx, 10 + dy)));
            Assert.Equal(new IntRectangle(dx, dy, 10 - dx, 10 - dy), rect.Intersection(negRect + (10 + dx, 10 + dy)));


            Assert.Equal(new IntRectangle(0, 0, 0, 0), negRect.Intersection(rect));
            Assert.Null(negRect.Intersection(rect - (20, 20)));

            Assert.Equal(new IntRectangle(dx, dy, -dx, -dy), (negRect + (dx, dy)).Intersection(rect));
            Assert.Equal(new IntRectangle(dx, 0, 10 - dx, dy), rect.Intersection(negRect + (10 + dx, dy)));
            Assert.Equal(new IntRectangle(0, dy, dx, 10 - dy), rect.Intersection(negRect + (dx, 10 + dy)));
            Assert.Equal(new IntRectangle(dx, dy, 10 - dx, 10 - dy), rect.Intersection(negRect + (10 + dx, 10 + dy)));
        }

    }
}
