using System;
using GRaff;
using Xunit;

namespace GRaff.UnitTesting
{
	public class MatrixTest
	{
		[Fact]
		public void Matrix_Properties()
		{
			var matrix = new Matrix(0, 1, 2, 10, 11, 12);
			Assert.Equal(0, matrix.M00);
			Assert.Equal(1, matrix.M01);
			Assert.Equal(2, matrix.M02);
			Assert.Equal(10, matrix.M10);
			Assert.Equal(11, matrix.M11);
			Assert.Equal(12, matrix.M12);

			Assert.Equal(new Matrix(1, 0, 0, 0, 1, 0), new Matrix());
		}

		[Fact]
		public void Matrix_DeterminantInverse()
		{
			var affine = new Matrix(5, 3, 9, 1, 5, 2);
			Assert.Equal(22, affine.Determinant);
			Assert.Equal(new Matrix(5.0 / 22, -3.0 / 22, -39.0 / 22, -1.0 / 22, 5.0 / 22, -1.0 / 22), affine.Inverse);
			Assert.Equal(affine, affine.Inverse.Inverse);
		}

		[Fact]
		public void Matrix_Operations()
		{
			Assert.Equal(new Matrix(11, 22, 33, 44, 55, 66), new Matrix(1, 2, 3, 4, 5, 6) + new Matrix(10, 20, 30, 40, 50, 60));
			Assert.Equal(new Matrix(10, 20, 30, 40, 50, 60), new Matrix(11, 22, 33, 44, 55, 66) - new Matrix(1, 2, 3, 4, 5, 6));
			Assert.Equal(new Matrix(12, 15, 21, 12, 15, 21), new Matrix(1, 2, 3, 1, 2, 3) * new Matrix(4, 5, 6, 4, 5, 6));
		}

		[Fact]
		public void Matrix_Transformations()
		{
			var t = Angle.Rad(1);
			double sx = 2.72, sy = 6.28;
			double tx = GRandom.Double(), ty = GRandom.Double();
			double kx = 2, ky = 13;

			var affine = new Matrix(1, 9, 8, 6, 9, 1);
			Assert.Equal(Matrix.Rotation(t) * affine, affine.Rotate(t));
			Assert.Equal(Matrix.Scaling(sx, sy) * affine, affine.Scale(sx, sy));
			Assert.Equal(Matrix.Translation(tx, ty) * affine, affine.Translate(tx, ty));
			Assert.Equal(Matrix.Shearing(kx, ky) * affine, affine.Shear(kx, ky));
		}

		[Fact]
		public void Matrix_Mapping()
		{
			Triangle src, dst;

			src = new Triangle(0, 0, 1, 0, 0, 1);
			dst = new Triangle(0, 0, 2, 0, 0, 3);
			Assert.Equal(Matrix.Scaling(2, 3), Matrix.Mapping(src, dst));

			src = new Triangle(0, 0, 1, 0, 1, 1);
			dst = new Triangle(0, 0, 0, 1, -1, 1);
			Assert.Equal(Matrix.Rotation(Angle.Deg(90)), Matrix.Mapping(src, dst));
		}
	}
}
