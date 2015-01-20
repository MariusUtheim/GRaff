using System;


namespace GRaff
{
	/// <summary>
	/// Represents an affine transformation matrix.
	/// </summary>
	public sealed class AffineMatrix : IEquatable<AffineMatrix>
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

		public double M00 { get; set; }
		public double M01 { get; set; }
		public double M02 { get; set; }
		public double M10 { get; set; }
		public double M11 { get; set; }
		public double M12 { get; set; }

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

#warning More documentation
		public override bool Equals(object obj) { return Equals(obj as AffineMatrix); }

		public bool Equals(AffineMatrix other)
		{
			if (other == null) return false;
			return (M00 == other.M00 && M01 == other.M01 && M02 == other.M02 && M10 == other.M10 && M11 == other.M11 && M12 == other.M12);
		}

		public override int GetHashCode() { return M00.GetHashCode() ^ M01.GetHashCode() ^ M02.GetHashCode() ^ M10.GetHashCode() ^ M11.GetHashCode() ^ M12.GetHashCode(); }


		public static bool operator ==(AffineMatrix left, AffineMatrix right)   /*C#6.0*/ // left?.Equals(right) ?? right == null;
		{
			if (left == null)
				return right == null;
			else
				return left.Equals(right);
		}


		public static bool operator !=(AffineMatrix left, AffineMatrix right)
		{
			return !(left == right);
		}

		public static AffineMatrix operator +(AffineMatrix left, AffineMatrix right) { return new AffineMatrix(left.M00 + right.M00, left.M01 + right.M01, left.M02 + right.M02, left.M10 + right.M10, left.M11 + right.M11, left.M12 + right.M12); }


		public static AffineMatrix operator -(AffineMatrix left, AffineMatrix right) { return new AffineMatrix(left.M00 - right.M00, left.M01 - right.M01, left.M02 - right.M02, left.M10 - right.M10, left.M11 - right.M11, left.M12 - right.M12); }


		public static AffineMatrix operator *(AffineMatrix left, AffineMatrix right)
		{
			return new AffineMatrix(
				left.M00 * right.M00 + left.M01 * right.M10, left.M00 * right.M01 + left.M01 * right.M11, left.M00 * right.M02 + left.M01 * right.M12 + left.M02,
				left.M10 * right.M00 + left.M11 * right.M10, left.M10 * right.M01 + left.M11 * right.M11, left.M10 * right.M02 + left.M11 * right.M12 + left.M12
			);
		}


		public static Point operator *(AffineMatrix m, Point p) { return new Point(m.M00 * p.X + m.M01 * p.Y + m.M02, m.M10 * p.X + m.M11 * p.Y + m.M12); }

		public static Point operator *(Point p, AffineMatrix m) { return new Point(m.M00 * p.X + m.M01 * p.Y + m.M02, m.M10 * p.X + m.M11 * p.Y + m.M12); }

		public static Vector operator *(AffineMatrix m, Vector v) { return new Vector(m.M00 * v.X + m.M01 * v.Y, m.M10 * v.X + m.M11 * v.Y); }
	}
}
