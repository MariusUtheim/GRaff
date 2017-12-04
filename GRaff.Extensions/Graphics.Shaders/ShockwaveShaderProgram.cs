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
uniform vec2 scale;

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

        private ShaderUniformLocation _amplitude, _radius, _width, _origin, _scale;

		public ShockwaveShaderProgram(double width, Vector scale)
            : base(VertexShader.Default, ShockwaveFragmentShader)
        {
            _amplitude = UniformLocation("amplitude");
            this.Amplitude = 0.01;
            _radius = UniformLocation("radius");
            _width = UniformLocation("width");
            this.Width = width;
            _origin = UniformLocation("origin");
            _scale = UniformLocation("scale");
            this.Scale = scale;
        }

        public double Amplitude
        {
            get => GetUniformFloat(_amplitude);
            set => SetUniformFloat(_amplitude, (float)value);
        }

        public double Radius
        {
            get => GetUniformFloat(_radius);
            set => SetUniformFloat(_radius, (float)value);
        }

        public double Width
        {
            get => GetUniformFloat(_width);
            set => SetUniformFloat(_width, (float)value);
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
    }
}
