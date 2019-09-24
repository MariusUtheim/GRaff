using System;
using Xunit;

namespace GRaff.UnitTesting
{
	public class TransformTest
	{
        private const double delta = 1e-14;

		[Fact]
		public void Transform_Translate()
		{
			Transform transform = new Transform();
			transform.X = 50;
			transform.Y = 60;

			Point pt = new Point(4, 8);
			Point expected = new Point(54, 68);
			Point actual = transform.Point(pt);
			Assert.Equal(expected, actual);
		}
		
		[Fact]
		public void Transform_Scale()
		{
			Transform transform = new Transform();
			transform.XScale = 3;
			transform.YScale = 2;

			Point pt, expected, actual;

			pt = new Point(10, 13);
			expected = new Point(30, 26);
			actual = transform.Point(pt);
			Assert.Equal(expected, actual);

			pt = new Point(-15, -11);
			expected = new Point(-45, -22);
			actual = transform.Point(pt);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void Transform_Rotate()
		{
			Transform transform = new Transform();
			transform.Rotation = Angle.Deg(45);

			Point pt, expected, actual;

			pt = new Point(1, 0);
			expected = new Point(1 / GMath.Sqrt(2), 1 / GMath.Sqrt(2));
			actual = transform.Point(pt);
			Assert.Equal(expected.X, actual.X, 9);
			Assert.Equal(expected.Y, actual.Y, 9);

			pt = new Point(1, 1);
			expected = new Point(0, GMath.Sqrt(2));
			actual = transform.Point(pt);
			Assert.Equal(expected.X, actual.X, 9);
			Assert.Equal(expected.Y, actual.Y, 9);
		}

		[Fact]
		public void Transform_Shear()
		{
			Transform transform = new Transform();

			Point pt = new Point(5, 10);
			Point expected, actual;

			transform.XShear = 2;
			transform.YShear = 0;
			expected = new Point(25, 10);
			actual = transform.Point(pt);
			Assert.Equal(expected, actual);

			transform.XShear = 0;
			transform.YShear = 1;
			expected = new Point(5, 15);
			actual = transform.Point(pt);
			Assert.Equal(expected, actual);

			transform.XShear = 2;
			transform.YShear = 1;
			expected = new Point(25, 15);
			actual = transform.Point(pt);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void Transform_Compund()
		{
            Transform transform = new Transform
            {
                XScale = 2.0,
                YScale = 3.0,
                Rotation = Angle.Deg(90),
                X = 11,
                Y = 19
            };

            Point pt = new Point(1 / 2.0, 1 / 3.0);
			Point expected = new Point(10, 20);
			Point actual = transform.Point(pt);
			Assert.Equal(expected, actual);
		}

    }
}
