using System;
using GRaff;
using Xunit;

namespace GRaff.UnitTesting
{
	public class RectangleTest
	{
        Random rand = new Random();

        private static bool _rectEquals(Rectangle rect1, Rectangle rect2)
            => new Rectangle((Point)(rect1.TopLeft - rect2.TopLeft), rect1.Size - rect2.Size).Area <= GMath.MachineEpsilon;

        [Fact]
        public void Rectangle_Abs()
        {
            var (w, h) = (rand.Double(), rand.Double());
            var negRect = new Rectangle(0, 0, -w, -h);
            Assert.Equal(new Rectangle(-w, -h, w, h), negRect.Abs);
        }

        [Fact]
        public void Rectangle_ContainsPoint()
        {
            var (w, h) = (rand.Double(), rand.Double());
            var rect = new Rectangle(0, 0, w, h);

            Assert.True(rect.Contains((w / 2, h / 2)));
            Assert.True(rect.Contains((0, 0)));
            Assert.False(rect.Contains((w, 0)));
            Assert.False(rect.Contains((0, h)));
            Assert.False(rect.Contains((w, h)));

            var negRect = new Rectangle(0, 0, -w, -h);
            Assert.True(negRect.Contains((-w / 2, -h / 2)));
            Assert.True(negRect.Contains(Point.Zero));
            Assert.False(rect.Contains((-w, 0)));
            Assert.False(rect.Contains((0, -h)));
            Assert.False(negRect.Contains((-w, -h)));

            var (x, y) = (rand.Double(), rand.Double());
            Assert.True(new Rectangle(x, y, w, h).Contains((x, y)));
            Assert.False(new Rectangle(x, y, w, h).Contains((x + w, y + h)));
        }

        [Fact]
        public void Rectangle_Intersections()
        {
            var unitRect = new Rectangle(0, 0, 1, 1);

            var offsetRect = unitRect + new Vector(rand.Double(), rand.Double());
            Assert.True(unitRect.Intersects(offsetRect));

            var intersection = unitRect.Intersection(offsetRect).Value;
            Assert.Equal(new Rectangle(offsetRect.TopLeft, new Vector(1 - offsetRect.Left, 1 - offsetRect.Top)), intersection);
            
            var nonIntersection = new Rectangle(1 + rand.Double(), 1 + rand.Double(), 1, 1);
            Assert.False(unitRect.Intersects(nonIntersection));
            Assert.False(nonIntersection.Intersects(unitRect));
            Assert.Null(unitRect.Intersection(nonIntersection));
            Assert.Null(nonIntersection.Intersection(unitRect));
        }

        [Fact]
        public void Rectangle_BoundaryIntersections()
        {
            var rect = new Rectangle(0, 0, rand.Double(), rand.Double());

            Assert.False(rect.Intersects(new Rectangle(rect.Width, 0, 1, 1)));
            Assert.False(rect.Intersects(new Rectangle(0, rect.Height, 1, 1)));

            Assert.False(rect.Contains(new Point(rect.Width, rand.Double(rect.Height))));
            Assert.False(rect.Contains(new Point(rand.Double(rect.Width), rect.Height)));
            Assert.True(rect.Contains(new Point(0, rand.Double(rect.Height))));
            Assert.True(rect.Contains(new Point(rand.Double(rect.Width), 0)));

        }

        [Fact]
        public void Rectangle_NegativeIntersections()
        {
            var (dx, dy) = (rand.Double(), rand.Double());
            var unitRect = new Rectangle(0, 0, 1, 1);
            var negRect = new Rectangle(0, 0, -1, -1);

            Assert.Equal(new Rectangle(0, 0, 0, 0), unitRect.Intersection(negRect));
            Assert.Null(unitRect.Intersection(negRect + (2.0, 2.0)));

            Assert.True(_rectEquals(new Rectangle(0, 0, dx, dy), unitRect.Intersection(negRect + (dx, dy)).Value));
            Assert.True(_rectEquals(new Rectangle(dx, 0, 1 - dx, dy), unitRect.Intersection(negRect + (1 + dx, dy)).Value));
            Assert.True(_rectEquals(new Rectangle(0, dy, dx, 1 - dy), unitRect.Intersection(negRect + (dx, 1 + dy)).Value));
            Assert.True(_rectEquals(new Rectangle(dx, dy, 1 - dx, 1 - dy), unitRect.Intersection(negRect + (1 + dx, 1 + dy)).Value));


            Assert.Equal(new Rectangle(0, 0, 0, 0), negRect.Intersection(unitRect));
            Assert.Null(negRect.Intersection(unitRect - (2.0, 2.0)));

            Assert.True(_rectEquals(new Rectangle(dx, dy, -dx, -dy), (negRect + (dx, dy)).Intersection(unitRect).Value));
            Assert.True(_rectEquals(new Rectangle(dx, 0, 1 - dx, dy), unitRect.Intersection(negRect + (1 + dx, dy)).Value));
            Assert.True(_rectEquals(new Rectangle(0, dy, dx, 1 - dy), unitRect.Intersection(negRect + (dx, 1 + dy)).Value));
            Assert.True(_rectEquals(new Rectangle(dx, dy, 1 - dx, 1 - dy), unitRect.Intersection(negRect + (1 + dx, 1 + dy)).Value));
        }


    }
}
