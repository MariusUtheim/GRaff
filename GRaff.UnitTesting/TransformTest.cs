using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class TransformTest
	{
        private const double delta = 1e-14;

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

        [TestMethod]
        public void Transform_Matrix_Gives_Correct_Transform()
        {
            double sx = GRandom.Double(0, 1), sy = GRandom.Double(0, 1), shear = GRandom.Double(-1, 1),
                   tx = GRandom.Double(-100, 100), ty = GRandom.Double(-100, 100);
            Angle rot = GRandom.Angle();

            var matrix = Matrix.Translation(tx, ty) * Matrix.Rotation(rot) * Matrix.Shearing(shear, 0) * Matrix.Scaling(sx, sy);
            var transform = new Transform(matrix);

            Assert.AreEqual(sx, transform.XScale, delta);
            Assert.AreEqual(sy, transform.YScale, delta);
            Assert.AreEqual(shear, transform.XShear, delta);
            Assert.AreEqual(0, transform.YShear, delta);
            Assert.AreEqual(tx, transform.X, delta);
            Assert.AreEqual(ty, transform.Y, delta);
            Assert.AreEqual(rot.Radians, transform.Rotation.Radians, delta);
        }

        private static void _checkTransformsCorrectly(Matrix matrix)
        {
            var transform = new Transform(matrix);
            Assert.AreEqual(0, (matrix - transform.GetMatrix()).Determinant, delta);
        }

        [TestMethod]
        public void Transform_From_Random_Matrix()
        {
            double a = GRandom.Double(-1, 1), b = GRandom.Double(-1, 1), c = GRandom.Double(-1, 1), d = GRandom.Double(-1, 1);
            var matrix = new Matrix(a, b, 0, c, d, 0);
            _checkTransformsCorrectly(matrix);
        }

        [TestMethod]
        public void Transform_Degenerate_Matrices()
        {
            double a = GRandom.Double(-1, 1), b = GRandom.Double(-1, 1), c = GRandom.Double(-1, 1), d = GRandom.Double(-1, 1);

            _checkTransformsCorrectly(new Matrix(a, b, c, 0));
            _checkTransformsCorrectly(new Matrix(a, b, 0, d));
            _checkTransformsCorrectly(new Matrix(a, 0, c, d));
            _checkTransformsCorrectly(new Matrix(0, b, c, d));

            _checkTransformsCorrectly(new Matrix(a, b, 0, 0));
            _checkTransformsCorrectly(new Matrix(a, 0, c, 0));
            _checkTransformsCorrectly(new Matrix(a, 0, 0, d));
            _checkTransformsCorrectly(new Matrix(0, b, c, 0));
            _checkTransformsCorrectly(new Matrix(0, b, 0, d));
            _checkTransformsCorrectly(new Matrix(0, 0, c, d));

            _checkTransformsCorrectly(new Matrix(a, 0, 0, 0));
            _checkTransformsCorrectly(new Matrix(0, b, 0, 0));
            _checkTransformsCorrectly(new Matrix(0, 0, c, 0));
            _checkTransformsCorrectly(new Matrix(0, 0, 0, d));

            _checkTransformsCorrectly(new Matrix(1, 0, 0, 1));
            _checkTransformsCorrectly(new Matrix(0, 0, 0, 0));

        }
    }
}
