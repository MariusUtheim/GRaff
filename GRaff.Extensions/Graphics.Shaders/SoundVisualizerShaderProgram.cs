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
uniform highp float maxDist;
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

        private ShaderUniformLocation _origin, _scale, _orientation, _maxDist, _offset;
        private int _buffer, _texture;


        private static FragmentShader _fragShader(int dataLength)
            => new FragmentShader(
                Shader.GRaff_Header,
                FragmentShader.GRaff_GetFragColor,
            SimplifiedSource);


        public SoundVisualizerShaderProgram(byte[] data, Vector scale, double maxDistance)
            : base(VertexShader.Default, _fragShader(data.Length))
        {
            _origin = UniformLocation("origin");
            _scale = UniformLocation("scale");
            this.Scale = scale;
            _orientation = UniformLocation("orientation");
            _maxDist = UniformLocation("maxDist");
            this.MaxDistance = maxDistance;
            _offset = UniformLocation("offset");


            GL.ProgramUniform1(Id, GL.GetUniformLocation(Id, "data"), 1);
			GL.ActiveTexture(TextureUnit.Texture1);

			this._buffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.TextureBuffer, _buffer);
			GL.BufferData(BufferTarget.TextureBuffer, new IntPtr(data.Length), data, BufferUsageHint.StaticRead);

			this._texture = GL.GenTexture();
			GL.BindTexture(TextureTarget.TextureBuffer, _texture);
            GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.Rg8, _buffer);

            GL.ActiveTexture(TextureUnit.Texture0);

			_Graphics.ErrorCheck();
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

        public Angle Orientation
        {
            get => Angle.Rad(GetUniformFloat(_orientation));
            set => SetUniformFloat(_orientation, (float)value.Radians);
        }

        public double MaxDistance
        {
            get => GetUniformFloat(_maxDist);
            set => SetUniformFloat(_maxDist, (float)value);
        }

        public int Offset
        {
            get => GetUniformInt(_offset);
            set => SetUniformInt(_offset, value);
        }
    }
}
