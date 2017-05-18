using System;
using GRaff;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GRaff.UnitTesting
{
	[TestClass]
	public class MatrixTest
	{
		[TestMethod]
		public void Matrix_Properties()
		{
			var matrix = new Matrix(0, 1, 2, 10, 11, 12);
			Assert.AreEqual(0, matrix.M00);
			Assert.AreEqual(1, matrix.M01);
			Assert.AreEqual(2, matrix.M02);
			Assert.AreEqual(10, matrix.M10);
			Assert.AreEqual(11, matrix.M11);
			Assert.AreEqual(12, matrix.M12);

			Assert.AreEqual(new Matrix(1, 0, 0, 0, 1, 0), new Matrix());
		}

		[TestMethod]
		public void Matrix_DeterminantInverse()
		{
			var affine = new Matrix(5, 3, 9, 1, 5, 2);
			Assert.AreEqual(22, affine.Determinant);
			Assert.AreEqual(new Matrix(5.0 / 22, -3.0 / 22, -39.0 / 22, -1.0 / 22, 5.0 / 22, -1.0 / 22), affine.Inverse);
			Assert.AreEqual(affine, affine.Inverse.Inverse);
		}

		[TestMethod]
		public void Matrix_Operations()
		{
			Assert.AreEqual(new Matrix(11, 22, 33, 44, 55, 66), new Matrix(1, 2, 3, 4, 5, 6) + new Matrix(10, 20, 30, 40, 50, 60));
			Assert.AreEqual(new Matrix(10, 20, 30, 40, 50, 60), new Matrix(11, 22, 33, 44, 55, 66) - new Matrix(1, 2, 3, 4, 5, 6));
			Assert.AreEqual(new Matrix(12, 15, 21, 12, 15, 21), new Matrix(1, 2, 3, 1, 2, 3) * new Matrix(4, 5, 6, 4, 5, 6));
		}

		[TestMethod]
		public void Matrix_Transformations()
		{
			var t = Angle.Rad(1);
			double sx = 2.72, sy = 6.28;
			double tx = GRandom.Double(), ty = GRandom.Double();
			double kx = 2, ky = 13;

			var affine = new Matrix(1, 9, 8, 6, 9, 1);
			Assert.AreEqual(Matrix.Rotation(t) * affine, affine.Rotate(t));
			Assert.AreEqual(Matrix.Scaling(sx, sy) * affine, affine.Scale(sx, sy));
			Assert.AreEqual(Matrix.Translation(tx, ty) * affine, affine.Translate(tx, ty));
			Assert.AreEqual(Matrix.Shearing(kx, ky) * affine, affine.Shear(kx, ky));
		}

		[TestMethod]
		public void Matrix_Mapping()
		{
			Triangle src, dst;

			src = new Triangle(0, 0, 1, 0, 0, 1);
			dst = new Triangle(0, 0, 2, 0, 0, 3);
			Assert.AreEqual(Matrix.Scaling(2, 3), Matrix.Mapping(src, dst));

			src = new Triangle(0, 0, 1, 0, 1, 1);
			dst = new Triangle(0, 0, 0, 1, -1, 1);
			Assert.AreEqual(Matrix.Rotation(Angle.Deg(90)), Matrix.Mapping(src, dst));
		}
	}
}
