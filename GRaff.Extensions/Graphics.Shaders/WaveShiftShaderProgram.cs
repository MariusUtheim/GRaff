using System;
namespace GRaff.Graphics.Shaders
{
    public class WaveShiftShaderProgram : ShaderProgram
    {

        public static string WaveShiftSource { get; } =
@"
out vec4 out_FragColor;

uniform float phase;
uniform vec2 waveVector;
uniform vec2 wavePolarization;
                
vec2 map(vec2 p) {
    float amount = dot(p, waveVector);
    return p + wavePolarization * sin(amount + phase);
}

void main(void) {
    vec4 c = GRaff_GetFragColor(map(GRaff_TexCoord));
    out_FragColor = vec4(c.rgb, c.a);
}
";

        public static FragmentShader WaveShiftFragmentShader { get; }
        = new FragmentShader(Shader.GRaff_Header, FragmentShader.GRaff_GetFragColor, WaveShiftSource);

        private ShaderUniformLocation _phase, _waveVector, _wavePolarization;

		public WaveShiftShaderProgram(Vector waveVector, Vector polarizationVector)
            : base(VertexShader.Default, WaveShiftFragmentShader)
        {
            _phase = UniformLocation("phase");
            _waveVector = UniformLocation("waveVector");
            this.WaveVector = waveVector;
            _wavePolarization = UniformLocation("wavePolarization");
            this.WavePolarization = polarizationVector;
        }

        public double Phase
        {
            get => GetUniformFloat(_phase);
            set => SetUniformFloat(_phase, (float)value);
        }

        public Vector WaveVector
        {
            get => GetUniformVec2(_waveVector);
            set => SetUniformVec2(_waveVector, value);
        }

        public Vector WavePolarization
        {
            get => GetUniformVec2(_wavePolarization);
            set => SetUniformVec2(_wavePolarization, value);
        }
    }
}
