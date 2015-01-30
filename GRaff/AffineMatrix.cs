using System;


namespace GRaff
{
	/// <summary>
	/// Represents the matrix of an affine transformation.
	/// </summary>
	public sealed class AffineMatrix : ICloneable
	{
		/// <summary>
		/// Initializes a new instance of the GRaff.AffineMatrix class as an identity matrix.
		/// </summary>
		public AffineMatrix()
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
		public AffineMatrix(double m00, double m01, double m02, double m10, double m11, double m12)
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M02 = m02;
			this.M10 = m10;
			this.M11 = m11;
			this.M12 = m12;
		}

		/// <summary>
		/// Creates a GRaff.AffineMatrix representing a scaling transformation.
		/// </summary>
		/// <param name="scaleX">The horizontal scale factor.</param>
		/// <param name="scaleY">The vertical scale factor</param>
		/// <returns>A new GRaff.AffineMatrix representing the transformation.</returns>
		public static AffineMatrix Scaling(double scaleX, double scaleY) { return new AffineMatrix(scaleX, 0, 0, 0, scaleY, 0); }

		/// <summary>
		/// Creates a GRaff.AffineMatrix representing a shear transformation.
		/// </summary>
		/// <param name="shearX">The horizontal shear factor.</param>
		/// <param name="shearY">The vertical shear factor.</param>
		/// <returns>A new GRaff.AffineMatrix representing the transformation.</returns>
		public static AffineMatrix Shearing(double shearX, double shearY) { return new AffineMatrix(1, shearX, 0, shearY, 1, 0); }

		/// <summary>
		/// Creates a GRaff.AffineMatrix representing a rotation transform around the origin.
		/// </summary>
		/// <param name="a">The angle to rotate by.</param>
		/// <returns>A new GRaff.AffineMatrix representing the transformation.</returns>
		public static AffineMatrix Rotation(Angle a) { return new AffineMatrix(GMath.Cos(a), -GMath.Sin(a), 0, GMath.Sin(a), GMath.Cos(a), 0); }

		/// <summary>
		/// Creates a GRaff.AffineMatrix representing a translation trnasformation.
		/// </summary>
		/// <param name="dx">The amount to translate by in the horizontal direction.</param>
		/// <param name="dy">The amount to translate by in the vertical direction.</param>
		/// <returns>A new GRaff.AffineMatrix representing the transformation.</returns>
		public static AffineMatrix Translation(double dx, double dy) { return new AffineMatrix(1, 0, dx, 0, 1, dy); }

		/// <summary>
		/// Gets the first element of the first row of this GRaff.AffineMatrix.
		/// </summary>
		public double M00 { get; private set; }

		/// <summary>
		/// Gets the second element of the first row of this GRaff.AffineMatrix.
		/// </summary>
		public double M01 { get; private set; }

		/// <summary>
		/// Gets the third element of the first row of this GRaff.AffineMatrix.
		/// </summary>
		public double M02 { get; private set; }

		/// <summary>
		/// Gets the first element of the second row of this GRaff.AffineMatrix.
		/// </summary>
		public double M10 { get; private set; }

		/// <summary>
		/// Gets the second element of the second row of this GRaff.AffineMatrix.
		/// </summary>
		public double M11 { get; private set; }

		/// <summary>
		/// Gets the third element of the second row of this GRaff.AffineMatrix.
		/// </summary>
		public double M12 { get; private set; }

		/// <summary>
		/// Gets the determinant of this GRaff.AffineMatrix.
		/// </summary>
		public double Determinant
		{
			get
			{
				return M00 * M11 - M01 * M10;
			}
		}

		/// <summary>
		/// Gets the inverse of this GRaff.AffineMatrix.
		/// </summary>
		/// <exception cref="GRaff.MatrixException">If the determinant is zero.</exception>
		public AffineMatrix Inverse
		{
			get
			{
				double det = Determinant;
				if (det == 0)
					throw new MatrixException("Cannot invert a matrix with determinant 0.");
				return new AffineMatrix(M11 / det, -M01 / det, (-M02 * M11 + M01 * M12) / det, -M10 / det, M00 / det, (M02 * M10 - M00 * M12) / det);
			}
		}


