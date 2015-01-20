using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public sealed class LinearMatrix
	{
		public LinearMatrix()
			: this(1, 0, 0, 1)
		{ }

		public LinearMatrix(double m00, double m01, double m10, double m11)
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M10 = m10;
			this.M11 = m11;
		}

		public static LinearMatrix Scaling(double scaleX, double scaleY) { return new LinearMatrix(scaleX, 0, 0, scaleY); }

		public static LinearMatrix Shearing(double shearX, double shearY) { return new LinearMatrix(1, shearX, shearY, 1); }

		public static LinearMatrix Rotation(Angle a) { return new LinearMatrix(GMath.Cos(a), -GMath.Sin(a), GMath.Sin(a), GMath.Cos(a)); }


		public double M00 { get; set; }
		public double M01 { get; set; }
		public double M10 { get; set; }
		public double M11 { get; set; }

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

		public override bool Equals(object obj) { return (obj is LinearMatrix) ? (this == (LinearMatrix)obj) : false; }

		public override int GetHashCode() { return M00.GetHashCode() ^ M01.GetHashCode() ^ M10.GetHashCode() ^ M11.GetHashCode(); }

		public static bool operator ==(LinearMatrix left, LinearMatrix right)
		{
			if (left == null && right == null)
				return true;
			else if (left == null || right == null)
				return false;
			else
				return (left.M00 == right.M00 && left.M01 == right.M01 && left.M10 == right.M10 && left.M11 == right.M11);
		}


		public static bool operator !=(LinearMatrix left, LinearMatrix right)
		{
			return !(left == right);
		}

		public static LinearMatrix operator +(LinearMatrix left, LinearMatrix right) { return new LinearMatrix(left.M00 + right.M00, left.M01 + right.M01, left.M10 + right.M10, left.M11 + right.M11); }


		public static LinearMatrix operator -(LinearMatrix left, LinearMatrix right) { return new LinearMatrix(left.M00 - right.M00, left.M01 - right.M01, left.M10 - right.M10, left.M11 - right.M11); }


		public static LinearMatrix operator *(LinearMatrix left, LinearMatrix right)
		{
			return new LinearMatrix(
				left.M00 * right.M00 + left.M01 * right.M10, left.M00 * right.M01 + left.M01 * right.M11,
				left.M10 * right.M00 + left.M11 * right.M10, left.M10 * right.M01 + left.M11 * right.M11
			);
		}

		public static Point operator *(LinearMatrix m, Point p) { return new Point(m.M00 * p.X + m.M01 * p.Y, m.M10 * p.X + m.M11 * p.Y); }

		public static Point operator *(Point p, LinearMatrix m) { return new Point(m.M00 * p.X + m.M01 * p.Y, m.M10 * p.X + m.M11 * p.Y); }


		public static Vector operator *(LinearMatrix m, Vector v) { return new Vector(m.M00 * v.X + m.M01 * v.Y, m.M10 * v.X + m.M11 * v.Y); }

	}
}
