using System;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics.Shaders
{
    public class SoundVisualizerShaderProgram : ShaderProgram
    {

        public static string SimplifiedSource { get; } =
@"
layout(origin_upper_left) in vec4 gl_FragCoord;
out vec4 out_FragColor;

uniform highp vec2 origin;
uniform highp vec2 scale;
uniform highp float orientation;
uniform int offset;

uniform highp samplerBuffer data;

float getData(int index)
{
    vec4 lh = texelFetch(data, index);
    int low = int(255 * lh.r); 
    int high = int(255 * lh.g);

    if (high > 127)
        return -float(((255 - high) << 8) | low) / 32768.0f;
    else
        return float((high << 8) | low) / 32767.0f;
}

float f() 
{ 
    float d = (gl_FragCoord.x - origin.x) * cos(orientation) / scale.x + (gl_FragCoord.y - origin.y) * sin(orientation) / scale.y;
    return getData(int(d * 1000) + offset);
}

float sqr(float x) { return x * x; }

void main(void) {
    vec4 c = GRaff_GetFragColor();
    float y = (gl_FragCoord.y - origin.y) * cos(orientation) / scale.y - (gl_FragCoord.x - origin.x) * sin(orientation) / scale.x;
    float target = f();

    float amount = exp(-sqr((y - target) * 10));

    out_FragColor = vec4(amount * c.rgb, c.a);

}
";

        private UniformFloat _orientation;
        private UniformVec2 _origin, _scale;
        private UniformInt32 _offset;
        private UniformTexture _data;

        private SamplerBuffer _dataBuffer;


        private static FragmentShader _fragShader(int dataLength)
            => new FragmentShader(ShaderHints.Header, ShaderHints.GetFragColor, SimplifiedSource);


        public SoundVisualizerShaderProgram(byte[] data, Vector scale)
            : base(VertexShader.Default, _fragShader(data.Length))
        {
            _origin = new UniformVec2(this, "origin");
            _scale = new UniformVec2(this, "scale", scale);
            _orientation = new UniformFloat(this, "orientation");
            _offset = new UniformInt32(this, "offset");
            _data = new UniformTexture(this, "data", 1);

            _dataBuffer = new SamplerBuffer(data);
            _dataBuffer.BindToLocation(1);
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

        public Angle Orientation
        {
            get => Angle.Rad(_orientation.Value);
            set => _orientation.Value = (float)value.Radians;
        }

        public int Offset
        {
            get => _offset.Value;
            set => _offset.Value = value;
        }
    }
}
