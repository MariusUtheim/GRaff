using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class AngleTest
	{
		[TestMethod]
		public void Angle_RadiansAndDegrees()
		{
			for (int i = -360; i < 360; i++)
				Assert.IsTrue(Angle.Deg(i) == Angle.Rad(i * GMath.DegToRad), "Failed at " + i.ToString() + " degrees.");
		}
		
		[TestMethod]
		public void Angle_Direction()
		{
			double delta = Angle.Epsilon.Degrees;

			Angle expected = Angle.Deg(45);
			Assert.AreEqual(expected, Angle.Direction(2, 2));
			Assert.AreEqual(expected, Angle.Direction(new Point(5, 5)));
			Assert.AreEqual(expected, Angle.Direction(10, 0, 12, 2));
			Assert.AreEqual(expected, Angle.Direction(new Point(104, 204), new Point(108, 208)));
			Assert.AreEqual(Angle.Zero, Angle.Direction(0, 0));
		}

		[TestMethod]
		public void Angle_Acute()
		{
			Angle a0 = Angle.Deg(83);
			Assert.AreEqual(Angle.Deg(45), Angle.Acute(a0, a0 + Angle.Deg(45)));
			Assert.AreEqual(Angle.Deg(90), Angle.Acute(a0, a0 + Angle.Deg(270)));

			Angle _0 = Angle.Deg(0), _180 = Angle.Deg(180);
			Assert.AreEqual(Angle.Deg(180), Angle.Acute(_0, _180));
			Assert.AreEqual(Angle.Deg(180), Angle.Acute(_180, _0));
		}

		[TestMethod]
		public void Angle_Operators()
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
