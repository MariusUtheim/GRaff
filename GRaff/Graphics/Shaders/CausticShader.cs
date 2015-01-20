using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES30;

namespace GRaff.Graphics.Shaders
{
	public class CausticShader : ShaderElement
	{
		private const double DefaultIntensity = 0.35;
		private static readonly Color DefaultColor = Color.White;
		#region Source
		public const string Source =
			Shader.Header + @"
			#define TAU 6.28318530718
			#define MAX_ITER 5

			in highp vec4 pass_Color;
			uniform highp float time;
			uniform highp float xorigin;
			uniform highp float yorigin;
			uniform highp float xscale;
			uniform highp float yscale;
			uniform highp float intensity;

			void main() 
			{
				vec2 uv = (gl_FragCoord.xy - vec2(xorigin, yorigin)) / vec2(xscale, yscale) ;
   
				vec2 p = mod(uv*TAU, TAU)-250.0;
	
				vec2 i = vec2(p);
				float c = 1.0;
				float inten = .005;

				for (int n = 0; n < MAX_ITER; n++) 
				{
					float t = time * 0.007 * (1.0 - (3.5 / float(n+1)));
					i = p + vec2(cos(t - i.x) + sin(t + i.y), sin(t - i.y) + cos(t + i.x));
					c += 1.0/length(vec2(p.x / (sin(i.x+t)/inten),p.y / (cos(i.y+t)/inten)));
				}
				c /= float(MAX_ITER);
				c = 1.17-pow(c, 1.4);
				vec3 colour = vec3(pow(abs(c), 8.0));
				gl_FragColor = vec4(clamp(colour + pass_Color.rgb, 0.0, 1.0), pass_Color.a);
			}";
		#endregion

		private double _intensity;
		private double _time;

		public CausticShader(Rectangle region)
			: this(region, DefaultIntensity, DefaultColor)
		{ }

		public CausticShader(Rectangle region, Color color)
			: this(region, DefaultIntensity, color)
		{ }

		public CausticShader(Rectangle region, double intensity)
			: this(region, intensity, DefaultColor)
		{ }

		public CausticShader(Rectangle region, double intensity, Color color)
			: this(region, intensity, color, color, color, color)
		{ }

		public CausticShader(Rectangle region, Color c1, Color c2, Color c3, Color c4)
			: this(region, DefaultIntensity, c1, c2, c3, c4)
		{ }

		public CausticShader(Rectangle region, double intensity, Color c1, Color c2, Color c3, Color c4)
			: base(Source)
		{
			Intensity = intensity;
			SetPrimitive(PrimitiveType.Quads, region.QuadCoordinates());
			SetColors(c1, c2, c3, c4);

			AutomaticUniform("time", () => Time.LoopCount / 2.0);

			SetUniform("xorigin", region.Left);
			SetUniform("yorigin", region.Top);
			SetUniform("xscale", region.Width);
			SetUniform("yscale", region.Height);
		}

		public double Intensity
		{
			get { return _intensity; }
			set
			{
				_intensity = value;
				SetUniform("intensity", _intensity);
			}
		}

		public void SetColor(Color color)
		{
			SetColors(color, color, color, color);
		}

		public void SetColor(Color c1, Color c2, Color c3, Color c4)
		{
			SetColors(c1, c2, c3, c4);
		}
	}
}
