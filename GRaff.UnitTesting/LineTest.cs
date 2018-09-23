using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
    [TestClass]
    public class LineTest
    { 

        [TestMethod]
        public void Line_Intersections()
        {
            var line = new Line(0, 0, 1, 0);

            // Test parallel lines
            Assert.IsFalse(line.Intersects(new Line(0, -1, 1, -1)));
            Assert.IsTrue(line.Intersects(new Line(0, 0, 1, 0)));
            Assert.IsFalse(line.Intersects(new Line(0, 1, 1, 1)));

            // Test perpendicular lines
            Assert.IsFalse(line.Intersects(new Line(-0.5, -1, -0.5, 1)));
            Assert.IsTrue(line.Intersects(new Line(0.5, -1, 0.5, 1)));
            Assert.IsFalse(line.Intersects(new Line(1.5, -1, 1.5, 1)));

            Assert.IsFalse(line.Intersects(new Line(1, 1, 1, 2)));
            Assert.IsFalse(line.Intersects(new Line(0, 1, 0, 2)));
            Assert.IsFalse(line.Intersects(new Line(1, -2, 1, -1)));
            Assert.IsFalse(line.Intersects(new Line(0, -2, 0, -1)));


            // Test collinear lines
            var otherLine = new Line(-2, 0, -1, 0);
            Assert.IsFalse(line.Intersects(otherLine));
            Assert.IsTrue(line.Intersects(otherLine + new Vector(2, 0)));
            Assert.IsFalse(line.Intersects(otherLine + new Vector(4, 0)));

            // Test other lines
            Assert.IsTrue(line.Intersects(new Line(0, -1, 1, 2)));
            Assert.IsTrue(line.Intersects(new Line(-1, -1, 2, 2)));
            Assert.IsFalse(line.Intersects(new Line(-1, -1, 2, 3)));
        }
    }
}