		/// <summary>
		/// Applies a scaling transformation to this GRaff.AffineMatrix.
		/// </summary>
		/// <param name="scaleX">The horizontal scale factor.</param>
		/// <param name="scaleY">The vertical scale factor.</param>
		/// <returns>This GRaff.AffineMatrix, after the transformation.</returns>
		public AffineMatrix Scale(double scaleX, double scaleY)
		{
			return new AffineMatrix(M00 * scaleX, M01 * scaleX, M02 * scaleX, M10 * scaleY, M11 * scaleY, M12 * scaleY);
		}

		/// <summary>
		/// Applies a rotation transformation to this GRaff.AffineMatrix.
		/// </summary>
		/// <param name="a">The angle to rotate by.</param>
		/// <returns>This GRaff.AffineMatrix, after the transformation.</returns>
		public AffineMatrix Rotate(Angle a)
		{
			double c = GMath.Cos(a), s = GMath.Sin(a);
			return new AffineMatrix(M00 * c - M10 * s, M01 * c - M11 * s, M02 * c - M12 * s, M00 * s + M10 * c, M01 * s + M11 * c, M02 * s + M12 * c);
		}

		/// <summary>
		/// Applies a translation transformation to this GRaff.AffineMatrix.
		/// </summary>
		/// <param name="tx">The horizontal translation.</param>
		/// <param name="ty">The vertical translation.</param>
		/// <returns>This GRaff.AffineMatrix, after the transformation.</returns>
		public AffineMatrix Translate(double tx, double ty)
		{
			return new AffineMatrix(M00, M01, M02 + tx, M10, M11, M12 + ty);
		}

		/// <summary>
		/// Applies a shear transformation to this GRaff.AffineMatrix.
		/// </summary>
		/// <param name="shearX">The horizontal shear factor.</param>
		/// <param name="shearY">The vertical shear factor.</param>
		/// <returns>This GRaff.AffineMatrix, after the transformation.</returns>
		public AffineMatrix Shear(double shearX, double shearY)
		{
			return new AffineMatrix(M00 + shearX * M10, M01 + shearX * M11, M02 + shearX * M12, M10 + shearY * M00, M11 + shearY * M01, M12 + shearY * M02);
		}

		/// <summary>
		/// Specifies whether this GRaff.AffineMatrix contains the same elements as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.AffineMatrix and has the same elements as this GRaff.AffineMatrix.</returns>
		public override bool Equals(object obj) { return ((obj is AffineMatrix) ? (this == (AffineMatrix)obj) : base.Equals(obj)); }

		/// <summary>
		/// Returns a hash code for this GRaff.AffineMatrix.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.AffineMatrix.</returns>
		public override int GetHashCode() { return GMath.HashCombine(M00.GetHashCode(), M01.GetHashCode(), M02.GetHashCode(), M10.GetHashCode(), M11.GetHashCode(), M12.GetHashCode()); }

		/// <summary>
		/// Converts this GRaff.AffineMatrix to a human-readable string, displaying the values of the elements.
		/// </summary>
		/// <returns>A string that represents this GRaff.AffineMatrix.</returns>
		public override string ToString()
		{
			return string.Format("[[{0}, {1}, {2}], [{3}, {4}, {5}]]", M00, M01, M02, M10, M11, M12);
		}

		/// <summary>
		/// Creates a deep clone of this GRaff.AffineMatrix.
		/// </summary>
		/// <returns>A deep clone of this GRaff.AffineMatrix.</returns>
		public AffineMatrix Clone()
		{
			return new AffineMatrix(M00, M01, M02, M10, M11, M12);
		}

		/// <summary>
		/// Creates a deep clone of this GRaff.LinearMatrix.
		/// </summary>
		/// <returns>A deep clone of this GRaff.LinearMatrix.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}

		private double _magnitude
		{
			get { return M00 * M00 + M01 * M01 + M02 * M02 + M10 * M10 + M11 * M11 + M12 * M12; }
		}

