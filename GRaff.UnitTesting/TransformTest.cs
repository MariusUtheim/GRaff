using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class TransformTest
	{

		[TestMethod]
		public void Transform_Translate()
		{
			Transform transform = new Transform();
			transform.X = 50;
			transform.Y = 60;

			Point pt = new Point(4, 8);
			Point expected = new Point(54, 68);
			Point actual = transform.Point(pt);
			Assert.AreEqual<Point>(expected, actual);
		}

		[TestMethod]
		public void Transform_Scale()
		{
			Transform transform = new Transform();
			transform.XScale = 3;
			transform.YScale = 2;

			Point pt, expected, actual;

			pt = new Point(10, 13);
			expected = new Point(30, 26);
			actual = transform.Point(pt);
			Assert.AreEqual<Point>(expected, actual);

			pt = new Point(-15, -11);
			expected = new Point(-45, -22);
			actual = transform.Point(pt);
			Assert.AreEqual<Point>(expected, actual);
		}

		[TestMethod]
		public void Transform_Rotate()
		{
			Transform transform = new Transform();
			transform.Rotation = Angle.Deg(45);

			Point pt, expected, actual;

			pt = new Point(1, 0);
			expected = new Point(1 / GMath.Sqrt(2), 1 / GMath.Sqrt(2));
			actual = transform.Point(pt);
			Assert.AreEqual(expected.X, actual.X, 1e-9);
			Assert.AreEqual(expected.Y, actual.Y, 1e-9);

			pt = new Point(1, 1);
			expected = new Point(0, GMath.Sqrt(2));
			actual = transform.Point(pt);
			Assert.AreEqual(expected.X, actual.X, 1e-9);
			Assert.AreEqual(expected.Y, actual.Y, 1e-9);

			transform.Rotation = Angle.Deg(-45 - 30);
			pt = new Point(-1, -1);
			expected = new Point(0.5, GMath.Sqrt(3) / 2);
			actual = transform.Point(pt);
		}

		[TestMethod]
		public void Transform_Shear()
		{
			Transform transform = new Transform();

			Point pt = new Point(5, 10);
			Point expected, actual;

			transform.XShear = 2;
			transform.YShear = 0;
			expected = new Point(25, 10);
			actual = transform.Point(pt);
			Assert.AreEqual<Point>(expected, actual);

			transform.XShear = 0;
			transform.YShear = 1;
			expected = new Point(5, 15);
			actual = transform.Point(pt);
			Assert.AreEqual<Point>(expected, actual);

			transform.XShear = 2;
			transform.YShear = 1;
			expected = new Point(25, 15);
			actual = transform.Point(pt);
			Assert.AreEqual<Point>(expected, actual);
		}

		[TestMethod]
		public void Transform_Compund()
		{
			Transform transform = new Transform();
			transform.XScale = 2.0;
			transform.YScale = 3.0;
			transform.Rotation = Angle.Deg(90);
			transform.X = 11;
			transform.Y = 19;

			Point pt = new Point(1 / 2.0, 1 / 3.0);
			Point expected = new Point(10, 20);
			Point actual = transform.Point(pt);
			Assert.AreEqual<Point>(expected, actual);
		}
	}
}
