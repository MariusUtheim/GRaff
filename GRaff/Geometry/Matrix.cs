using System;
using System.Diagnostics.Contracts;

namespace GRaff.Geometry
{
	/// <summary>
	/// Represents the matrix of an affine transformation.
	/// </summary>
	public sealed class Matrix : ICloneable, IEquatable<Matrix>
	{

		/// <summary>
		/// Initializes a new instance of the GRaff.Matrix class as an identity matrix.
		/// </summary>
		public Matrix()
			: this(1, 0, 0, 0, 1, 0)
		{ }

		/// <summary>
		/// Initializes a new instance of the GRaff.AffineMatix class with the specified matrix elements.
		/// </summary>
		/// <param name="m00">The first element of the first row.</param>
		/// <param name="m01">The second element of the first row.</param>
		/// <param name="m02">The third element of the first row.</param>
		/// <param name="m10">The first element of the second row.</param>
		/// <param name="m11">The second element of the second row.</param>
		/// <param name="m12">The third element of the second row.</param>
		public Matrix(double m00, double m01, double m02, double m10, double m11, double m12)
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M02 = m02;
			this.M10 = m10;
			this.M11 = m11;
			this.M12 = m12;
		}

		public static Matrix Identity { get; } = new Matrix();


		/// <summary>
		/// Creates a GRaff.Matrix representing a scaling transformation.
		/// </summary>
		/// <param name="scaleX">The horizontal scale factor.</param>
		/// <param name="scaleY">The vertical scale factor</param>
		/// <returns>A new GRaff.Matrix representing the transformation.</returns>
		public static Matrix Scaling(double scaleX, double scaleY)
		{
			Contract.Ensures(Contract.Result<Matrix>() != null);
			return new Matrix(scaleX, 0, 0, 0, scaleY, 0);
		}

		/// <summary>
		/// Creates a GRaff.Matrix representing a shear transformation.
		/// </summary>
		/// <param name="shearX">The horizontal shear factor.</param>
		/// <param name="shearY">The vertical shear factor.</param>
		/// <returns>A new GRaff.Matrix representing the transformation.</returns>
		public static Matrix Shearing(double shearX, double shearY)
		{
			Contract.Ensures(Contract.Result<Matrix>() != null);
			return new Matrix(1, shearX, 0, shearY, 1, 0);
		}

		/// <summary>
		/// Creates a GRaff.Matrix representing a rotation transform around the origin.
		/// </summary>
		/// <param name="a">The angle to rotate by.</param>
		/// <returns>A new GRaff.Matrix representing the transformation.</returns>
		public static Matrix Rotation(Angle a)
		{
			Contract.Ensures(Contract.Result<Matrix>() != null);
			return new Matrix(GMath.Cos(a), -GMath.Sin(a), 0, GMath.Sin(a), GMath.Cos(a), 0);
		}

		/// <summary>
		/// Creates a GRaff.Matrix representing a translation trnasformation.
		/// </summary>
		/// <param name="dx">The amount to translate by in the horizontal direction.</param>
		/// <param name="dy">The amount to translate by in the vertical direction.</param>
		/// <returns>A new GRaff.Matrix representing the transformation.</returns>
		public static Matrix Translation(double dx, double dy)
		{
			Contract.Ensures(Contract.Result<Matrix>() != null);
			return new Matrix(1, 0, dx, 0, 1, dy);
		}

		public static Matrix Mapping(Triangle src, Triangle dst)
		{
			Contract.Ensures(Contract.Result<Matrix>() != null);
			double c12 = src.X1 * src.Y2 - src.X2 * src.Y1;
			double c23 = src.X2 * src.Y3 - src.X3 * src.Y2;
			double c31 = src.X3 * src.Y1 - src.X1 * src.Y3;
			double determinant = c12 + c23 + c31;

			if (determinant == 0)
				throw new MatrixException("The components of the source triangle are not linearly independent.");

			return new Matrix(
				(src.Y2 - src.Y3) * dst.X1 + (src.Y3 - src.Y1) * dst.X2 + (src.Y1 - src.Y2) * dst.X3,
				(src.X3 - src.X2) * dst.X1 + (src.X1 - src.X3) * dst.X2 + (src.X2 - src.X1) * dst.X3,
				c23 * dst.X1 + c31 * dst.X2 + c12 * dst.X3,
				(src.Y2 - src.Y3) * dst.Y1 + (src.Y3 - src.Y1) * dst.Y2 + (src.Y1 - src.Y2) * dst.Y3,
				(src.X3 - src.X2) * dst.Y1 + (src.X1 - src.X3) * dst.Y2 + (src.X2 - src.X1) * dst.Y3,
				c23 * dst.Y1 + c31 * dst.Y2 + c12 * dst.Y3
			) / determinant;
		}

		/// <summary>
		/// Gets the first element of the first row of this GRaff.Matrix.
		/// </summary>
		public double M00 { get; private set; }

