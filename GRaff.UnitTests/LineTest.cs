using System;
using Xunit;

namespace GRaff.UnitTesting
{
    public class LineTest
    { 

        [Fact]
        public void Line_Intersections()
        {
            var line = new Line(0, 0, 1, 0);

            // Test parallel lines
            Assert.False(line.Intersects(new Line(0, -1, 1, -1)));
            Assert.True(line.Intersects(new Line(0, 0, 1, 0)));
            Assert.False(line.Intersects(new Line(0, 1, 1, 1)));

            // Test perpendicular lines
            Assert.False(line.Intersects(new Line(-0.5, -1, -0.5, 1)));
            Assert.True(line.Intersects(new Line(0.5, -1, 0.5, 1)));
            Assert.False(line.Intersects(new Line(1.5, -1, 1.5, 1)));

            Assert.False(line.Intersects(new Line(1, 1, 1, 2)));
            Assert.False(line.Intersects(new Line(0, 1, 0, 2)));
            Assert.False(line.Intersects(new Line(1, -2, 1, -1)));
            Assert.False(line.Intersects(new Line(0, -2, 0, -1)));


            // Test collinear lines
            var otherLine = new Line(-2, 0, -1, 0);
            Assert.False(line.Intersects(otherLine));
            Assert.True(line.Intersects(otherLine + new Vector(2, 0)));
            Assert.False(line.Intersects(otherLine + new Vector(4, 0)));

            // Test other lines
            Assert.True(line.Intersects(new Line(0, -1, 1, 2)));
            Assert.True(line.Intersects(new Line(-1, -1, 2, 2)));
            Assert.False(line.Intersects(new Line(-1, -1, 2, 3)));
        }
    }
}
