using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GRaff;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class MovingObjectTest
	{
		private class SimpleMovingObject : MovingObject
		{
			public SimpleMovingObject(double x, double y)
				: base(x, y) { }
		}

		[TestMethod]
		public void LinearMotion()
		{
			var testCase = (Action<Vector, int, Point>) delegate(Vector vel, int nsteps, Point end) {
				var instance = new SimpleMovingObject(0, 0);
				instance.Velocity = vel;
				for (int i = 0; i < nsteps; i++)
					instance.OnStep();
				Assert.AreEqual(0, (end - instance.Location).Magnitude, 1.0e-14 * nsteps, String.Format("Initial velocity: {0}", vel));
			};
			
			testCase(new Vector(1, 0), 1000, new Point(1000, 0));
			testCase(new Vector(Math.Sqrt(2), Angle.Deg(45)), 1000, new Point(1000, 1000));
			testCase(new Vector(4, 4), 1000, new Point(4000, 4000));
			testCase(new Vector(1.1, 0), 1000, new Point(1100, 0));
		}
	}
}
