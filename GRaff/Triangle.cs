using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public struct Triangle : IEquatable<Triangle>
	{

		public Triangle(Point v1, Point v2, Point v3)
			: this()
		{
			V1 = v1;
			V2 = v2;
			V3 = v3;
		}

		public Triangle(double x1, double y1, double x2, double y2, double x3, double y3)
			: this()
		{
			this.V1 = new Point(x1, y1);
			this.V2 = new Point(x2, y2);
			this.V3 = new Point(x3, y3);
		}

		public Point V1 { get; private set; }

		public Point V2 { get; private set; }

		public Point V3 { get; private set; }

		public double X1 => V1.X;
		public double Y1 => V1.Y;
		public double X2 => V2.X;
		public double Y2 => V2.Y;
		public double X3 => V3.X;
		public double Y3 => V3.Y;


		public bool Equals(Triangle other)
			=> V1 == other.V1 && V2 == other.V2 && V3 == other.V3;


		public static Triangle operator +(Triangle left, Vector right)
			=> new Triangle(left.V1 + right, left.V2 + right, left.V3 + right);


		public static Triangle operator -(Triangle left, Vector right)
			=> new Triangle(left.V1 - right, left.V2 - right, left.V3 - right);


		public static Triangle operator *(LinearMatrix left, Triangle right)
		{
			Contract.Requires<ArgumentNullException>(left != null);
			return new Triangle(left * right.V1, left * right.V2, left * right.V3);
		}


		public static Triangle operator *(AffineMatrix left, Triangle right)
		{
			Contract.Requires<ArgumentNullException>(left != null);
			return new Triangle(left* right.V1, left* right.V2, left* right.V3);
		}
		
	}
}
