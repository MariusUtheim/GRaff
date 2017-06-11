using GRaff.Graphics;
using OpenTK.Graphics.OpenGL4;


namespace GRaff.Graphics.Shaders
{
    public class SpotlightShaderProgram : ShaderProgram
    {
        #region Source
        public static string SpotlightShaderSource { get; } =
@"
layout(origin_upper_left) in vec4 gl_FragCoord;
out vec4 out_FragColor;
uniform highp vec2 origin;
uniform highp vec2 scale;
uniform highp float innerRadius;
uniform highp float outerRadius;
void main() {
	vec2 uv = (gl_FragCoord.xy - origin);
    double d = clamp((length(uv) - innerRadius) / (outerRadius - innerRadius), 0.0, 1.0);
    vec4 c = GRaff_GetFragColor();
    out_FragColor = vec4((1 - d) * c.rgb, 1);
}";
        #endregion


        private static readonly FragmentShader _fragmentShader =
            new FragmentShader(
                FragmentShader.GRaff_Header,
                FragmentShader.GRaff_GetFragColor,
                SpotlightShaderSource
            );

        private int _offsetLoc, _scaleLoc, _innerRadiusLoc, _outerRadiusLoc;

        public SpotlightShaderProgram(double innerRadius, double outerRadius)
            : base(VertexShader.Default, _fragmentShader)
        {
            _offsetLoc = GL.GetUniformLocation(Id, "origin");
            _scaleLoc = GL.GetUniformLocation(Id, "scale");
            _innerRadiusLoc = GL.GetUniformLocation(Id, "innerRadius");
            InnerRadius = innerRadius;
            _outerRadiusLoc = GL.GetUniformLocation(Id, "outerRadius");
            OuterRadius = outerRadius;
        }

        public Point Origin
        {
            get
            {
                var coords = new float[2];
                GL.GetUniform(Id, _offsetLoc, coords);
                return new Point(coords[0], coords[1]);
            }
            set
            {
                GL.ProgramUniform2(Id, _offsetLoc, (float)value.X, (float)value.Y);
            }
        }

        public Vector Scale
        {
            get
            {
                var coords = new double[2];
                GL.GetUniform(Id, _scaleLoc, coords);
                return new Vector(coords[0], coords[1]);
            }
            set
            {
                GL.ProgramUniform2(Id, _scaleLoc, (float)value.X, (float)value.Y);
            }
        }

        public double InnerRadius
        {
            get
            {
                GL.GetUniform(Id, _innerRadiusLoc, out float value);
                return value;
            }
            set
            {
                GL.ProgramUniform1(Id, _innerRadiusLoc, (float)value);
            }
        }

        public double OuterRadius
        {
            get
            {
                GL.GetUniform(Id, _outerRadiusLoc, out float value);
                return value;
            }
            set
            {
                GL.ProgramUniform1(Id, _outerRadiusLoc, (float)value);
            }
        }

    }
}
