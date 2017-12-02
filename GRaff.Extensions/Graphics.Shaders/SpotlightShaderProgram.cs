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
uniform vec4 darknessColor;

void main() {
	vec2 uv = (gl_FragCoord.xy - origin) / scale;
    double d;
    if (innerRadius < outerRadius)
        d = clamp(0.0, (length(uv) - innerRadius) / (outerRadius - innerRadius), 1.0);
    else if (innerRadius == outerRadius)
        d = length(uv) < innerRadius ? 0.0 : 1.0;
    else
        d = clamp(0.0, (innerRadius - length(uv)) / (innerRadius - outerRadius), 1.0);
    d = d * darknessColor.a;
    vec4 c = GRaff_GetFragColor();
    out_FragColor = vec4((1 - d) * c.rgb + d * darknessColor.rgb, 1);
}";
        #endregion


        private static readonly FragmentShader _fragmentShader =
            new FragmentShader(
                Shader.GRaff_Header,
                FragmentShader.GRaff_GetFragColor,
                SpotlightShaderSource
            );

        private ShaderUniformLocation _origin, _scale, _innerRadius, _outerRadius, _darknessColor;

        public SpotlightShaderProgram(double innerRadius, double outerRadius)
            : base(VertexShader.Default, _fragmentShader)
        {
			_origin = UniformLocation("origin");
			_scale = UniformLocation("scale");
            Scale = (1, 1);
            _innerRadius = UniformLocation("innerRadius");
            InnerRadius = innerRadius;
            _outerRadius = UniformLocation("outerRadius");
            OuterRadius = outerRadius;
            _darknessColor = UniformLocation("darknessColor");
            DarknessColor = Colors.Black;
        }

        public Point Origin
        {
            get => GetUniformVec2(_origin);
            set => SetUniformVec2(_origin, value);
        }

        public Vector Scale
        {
            get => GetUniformVec2(_scale);
            set => SetUniformVec2(_scale, value);
        }

        public double InnerRadius
        {
            get => GetUniformFloat(_innerRadius);
            set => SetUniformFloat(_innerRadius, (float)value);
        }

        public double OuterRadius
        {
            get => GetUniformFloat(_outerRadius);
            set => SetUniformFloat(_outerRadius, (float)value);
        }

        public Color DarknessColor
        {
            get => GetUniformColor(_darknessColor);
            set => SetUniformColor(_darknessColor, value);
        }

    }
}
