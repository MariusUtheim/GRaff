using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GRaff.Graphics.Shaders
{
    public sealed class ColorMatrix
    {
        internal Matrix3 Matrix;
        internal Vector3 Vec;

        public ColorMatrix()
        {
            Matrix = Identity.Matrix;
        }

        public ColorMatrix(double crr, double crg, double crb, double cr1, double cgr, double cgg, double cgb, double cg1, double cbr, double cbg, double cbb, double cb1)
        {
            Matrix = new Matrix3((float)crr, (float)crg, (float)crb, (float)cgr, (float)cgg, (float)cgb, (float)cbr, (float)cbg, (float)cbb);
            Vec = new Vector3((float)cr1, (float)cg1, (float)cb1);
        }

        public static ColorMatrix Identity { get; } = new ColorMatrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0);

        public override bool Equals(object obj)
        {
            var m = obj as ColorMatrix;
            if (m == null)
                return false;
            else
                return Matrix.Equals(m.Matrix) && Vec.Equals(m.Vec);
        }

        public override int GetHashCode()
        {
            return GMath.HashCombine(Matrix.GetHashCode(), Vec.GetHashCode());
        }
    }
}
