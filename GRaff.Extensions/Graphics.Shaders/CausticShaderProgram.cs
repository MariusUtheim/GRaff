using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics.Shaders
{
    public class CausticShaderProgram : ShaderProgram
	{
        #region Source
        public static string CausticShaderSource { get; } =
@"
#define TAU 6.28318530718
#define MAX_ITER 5
out vec4 out_FragColor;

uniform highp float phase;
uniform highp vec2 offset;
uniform highp vec2 scale;
uniform highp float intensity;

void main() {
	vec2 uv = (gl_FragCoord.xy - offset) / scale;
    
    vec2 p = mod(uv*TAU, TAU)-250.0;
    
	vec2 i = vec2(p);
	float c = 1.0;
    float inten = intensity / 200;                    

	for (int n = 0; n < MAX_ITER; n++) 
	{
		float t = phase * 0.007 * (1.0 - (3.5 / float(n+1)));
		i = p + vec2(cos(t - i.x) + sin(t + i.y), sin(t - i.y) + cos(t + i.x));
		c += 1.0/length(vec2(p.x / (sin(i.x+t)/inten),p.y / (cos(i.y+t)/inten)));
	}
	c /= float(MAX_ITER);
	c = 1.17-pow(c, 1.4);
	vec3 color = vec3(pow(abs(c), 8.0));

    vec4 inputColor = GRaff_GetFragColor();
	out_FragColor = vec4(clamp(color * inputColor.rgb, 0.0, 1.0), inputColor.a);
}";
        #endregion

        private static FragmentShader _causticFragmentShader =
            new FragmentShader(
                ShaderHints.Header,
                ShaderHints.GetFragColor,
                CausticShaderSource
            );

        private ShaderUniformLocation _phase, _offset, _scale, _intensity;
        
		public CausticShaderProgram(Rectangle region, double intensity = 1)
			: base(VertexShader.Default, _causticFragmentShader)
		{
            _phase = UniformLocation("phase");
            _offset = UniformLocation("offset");
            _scale = UniformLocation("scale");
            _intensity = UniformLocation("intensity");

            this.Intensity = intensity;
            this.Region = region;
            _Graphics.ErrorCheck();
		}

        public double Phase
        {
            get => GetUniformFloat(_phase);
            set => SetUniformFloat(_phase, (float)value);
        }

        public double Intensity
        {
            get => GetUniformFloat(_intensity);
            set => SetUniformFloat(_intensity, (float)value);
        }


        public Point Offset
        {
            get => GetUniformVec2(_offset);
            set => SetUniformVec2(_offset, value);
        }

        public Vector Scale
        {
            get => GetUniformVec2(_scale);
            set => SetUniformVec2(_scale, value);
        }
        
        public Rectangle Region
        {
            get => new Rectangle(Offset, Scale);
            set => (Offset, Scale) = (value.TopLeft, value.Size);
        }
    }
}
