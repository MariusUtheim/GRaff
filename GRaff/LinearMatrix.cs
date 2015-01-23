using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	/// <summary>
	/// Represents the matrix of a linear transformation.
	/// </summary>
	public sealed class LinearMatrix : ICloneable
	{
		/// <summary>
		/// Initializes a new instance of the GRaff.LinearMatrix class as an identity matrix.
		/// </summary>
		public LinearMatrix()
			: this(1, 0, 0, 1)
		{ }

		/// <summary>
		/// Initializes a new instance of the GRaff.LinearMatrix class with the specified matrix elements.
		/// </summary>
		/// <param name="m00">The first element of the first row.</param>
		/// <param name="m01">The second element of the first row.</param>
		/// <param name="m10">The first element of the second row.</param>
		/// <param name="m11">The second element of the second row.</param>
		public LinearMatrix(double m00, double m01, double m10, double m11)
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M10 = m10;
			this.M11 = m11;
		}

		/// <summary>
		/// Creates a GRaff.LinearMatrix representing a scaling transformation.
		/// </summary>
		/// <param name="scaleX">The horizontal scale factor.</param>
		/// <param name="scaleY">The vertical scale factor</param>
		/// <returns>A new GRaff.LinearMatrix representing the transformation.</returns>
		public static LinearMatrix Scaling(double scaleX, double scaleY) { return new LinearMatrix(scaleX, 0, 0, scaleY); }

		/// <summary>
		/// Creates a GRaff.LinearMatrix representing a shear transformation.
		/// </summary>
		/// <param name="shearX">The horizontal shear factor.</param>
		/// <param name="shearY">The vertical shear factor.</param>
		/// <returns>A new GRaff.LinearMatrix representing the transformation.</returns>
		public static LinearMatrix Shearing(double shearX, double shearY) { return new LinearMatrix(1, shearX, shearY, 1); }

		/// <summary>
		/// Creates a GRaff.AffineMatrix representing a rotation transform around the origin.
		/// </summary>
		/// <param name="a">The angle to rotate by.</param>
		/// <returns>A new GRaff.AffineMatrix representing the transformation.</returns>
		public static LinearMatrix Rotation(Angle a) { return new LinearMatrix(GMath.Cos(a), -GMath.Sin(a), GMath.Sin(a), GMath.Cos(a)); }

		/// <summary>
		/// Gets the first element of the first row of this GRaff.AffineMatrix.
		/// </summary>
		public double M00 { get; set; }

		/// <summary>
		/// Gets the second element of the first row of this GRaff.AffineMatrix.
		/// </summary>
		public double M01 { get; set; }

		/// <summary>
		/// Gets the first element of the second row of this GRaff.AffineMatrix.
		/// </summary>
		public double M10 { get; set; }

		/// <summary>
		/// Gets the second element of the second row of this GRaff.AffineMatrix.
		/// </summary>
		public double M11 { get; set; }

		/// <summary>
		/// Applies a scaling transformation to this GRaff.AffineMatrix.
		/// </summary>
		/// <param name="scaleX">The horizontal scale factor.</param>
		/// <param name="scaleY">The vertical scale factor.</param>
		public void Scale(double scaleX, double scaleY)
		{
			M00 *= scaleX;
			M10 *= scaleX;
			M01 *= scaleY;
			M11 *= scaleY;
		}


		/// <summary>
		/// Applies a rotation transformation to this GRaff.AffineMatrix.
		/// </summary>
		/// <param name="a">The angle to rotate by.</param>
		public void Rotate(Angle a)
		{
			double c = GMath.Cos(a), s = GMath.Sin(a);
			double xx = M00, yx = M10, xy = M01, yy = M11;
			M00 = xx * c + xy * s;
			M10 = yx * c + yy * s;
			M01 = xy * c - xx * s;
			M11 = yy * c - yx * s;
		}


		/// <summary>
		/// Specifies whether this GRaff.LinearMatrix contains the same elements as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.LinearMatrix and has the same elements as this GRaff.LinearMatrix.</returns>
		public override bool Equals(object obj)
		{
			return ((obj is LinearMatrix) ? (this == (LinearMatrix)obj) : base.Equals(obj));
		}

		/// <summary>
		/// Returns a hash code for this GRaff.LinearMatrix.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.LinearMatrix.</returns>
		public override int GetHashCode() { return GMath.HashCombine(M00.GetHashCode(), M01.GetHashCode(), M10.GetHashCode(), M11.GetHashCode()); }

		/// <summary>
		/// Creates a deep clone of this GRaff.LinearMatrix.
		/// </summary>
		/// <returns>A deep clone of this GRaff.LinearMatrix.</returns>
		public LinearMatrix Clone()
		{
			return new LinearMatrix(M00, M01, M10, M11);
		}

		/// <summary>
		/// Creates a deep clone of this GRaff.LinearMatrix.
		/// </summary>
		/// <returns>A deep clone of this GRaff.LinearMatrix.</returns>
		object ICloneable.Clone()
		{
			return Clone();
		}


		/// <summary>
		/// Compares two GRaff.LinearMatrix objects. The result specifies whether all their elements are equal.
		/// </summary>
		/// <param name="left">The first GRaff.LinearMatrix to compare.</param>
		/// <param name="right">The second GRaff.LinearMatrix to compare.</param>
		/// <returns>true if all elements of the two GRaff.LinearMatrix objects are equal.</returns>
		public static bool operator ==(LinearMatrix left, LinearMatrix right)
		{
			if (left == null && right == null)
				return true;
			else if (left == null || right == null)
				return false;
			else
				return (left.M00 == right.M00 && left.M01 == right.M01 && left.M10 == right.M10 && left.M11 == right.M11);
		}

		/// <summary>
		/// Compares two GRaff.LinearMatrix objects. The result specifies whether all their elements are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.LinearMatrix to compare.</param>
		/// <param name="right">The second GRaff.LinearMatrix to compare.</param>
		/// <returns>true if all elements of thow two GRaff.LinearMatrix objects are unequal.</returns>
		public static bool operator !=(LinearMatrix left, LinearMatrix right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Computes the element-wise sum of the two GRaff.LinearMatrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.LinearMatrix.</param>
		/// <param name="right">The second GRaff.LinearMatrix.</param>
		/// <returns>The sum of the elements of each GRaff.LinearMatrix.</returns>
		public static LinearMatrix operator +(LinearMatrix left, LinearMatrix right) { return new LinearMatrix(left.M00 + right.M00, left.M01 + right.M01, left.M10 + right.M10, left.M11 + right.M11); }

		/// <summary>
		/// Computes the element-wise difference of the two GRaff.LinearMatrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.LinearMatrix.</param>
		/// <param name="right">The second GRaff.LinearMatrix.</param>
		/// <returns>The difference of the elements of each GRaff.LinearMatrix.</returns>
		public static LinearMatrix operator -(LinearMatrix left, LinearMatrix right) { return new LinearMatrix(left.M00 - right.M00, left.M01 - right.M01, left.M10 - right.M10, left.M11 - right.M11); }

		/// <summary>
		/// Computes the matrix product of the two GRaff.LinearMatrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.LinearMatrix.</param>
		/// <param name="right">The second GRaff.LinearMatrix.</param>
		/// <returns>The matrix product of the two GRaff.LinearMatrix.</returns>
		public static LinearMatrix operator *(LinearMatrix left, LinearMatrix right)
		{
			return new LinearMatrix(
				left.M00 * right.M00 + left.M01 * right.M10, left.M00 * right.M01 + left.M01 * right.M11,
				left.M10 * right.M00 + left.M11 * right.M10, left.M10 * right.M01 + left.M11 * right.M11
			);
		}

		/// <summary>
		/// Computes the matrix product of the GRaff.LinearMatrix and the GRaff.Point.
		/// This constitutes performing the linear transformation on that GRaff.Point.
		/// </summary>
		/// <param name="m">A GRaff.LinearMatrix representing the linear transformation.</param>
		/// <param name="p">A GRaff.Point to be transformed by the linear transformation.</param>
		/// <returns>The transformed GRaff.Point.</returns>
		public static Point operator *(LinearMatrix m, Point p) { return new Point(m.M00 * p.X + m.M01 * p.Y, m.M10 * p.X + m.M11 * p.Y); }

		/// <summary>
		/// Computes the matrix product of the GRaff.LinearMatrix and the GRaff.Vector.
		/// This constitutes performing the linear transformation on that GRaff.Vector.
		/// </summary>
		/// <param name="m">A GRaff.LinearMatrix representing the linear transformation.</param>
		/// <param name="v">A GRaff.Vector to be transformed by the linear transformation.</param>
		/// <returns>The transformed GRaff.Vector.</returns>
		public static Vector operator *(LinearMatrix m, Vector v) { return new Vector(m.M00 * v.X + m.M01 * v.Y, m.M10 * v.X + m.M11 * v.Y); }

	}
}
