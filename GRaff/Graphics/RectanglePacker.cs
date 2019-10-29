using System;
using System.Collections.Generic;
using System.Linq;

namespace GRaff.Graphics
{
    internal static class RectanglePacker
    {
        
        public static IntRectangle[] Pack(IEnumerable<IntVector> rects, out IntVector bounds)
        {
            var result = new IntRectangle[rects.Count()];
            var x = 0;
            var maxH = 0;
            var i = 0;

            foreach (var sz in rects)
            {
                result[i++] = new IntRectangle(x, 0, sz.X, sz.Y);
                x += sz.X;
                maxH = GMath.Max(maxH, sz.Y);
            }

            bounds = new IntVector(x, maxH);
            return result;
        }


    }
}
