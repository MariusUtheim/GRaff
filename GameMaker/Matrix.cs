using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public struct Matrix
	{
		public Matrix(double m00, double m01, double m02, double m10, double m11, double m12)
			: this()
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M02 = m02;
			this.M10 = m10;
			this.M11 = m11;
			this.M12 = m12;
		}

		public static Matrix Identity()
		{
			return new Matrix(1, 0, 0, 0, 1, 0);
		}

		public static Matrix Scale(double scaleX, double scaleY)
		{
			return new Matrix(scaleX, 0, 0, 0, scaleY, 0);
		}

		public static Matrix Shear(double shearX, double shearY)
		{
			return new Matrix(1, shearX, 0, shearY, 1, 0);
		}

		public static Matrix Rotate(Angle a)
		{
			double c = GMath.Cos(a), s = GMath.Sin(a);
			return new Matrix(c, -s, 0, s, c, 0);
		}

		public static Matrix Translate(double dx, double dy)
		{
			return new Matrix(0, 0, dx, 0, 0, dy);
		}

		public double M00 { get; set; }
		public double M01 { get; set; }
		public double M02 { get; set; }
		public double M10 { get; set; }
		public double M11 { get; set; }
		public double M12 { get; set; }

		public override bool Equals(object obj)
		{
			if (obj is Matrix)
				return this == (Matrix)obj;
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return M00.GetHashCode() ^ M01.GetHashCode() ^ M02.GetHashCode() ^ M10.GetHashCode() ^ M11.GetHashCode() ^ M12.GetHashCode();
		}

		public static bool operator ==(Matrix left, Matrix right)
		{
			return left.M00 == right.M00 && left.M01 == right.M01 && left.M02 == right.M02 && left.M10 == right.M10 && left.M11 == right.M11 && left.M12 == right.M12;
		}

		public static bool operator !=(Matrix left, Matrix right)
		{
			return left.M00 != right.M00 || left.M01 != right.M01 || left.M02 != right.M02 || left.M10 != right.M10 || left.M11 != right.M11 || left.M12 != right.M12;
		}

		public static Matrix operator +(Matrix left, Matrix right)
		{
			return new Matrix(left.M00 + right.M00, left.M01 + right.M01, left.M02 + right.M02, left.M10 + right.M10, left.M11 + right.M11, left.M12 + right.M12);
		}

		public static Matrix operator -(Matrix left, Matrix right)
		{
			return new Matrix(left.M00 - right.M00, left.M01 - right.M01, left.M02 - right.M02, left.M10 - right.M10, left.M11 - right.M11, left.M12 - right.M12);
		}

		public static Matrix operator *(Matrix left, Matrix right)
		{
			return new Matrix(
				left.M00 * right.M00 + left.M01 * right.M10, left.M00 * right.M01 + left.M01 * right.M11, left.M00 * right.M02 + left.M01 * right.M12 + left.M02,
				left.M10 * right.M00 + left.M11 * right.M10, left.M10 * right.M01 + left.M11 * right.M11, left.M10 * right.M02 + left.M11 * right.M12 + left.M12
				);
		}

		public static Point operator *(Matrix m, Point p)
		{
			return new Point(m.M00 * p.X + m.M01 * p.Y + m.M02, m.M10 * p.X + m.M11 * p.Y + m.M12);
		}

		public static Vector operator *(Matrix m, Vector v)
		{
			return new Vector(m.M00 * v.X + m.M01 * v.Y, m.M10 * v.X + m.M11 * v.Y);
		}
	}
}
