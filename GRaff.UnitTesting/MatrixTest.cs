using System;
using GRaff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameMaker.UnitTesting
{
	[TestClass]
	public class MatrixTest
	{
		[TestMethod]
		public void Matrix_Properties()
		{
			var affine = new AffineMatrix(0, 1, 2, 10, 11, 12);
			Assert.AreEqual(0, affine.M00);
			Assert.AreEqual(1, affine.M01);
			Assert.AreEqual(2, affine.M02);
			Assert.AreEqual(10, affine.M10);
			Assert.AreEqual(11, affine.M11);
			Assert.AreEqual(12, affine.M12);

			var linear = new LinearMatrix(0, 1, 10, 11);
			Assert.AreEqual(0, linear.M00);
			Assert.AreEqual(1, linear.M01);
			Assert.AreEqual(10, linear.M10);
			Assert.AreEqual(11, linear.M11);

			Assert.AreEqual(new AffineMatrix(1, 0, 0, 0, 1, 0), new AffineMatrix());
			Assert.AreEqual(new LinearMatrix(1, 0, 0, 1), new LinearMatrix());
		}

		[TestMethod]
		public void Matrix_DeterminantInverse()
		{
			var affine = new AffineMatrix(5, 3, 9, 1, 5, 2);
			Assert.AreEqual(22, affine.Determinant);
			Assert.AreEqual(new AffineMatrix(5.0 / 22, -3.0 / 22, -39.0 / 22, -1.0 / 22, 5.0 / 22, -1.0 / 22), affine.Inverse);
			Assert.AreEqual(affine, affine.Inverse.Inverse);

			var linear = new LinearMatrix(5, 3, 1, 6);
			Assert.AreEqual(27, linear.Determinant);
			Assert.AreEqual(new LinearMatrix(6.0 / 27, -3.0 / 27, -1.0 / 27, 5.0 / 27), linear.Inverse);
			Assert.AreEqual(linear, linear.Inverse.Inverse);
		}

		[TestMethod]
		public void Matrix_Operations()
		{
			Assert.AreEqual(new AffineMatrix(11, 22, 33, 44, 55, 66), new AffineMatrix(1, 2, 3, 4, 5, 6) + new AffineMatrix(10, 20, 30, 40, 50, 60));
			Assert.AreEqual(new AffineMatrix(10, 20, 30, 40, 50, 60), new AffineMatrix(11, 22, 33, 44, 55, 66) - new AffineMatrix(1, 2, 3, 4, 5, 6));
			Assert.AreEqual(new AffineMatrix(12, 15, 21, 12, 15, 21), new AffineMatrix(1, 2, 3, 1, 2, 3) * new AffineMatrix(4, 5, 6, 4, 5, 6));

			Assert.AreEqual(new LinearMatrix(11, 22, 33, 44), new LinearMatrix(1, 2, 3, 4) + new LinearMatrix(10, 20, 30, 40));
			Assert.AreEqual(new LinearMatrix(10, 20, 30, 40), new LinearMatrix(11, 22, 33, 44) - new LinearMatrix(1, 2, 3, 4));
			Assert.AreEqual(new LinearMatrix(19, 22, 43, 50), new LinearMatrix(1, 2, 3, 4) * new LinearMatrix(5, 6, 7, 8));
		}

		[TestMethod]
		public void Matrix_Transformations()
		{
			var t = Angle.Rad(1);
			double sx = 2.72, sy = 6.28;
			double tx = GRandom.Double(), ty = GRandom.Double();
			double kx = 2, ky = 13;

			var affine = new AffineMatrix(1, 9, 8, 6, 9, 1);
			Assert.AreEqual(AffineMatrix.Rotation(t) * affine, affine.Rotate(t));
			Assert.AreEqual(AffineMatrix.Scaling(sx, sy) * affine, affine.Scale(sx, sy));
			Assert.AreEqual(AffineMatrix.Translation(tx, ty) * affine, affine.Translate(tx, ty));
			Assert.AreEqual(AffineMatrix.Shearing(kx, ky) * affine, affine.Shear(kx, ky));

			var linear = new LinearMatrix(11, 6, 11, 1);
			Assert.AreEqual(LinearMatrix.Rotation(t) * linear, linear.Rotate(t));
			Assert.AreEqual(LinearMatrix.Scaling(sx, sy) * linear, linear.Scale(sx, sy));
			Assert.AreEqual(LinearMatrix.Shearing(kx, ky) * linear, linear.Shear(kx, ky));
		}
	}
}
