#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics
{
    public class FragmentShader : Shader
    {
         public FragmentShader(string source)
            : base(ShaderType.FragmentShader, source) { }


        public static FragmentShader Default { get; }
            = new FragmentShader(
                Header + @"
			    in lowp vec4 pass_Color;
			    in highp vec2 pass_TexCoord;
                out highp vec4 out_FragColor;
			    uniform highp sampler2D tex;
                uniform bool GRaff_IsTextured;

			    void main(void) {
                    if (GRaff_IsTextured)
    		    		out_FragColor = texture(tex, pass_TexCoord).rgba * pass_Color;
                    else
                        out_FragColor = pass_Color;
			    }");

        public static FragmentShader BlackWhite { get; }
            = new FragmentShader(
                Header + @"
			    in lowp vec4 pass_Color;
			    in highp vec2 pass_TexCoord;
                out highp vec4 out_FragColor;
			    uniform highp sampler2D tex;
                uniform bool GRaff_IsTextured;
        
			    void main(void) {
                    vec4 c;
                    if (GRaff_IsTextured)
    		    		c = texture(tex, pass_TexCoord).rgba * pass_Color;
                    else
                        c = pass_Color;
                    double intensity = (c.r + c.g + c.b) / 3.0;
                    out_FragColor = vec4(intensity, intensity, intensity, c.a);
			    }");

        public static FragmentShader Sepia { get; }
            = new FragmentShader(
                Header + @"
			    in lowp vec4 pass_Color;
			    in highp vec2 pass_TexCoord;
                out highp vec4 out_FragColor;
			    uniform highp sampler2D tex;
                uniform bool GRaff_IsTextured;

                const mat3 sepiaMatrix = mat3(0.393, 0.349, 0.272, 0.769, 0.686, 0.534, 0.189, 0.168, 0.131);        

			    void main(void) {
                    vec4 c;
                    if (GRaff_IsTextured)
    		    		c = texture(tex, pass_TexCoord).rgba * pass_Color;
                    else
                        c = pass_Color;
                    
                    out_FragColor = vec4(sepiaMatrix * c.rgb, c.a);
			    }");

    }
}