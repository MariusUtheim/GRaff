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
        = new FragmentShader(ShaderHints.Header, ShaderHints.GetFragColor, WaveShiftSource);

        private UniformFloat _phase;
        private UniformVec2 _waveVector, _wavePolarization;

		public WaveShiftShaderProgram(Vector waveVector, Vector polarizationVector)
            : base(VertexShader.Default, WaveShiftFragmentShader)
        {
            _phase = new UniformFloat(this, "phase");
            _waveVector = new UniformVec2(this, "waveVector", waveVector);
            _wavePolarization = new UniformVec2(this, "wavePolarization", polarizationVector);
        }

        public double Phase
        {
            get => _phase.Value;
            set => _phase.Value = value;
        }

        public Vector WaveVector
        {
            get => _waveVector.Value;
            set => _waveVector.Value = value;
        }

        public Vector WavePolarization
        {
            get => _wavePolarization.Value;
            set => _wavePolarization.Value = value;
        }
    }
}
