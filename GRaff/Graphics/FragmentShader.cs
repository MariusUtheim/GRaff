#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics
{
    public class FragmentShader : Shader
    {
        public FragmentShader(params string[] source)
            : base(ShaderType.FragmentShader, source) { }

        public static string GRaff_GetFragColor { get; } =
@"
in highp vec4 GRaff_Color;
in highp vec2 GRaff_TexCoord;
uniform highp sampler2D GRaff_Texture;
uniform bool GRaff_IsTextured;
vec4 GRaff_GetFragColor(void) {
    if (GRaff_IsTextured)
        return texture(GRaff_Texture, GRaff_TexCoord).rgba * GRaff_Color;
    else
        return GRaff_Color;
}";


        public static FragmentShader Default { get; }
            = new FragmentShader(
                Header + @"
                out highp vec4 out_FragColor;

                vec4 GRaff_GetFragColor(void);

			    void main() {
                    out_FragColor = GRaff_GetFragColor();
			    }
                ", GRaff_GetFragColor);

        public static FragmentShader BlackWhite { get; }
            = new FragmentShader(
                Header + @"
                out highp vec4 out_FragColor;
                
                vec4 GRaff_GetFragColor(void);

			    void main(void) {
                    vec4 c = GRaff_GetFragColor();
                    double intensity = (c.r + c.g + c.b) / 3.0;
                    out_FragColor = vec4(intensity, intensity, intensity, c.a);
			    }
                ", GRaff_GetFragColor);

        public static FragmentShader Sepia { get; }
            = new FragmentShader(
                Header + @"
                out highp vec4 out_FragColor;

                vec4 GRaff_GetFragColor(void);

                const mat3 sepiaMatrix = mat3(0.393, 0.349, 0.272, 0.769, 0.686, 0.534, 0.189, 0.168, 0.131);        

			    void main(void) {
                    vec4 c = GRaff_GetFragColor();
                    out_FragColor = vec4(sepiaMatrix * c.rgb, c.a);
			    }
                ", GRaff_GetFragColor);

    }
}