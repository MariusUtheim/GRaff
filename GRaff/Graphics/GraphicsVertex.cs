using System;
using System.Runtime.InteropServices;

namespace GRaff.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GraphicsVertex
    {
  
        public GraphicsVertex(Point point, Color color)
        {
            this.Point = (GraphicsPoint)point;
            this.Color = color;
        }

        internal static readonly int Size = Marshal.SizeOf<GraphicsVertex>();

        public GraphicsPoint Point { get; private set; }

        public Color Color { get; private set; }

        public override string ToString() => $"({Point.X}, {Point.Y}, {Color}";

    }
}