		/// <summary>
		/// Compares two GRaff.AffineMatrix objects. The result specifies whether all their elements are equal.
		/// </summary>
		/// <param name="left">The first GRaff.AffineMatrix to compare.</param>
		/// <param name="right">The second GRaff.AffineMatrix to compare.</param>
		/// <returns>true if all elements of the two GRaff.AffineMatrix objects are equal.</returns>
		public static bool operator ==(AffineMatrix left, AffineMatrix right)   /*C#6.0*/ // left?.Equals(right) ?? right == null;
		{
			if (ReferenceEquals(left, null))
				return object.ReferenceEquals(right, null);
			else
				return (left - right)._magnitude < 10e-14;
		}

		/// <summary>
		/// Compares two GRaff.AffineMatrix objects. The result specifies whether all their elements are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.AffineMatrix to compare.</param>
		/// <param name="right">The second GRaff.AffineMatrix to compare.</param>
		/// <returns>true if all elements of the two GRaff.AffineMatrix objects are unequal.</returns>
		public static bool operator !=(AffineMatrix left, AffineMatrix right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Computes the element-wise sum of the two GRaff.AffineMatrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.AffineMatrix.</param>
		/// <param name="right">The second GRaff.AffineMatrix.</param>
		/// <returns>The sum of the elements of each GRaff.AffineMatrix.</returns>
		public static AffineMatrix operator +(AffineMatrix left, AffineMatrix right) { return new AffineMatrix(left.M00 + right.M00, left.M01 + right.M01, left.M02 + right.M02, left.M10 + right.M10, left.M11 + right.M11, left.M12 + right.M12); }

		/// <summary>
		/// Computes the element-wise difference of the two GRaff.AffineMatrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.AffineMatrix.</param>
		/// <param name="right">The second GRaff.AffineMatrix.</param>
		/// <returns>The difference of the elements of each GRaff.AffineMatrix.</returns>
		public static AffineMatrix operator -(AffineMatrix left, AffineMatrix right) { return new AffineMatrix(left.M00 - right.M00, left.M01 - right.M01, left.M02 - right.M02, left.M10 - right.M10, left.M11 - right.M11, left.M12 - right.M12); }

		/// <summary>
		/// Computes the matrix product of the two GRaff.AffineMatrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.AffineMatrix.</param>
		/// <param name="right">The second GRaff.AffineMatrix.</param>
		/// <returns>The matrix product of the two GRaff.AffineMatrix.</returns>
		public static AffineMatrix operator *(AffineMatrix left, AffineMatrix right)
		{
			return new AffineMatrix(
				left.M00 * right.M00 + left.M01 * right.M10, left.M00 * right.M01 + left.M01 * right.M11, left.M00 * right.M02 + left.M01 * right.M12 + left.M02,
				left.M10 * right.M00 + left.M11 * right.M10, left.M10 * right.M01 + left.M11 * right.M11, left.M10 * right.M02 + left.M11 * right.M12 + left.M12
			);
		}

		/// <summary>
		/// Computes the matrix product of the GRaff.AffineMatrix and the GRaff.Point.
		/// This constitutes performing the affine transformation on that GRaff.Point.
		/// </summary>
		/// <param name="m">A GRaff.AffineMatrix representing the affine transformation.</param>
		/// <param name="p">A GRaff.Point to be transformed by the affine transformation.</param>
		/// <returns>The transformed GRaff.Point.</returns>
		public static Point operator *(AffineMatrix m, Point p) { return new Point(m.M00 * p.X + m.M01 * p.Y + m.M02, m.M10 * p.X + m.M11 * p.Y + m.M12); }

		/// <summary>
		/// Computes the matrix product of the GRaff.AffineMatrix and the GRaff.Vector.
		/// This constitutes performing the affine transformation on that GRaff.Vector.
		/// </summary>
		/// <param name="m">A GRaff.AffineMatrix representing the affine transformation.</param>
		/// <param name="v">A GRaff.Vector to be transformed by the affine transformation.</param>
		/// <returns>The transformed GRaff.Vector.</returns>
		public static Vector operator *(AffineMatrix m, Vector v) { return new Vector(m.M00 * v.X + m.M01 * v.Y, m.M10 * v.X + m.M11 * v.Y); }
	}
}
