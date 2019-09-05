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
            new FragmentShader(ShaderHints.Header, ShaderHints.GetFragColor, SpotlightShaderSource);

        private UniformVec2 _origin, _scale;
        private UniformFloat _innerRadius, _outerRadius;
        private UniformColor _darknessColor;

       
        public SpotlightShaderProgram(double innerRadius, double outerRadius)
            : base(VertexShader.Default, _fragmentShader)
        {
            _origin = new UniformVec2(this, "origin");
			_scale = new UniformVec2(this, "scale");
            Scale = (1, 1);
            _innerRadius = new UniformFloat(this, "innerRadius", innerRadius);
            _outerRadius = new UniformFloat(this, "outerRadius", outerRadius);
            _darknessColor = new UniformColor(this, "darknessColor", Colors.Black);
        }

        public Point Origin
        {
            get => _origin.Value;
            set => _origin.Value = value;
        }

        public Vector Scale
        {
            get => _scale.Value;
            set => _scale.Value = value;
        }

        public double InnerRadius
        {
            get => _innerRadius.Value;
            set => _innerRadius.Value = value;
        }

        public double OuterRadius
        {
            get => _outerRadius.Value;
            set => _outerRadius.Value = value;
        }

        public Color DarknessColor
        {
            get => _darknessColor.Value;
            set => _darknessColor.Value = value;
        }

    }
}