		/// <summary>
		/// Gets the second element of the first row of this GRaff.Matrix.
		/// </summary>
		public double M01 { get; private set; }

		/// <summary>
		/// Gets the third element of the first row of this GRaff.Matrix.
		/// </summary>
		public double M02 { get; private set; }

		/// <summary>
		/// Gets the first element of the second row of this GRaff.Matrix.
		/// </summary>
		public double M10 { get; private set; }

		/// <summary>
		/// Gets the second element of the second row of this GRaff.Matrix.
		/// </summary>
		public double M11 { get; private set; }

		/// <summary>
		/// Gets the third element of the second row of this GRaff.Matrix.
		/// </summary>
		public double M12 { get; private set; }

		/// <summary>
		/// Gets the determinant of this GRaff.Matrix.
		/// </summary>
		public double Determinant => M00 * M11 - M01 * M10;

		/// <summary>
		/// Gets the inverse of this GRaff.Matrix.
		/// </summary>
		/// <exception cref="GRaff.MatrixException">If the determinant is zero.</exception>
		public Matrix Inverse
		{
			get
			{
				Contract.Ensures(Contract.Result<Matrix>() != null);
				double det = Determinant;
				if (det == 0)
					throw new MatrixException();
				return new Matrix(M11 / det, -M01 / det, (-M02 * M11 + M01 * M12) / det, -M10 / det, M00 / det, (M02 * M10 - M00 * M12) / det);
			}
		}


		/// <summary>
		/// Applies a scaling transformation to this GRaff.Matrix.
		/// </summary>
		/// <param name="scaleX">The horizontal scale factor.</param>
		/// <param name="scaleY">The vertical scale factor.</param>
		/// <returns>This GRaff.Matrix, after the transformation.</returns>
		public Matrix Scale(double scaleX, double scaleY)
			=> new Matrix(M00 * scaleX, M01 * scaleX, M02 * scaleX, M10 * scaleY, M11 * scaleY, M12 * scaleY);


		/// <summary>
		/// Applies a rotation transformation to this GRaff.Matrix.
		/// </summary>
		/// <param name="a">The angle to rotate by.</param>
		/// <returns>This GRaff.Matrix, after the transformation.</returns>
		public Matrix Rotate(Angle a)
		{
			double c = GMath.Cos(a), s = GMath.Sin(a);
			return new Matrix(M00 * c - M10 * s, M01 * c - M11 * s, M02 * c - M12 * s, M00 * s + M10 * c, M01 * s + M11 * c, M02 * s + M12 * c);
		}

		/// <summary>
		/// Applies a translation transformation to this GRaff.Matrix.
		/// </summary>
		/// <param name="tx">The horizontal translation.</param>
		/// <param name="ty">The vertical translation.</param>
		/// <returns>This GRaff.Matrix, after the transformation.</returns>
		public Matrix Translate(double tx, double ty)
			=> new Matrix(M00, M01, M02 + tx, M10, M11, M12 + ty);

		/// <summary>
		/// Applies a shear transformation to this GRaff.Matrix.
		/// </summary>
		/// <param name="shearX">The horizontal shear factor.</param>
		/// <param name="shearY">The vertical shear factor.</param>
		/// <returns>This GRaff.Matrix, after the transformation.</returns>
		public Matrix Shear(double shearX, double shearY)
			=> new Matrix(M00 + shearX * M10, M01 + shearX * M11, M02 + shearX * M12, M10 + shearY * M00, M11 + shearY * M01, M12 + shearY * M02);

		
		public bool Equals(Matrix other)
			=> (!ReferenceEquals(other, null)) && (this - other)._magnitude <= GMath.MachineEpsilon;

