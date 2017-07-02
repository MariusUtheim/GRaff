using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics.Shaders
{
    public class ColorMatrixShaderProgram : ShaderProgram
    {

        public static string ColorMatrixShaderSource { get; } =
@"
out vec4 out_FragColor;
                
uniform mat3 GRaff_ColorMatrix;

void main(void) {
    vec4 c = GRaff_GetFragColor();
    out_FragColor = vec4(GRaff_ColorMatrix * c.rgb, c.a);
}
";

		public static FragmentShader ColorMatrixFragmentShader { get; }
		    = new FragmentShader(Shader.GRaff_Header, FragmentShader.GRaff_GetFragColor, ColorMatrixShaderSource);

		private int _colorMatrixLoc;

        public ColorMatrixShaderProgram(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22)
            : base(VertexShader.Default, ColorMatrixFragmentShader)
        {
            _colorMatrixLoc = GL.GetUniformLocation(Id, "GRaff_ColorMatrix");
            SetMatrix(new[] { m00, m01, m02, m10, m11, m12, m20, m21, m22 });
        }

        public void SetMatrix(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22)
            => SetMatrix((float)m00, (float)m01, (float)m02, (float)m10, (float)m11, (float)m12, (float)m20, (float)m21, (float)m22);

        public void SetMatrix(double[] matrix)
            => SetMatrix(matrix.Select(d => (float)d).ToArray());

        public void SetMatrix(float[] matrix)
        {
            Contract.Requires<ArgumentNullException>(matrix != null);
            Contract.Requires<ArgumentException>(matrix.Length == 9);
            GL.ProgramUniformMatrix3(Id, _colorMatrixLoc, 1, false, matrix);
        }

    }
}
