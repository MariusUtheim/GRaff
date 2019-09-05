using System;
namespace GRaff.Graphics.Shaders
{
    public class ShockwaveShaderProgram : ShaderProgram
    {

        public static string ShockwaveSource { get; } =
@"
layout(origin_upper_left) in vec4 gl_FragCoord;
out vec4 out_FragColor;

uniform float amplitude;
uniform float radius;
uniform float width;
uniform vec2 origin;

float sqr(float x) { return x * x; }

vec2 map(vec2 p) {
    vec2 uv = (gl_FragCoord.xy - origin);
    float r0 = length(uv);

    float amount = amplitude * exp(-sqr((radius - r0) / width));

    return p + amount * normalize(vec2(-uv.x, uv.y));
}

void main(void) {
    vec4 c = GRaff_GetFragColor(map(GRaff_TexCoord));
    out_FragColor = vec4(c.rgb, c.a);
}
";

        public static FragmentShader ShockwaveFragmentShader { get; }
        = new FragmentShader(ShaderHints.Header, ShaderHints.GetFragColor, ShockwaveSource);

        private UniformFloat _amplitude, _radius, _width;
        private UniformVec2 _origin;


		public ShockwaveShaderProgram(double width)
            : base(VertexShader.Default, ShockwaveFragmentShader)
        {
            _amplitude = new UniformFloat(this, "amplitude", 0.01);
            _radius = new UniformFloat(this, "radius");
            _width = new UniformFloat(this, "width", width);
            _origin = new UniformVec2(this, "origin");
        }

        public double Amplitude
        {
            get => _amplitude.Value;
            set => _amplitude.Value = value;
        }

        public double Radius
        {
            get => _radius.Value;
            set => _radius.Value = value;
        }

        public double Width
        {
            get => _width.Value;
            set => _width.Value = value;
        }

        public Point Origin
        {
            get => _origin.Value;
            set => _origin.Value = value;
        }
    }
}