		/// <summary>
		/// Specifies whether this GRaff.Matrix contains the same elements as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.Matrix and has the same elements as this GRaff.Matrix.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Matrix)
				return Equals((Matrix)obj);
			else
				return base.Equals(obj);
		}

		/// <summary>
		/// Returns a hash code for this GRaff.Matrix.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.Matrix.</returns>
		public override int GetHashCode()
			=> GMath.HashCombine(M00.GetHashCode(), M01.GetHashCode(), M02.GetHashCode(), M10.GetHashCode(), M11.GetHashCode(), M12.GetHashCode());

		/// <summary>
		/// Converts this GRaff.Matrix to a human-readable string, displaying the values of the elements.
		/// </summary>
		/// <returns>A string that represents this GRaff.Matrix.</returns>
		public override string ToString() => $"[[{M00}, {M01}, {M02}], [{M10}, {M11}, {M12}]]";

		/// <summary>
		/// Creates a deep clone of this GRaff.Matrix.
		/// </summary>
		/// <returns>A deep clone of this GRaff.Matrix.</returns>
		public Matrix Clone() => new Matrix(M00, M01, M02, M10, M11, M12);

		/// <summary>
		/// Creates a deep clone of this GRaff.LinearMatrix.
		/// </summary>
		/// <returns>A deep clone of this GRaff.LinearMatrix.</returns>
		object ICloneable.Clone() => Clone();

		private double _magnitude => M00 * M00 + M01 * M01 + M02 * M02 + M10 * M10 + M11 * M11 + M12 * M12;

		/// <summary>
		/// Compares two GRaff.Matrix objects. The result specifies whether all their elements are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Matrix to compare.</param>
		/// <param name="right">The second GRaff.Matrix to compare.</param>
		/// <returns>true if all elements of the two GRaff.Matrix objects are equal.</returns>
		public static bool operator ==(Matrix left, Matrix right)
			=> left?.Equals(right) ?? ReferenceEquals(null, right);

		/// <summary>
		/// Compares two GRaff.Matrix objects. The result specifies whether all their elements are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.Matrix to compare.</param>
		/// <param name="right">The second GRaff.Matrix to compare.</param>
		/// <returns>true if all elements of the two GRaff.Matrix objects are unequal.</returns>
		public static bool operator !=(Matrix left, Matrix right)
			=> !(left == right);

		/// <summary>
		/// Computes the element-wise sum of the two GRaff.Matrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.Matrix.</param>
		/// <param name="right">The second GRaff.Matrix.</param>
		/// <returns>The sum of the elements of each GRaff.Matrix.</returns>
		public static Matrix operator +(Matrix left, Matrix right)
		{
			Contract.Requires<ArgumentNullException>(left != null && right != null);
			return new Matrix(left.M00 + right.M00, left.M01 + right.M01, left.M02 + right.M02, left.M10 + right.M10, left.M11 + right.M11, left.M12 + right.M12);
		}


		/// <summary>
		/// Computes the element-wise difference of the two GRaff.Matrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.Matrix.</param>
		/// <param name="right">The second GRaff.Matrix.</param>
		/// <returns>The difference of the elements of each GRaff.Matrix.</returns>
		public static Matrix operator -(Matrix left, Matrix right)
		{
			Contract.Requires<ArgumentNullException>(left != null && right != null);
			return new Matrix(left.M00 - right.M00, left.M01 - right.M01, left.M02 - right.M02, left.M10 - right.M10, left.M11 - right.M11, left.M12 - right.M12);
		}

		/// <summary>
		/// Computes the matrix product of the two GRaff.Matrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.Matrix.</param>
		/// <param name="right">The second GRaff.Matrix.</param>
		/// <returns>The matrix product of the two GRaff.Matrix.</returns>
		public static Matrix operator *(Matrix left, Matrix right)
		{
			Contract.Requires<ArgumentNullException>(left != null && right != null);
			return new Matrix(
					left.M00 * right.M00 + left.M01 * right.M10, left.M00 * right.M01 + left.M01 * right.M11, left.M00 * right.M02 + left.M01 * right.M12 + left.M02,
					left.M10 * right.M00 + left.M11 * right.M10, left.M10 * right.M01 + left.M11 * right.M11, left.M10 * right.M02 + left.M11 * right.M12 + left.M12
				);
		}

		public static Matrix operator *(Matrix left, double right)
		{
			Contract.Requires<ArgumentNullException>(left != null);
			return new Matrix(left.M00 * right, left.M01 * right, left.M02 * right, left.M10 * right, left.M11 * right, left.M12 * right);
		}

		public static Matrix operator /(Matrix left, double right)
		{
			Contract.Requires<ArgumentNullException>(left != null);
			return new Matrix(left.M00 / right, left.M01 / right, left.M02 / right, left.M10 / right, left.M11 / right, left.M12 / right);
		}

		/// <summary>
		/// Computes the matrix product of the GRaff.Matrix and the GRaff.Point.
		/// This constitutes performing the affine transformation on that GRaff.Point.
		/// </summary>
		/// <param name="m">A GRaff.Matrix representing the affine transformation.</param>
		/// <param name="p">A GRaff.Point to be transformed by the affine transformation.</param>
		/// <returns>The transformed GRaff.Point.</returns>
		public static Point operator *(Matrix m, Point p)
		{
			Contract.Requires<ArgumentNullException>(m != null);
			return new Point(m.M00 * p.X + m.M01 * p.Y + m.M02, m.M10 * p.X + m.M11 * p.Y + m.M12);
		}

		/// <summary>
		/// Computes the matrix product of the GRaff.Matrix and the GRaff.Vector.
		/// This constitutes performing the affine transformation on that GRaff.Vector.
		/// </summary>
		/// <param name="m">A GRaff.Matrix representing the affine transformation.</param>
		/// <param name="v">A GRaff.Vector to be transformed by the affine transformation.</param>
		/// <returns>The transformed GRaff.Vector.</returns>
		public static Vector operator *(Matrix m, Vector v)
		{
			Contract.Requires<ArgumentNullException>(m != null);
			return new Vector(m.M00 * v.X + m.M01 * v.Y, m.M10 * v.X + m.M11 * v.Y);
		}
	}
}
