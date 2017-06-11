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
layout(origin_upper_left) in vec4 gl_FragCoord;
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
                Shader.GRaff_Header,
                FragmentShader.GRaff_GetFragColor,
                CausticShaderSource
            );

        private int _phaseLoc, _offsetLoc, _scaleLoc, _intensityLoc;
        
		public CausticShaderProgram(Rectangle region, double intensity = 1)
			: base(VertexShader.Default, _causticFragmentShader)
		{
            _phaseLoc = GL.GetUniformLocation(Id, "phase");
            _offsetLoc = GL.GetUniformLocation(Id, "offset");
            _scaleLoc = GL.GetUniformLocation(Id, "scale");
            _intensityLoc = GL.GetUniformLocation(Id, "intensity");

            this.Intensity = intensity;
            this.Region = region;
            _Graphics.ErrorCheck();
		}

        public double Phase
        {
            get
            {
                GL.GetUniform(Id, _phaseLoc, out float value);
                return value;
            }
            set
            {
                GL.ProgramUniform1(Id, _phaseLoc, (float)value);
            }
        }

        public double Intensity
        {
            get
            {
                GL.GetUniform(Id, _intensityLoc, out float value);
                return value;
            }
            set
            {
                GL.ProgramUniform1(Id, _intensityLoc, (float)value);
            }
        }


        public Point Offset
        {
            get
            {
                var coords = new double[2];
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
        
        public Rectangle Region
        {
            get => new Rectangle(Offset, Scale);
            set => (Offset, Scale) = (value.TopLeft, value.Size);
        }
    }
}
