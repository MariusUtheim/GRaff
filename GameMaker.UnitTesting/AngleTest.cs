using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class AngleTest
	{
		[TestMethod]
		public void BasicOperations()
		{
			Angle a = new Angle(90);
		
			Assert.AreEqual(90, a.Degrees);
			Assert.AreEqual(GMath.Tau / 4.0, a.Radians);
			Assert.AreEqual(a, Angle.Deg(90));
			Assert.AreEqual(a, Angle.Rad(GMath.Tau / 4.0));
			Assert.AreEqual(a, Angle.Deg(-270));
			Assert.AreEqual(a, Angle.Deg(450));
		}
		
		[TestMethod]
		public void AdvancedOperations()
		{
			Angle expected = new Angle(45);

			Assert.AreEqual(expected, Angle.Direction(2, 2));
			Assert.AreEqual(expected, Angle.Direction(new Point(5, 5)));
			Assert.AreEqual(expected, Angle.Direction(10, 0, 12, 2));
			Assert.AreEqual(expected, Angle.Direction(new Point(104, 204), new Point(108, 208)));
			Assert.AreEqual(Angle.Zero, Angle.Direction(0, 0));

			Angle a0 = Angle.Deg(83);
			Assert.AreEqual(Angle.Deg(45), Angle.Acute(a0, a0 + Angle.Deg(45)));
			Assert.AreEqual(Angle.Deg(90), Angle.Acute(a0, a0 + Angle.Deg(270)));
		}

		[TestMethod]
		public void Operators()
		{
			Assert.AreEqual("45°", Angle.Deg(45).ToString());

			Assert.IsTrue(Angle.Deg(180) == Angle.Rad(GMath.Tau / 2));
			Assert.IsTrue(Angle.Deg(180) != Angle.Rad(180.00001));
			Assert.AreEqual(Angle.Deg(100), Angle.Deg(40) + Angle.Deg(60));
			Assert.AreEqual(Angle.Deg(-40), Angle.Deg(60) - Angle.Deg(100));
			Assert.AreEqual(Angle.Deg(90) * 4.0, Angle.Deg(360));
			Assert.AreEqual(-Angle.Deg(90), Angle.Deg(270));
        }
	}
}
