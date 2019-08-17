using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace GRaff
{
    [StructLayout(LayoutKind.Sequential)]
	public struct Quadrilateral : IEquatable<Quadrilateral>
	{

        public Quadrilateral(Point v1, Point v2, Point v3, Point v4)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            V4 = v4;
        }

		public Quadrilateral(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
		{
            V1 = (x1, y1);
            V2 = (x2, y2);
            V3 = (x3, y3);
            V4 = (x4, y4);
		}
        
        // Note: The vertices (V1,V2,V3,V4) refer to the vertices in a clockwise order, starting with top-left. 
        // However, the layout order is (V1,V2,V4,V3), so that the quadrilateral renders correctly as a triangle strip.
        public Point V1 { get; private set; }

        public Point V2 { get; private set; }

        public Point V4 { get; private set; }

        public Point V3 { get; private set; }

        public double X1 => V1.X;
        public double Y1 => V1.Y;
        public double X2 => V2.X;
        public double Y2 => V2.Y;
        public double X3 => V3.X;
        public double Y3 => V3.Y;
        public double X4 => V4.X;
        public double Y4 => V4.Y;

        public IEnumerable<Point> Vertices => new[] { V1, V2, V3, V4 };

        public IEnumerable<Line> Edges => new[] { new Line(V1, V2), new Line(V2, V3), new Line(V3, V4), new Line(V4, V1) };

        public bool Equals(Quadrilateral other)
            => V1 == other.V1 && V2 == other.V2 && V3 == other.V3 && V4 == other.V4;

        public static Quadrilateral operator +(Quadrilateral left, Vector right)
            => new Quadrilateral(left.V1 + right, left.V2 + right, left.V3 + right, left.V4 + right);

        public static Quadrilateral operator -(Quadrilateral left, Vector right)
            => new Quadrilateral(left.V1 - right, left.V2 - right, left.V3 - right, left.V4 + right);

        public static implicit operator Quadrilateral(Rectangle rect) 
            => new Quadrilateral(rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft);
    }
}
