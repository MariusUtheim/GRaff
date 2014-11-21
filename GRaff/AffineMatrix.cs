


namespace GRaff
{
	/// <summary>
	/// Represents an affine transformation matrix.
	/// </summary>
	public sealed class AffineMatrix
	{
		public AffineMatrix()
			: this(1, 0, 0, 0, 1, 0)
		{ }

		public AffineMatrix(double m00, double m01, double m02, double m10, double m11, double m12)
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M02 = m02;
			this.M10 = m10;
			this.M11 = m11;
			this.M12 = m12;
		}

		public static AffineMatrix Scaling(double scaleX, double scaleY) { return new AffineMatrix(scaleX, 0, 0, 0, scaleY, 0); }

		public static AffineMatrix Shearing(double shearX, double shearY) { return new AffineMatrix(1, shearX, 0, shearY, 1, 0);	}

		public static AffineMatrix Rotation(Angle a) { return new AffineMatrix(GMath.Cos(a), -GMath.Sin(a), 0, GMath.Sin(a), GMath.Cos(a), 0); }

		public static AffineMatrix Translation(double dx, double dy) { return new AffineMatrix(1, 0, dx, 0, 1, dy); }

		public double M00 { get; set; }
		public double M01 { get; set; }
		public double M02 { get; set; }
		public double M10 { get; set; }
		public double M11 { get; set; }
		public double M12 { get; set; }

		public void Scale(double scaleX, double scaleY)
		{
			M00 *= scaleX;
			M10 *= scaleX;
			M01 *= scaleY;
			M11 *= scaleY;
		}

		public void Rotate(Angle a)
		{
			double c = GMath.Cos(a), s = GMath.Sin(a);
			double xx = M00, yx = M10, xy = M01, yy = M11;
			M00 = xx * c + xy * s;
			M10 = yx * c + yy * s;
			M01 = xy * c - xx * s;
			M11 = yy * c - yx * s;
		}

		public override bool Equals(object obj) { return (obj is AffineMatrix) ? (this == (AffineMatrix)obj) : false; }

		public override int GetHashCode() { return M00.GetHashCode() ^ M01.GetHashCode() ^ M02.GetHashCode() ^ M10.GetHashCode() ^ M11.GetHashCode() ^ M12.GetHashCode(); }


		public static bool operator ==(AffineMatrix left, AffineMatrix right)
		{
			if (left == null && right == null)
				return true;
			else if (left == null || right == null)
				return false;
			else
				return (left.M00 == right.M00 && left.M01 == right.M01 && left.M02 == right.M02 && left.M10 == right.M10 && left.M11 == right.M11 && left.M12 == right.M12);
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
