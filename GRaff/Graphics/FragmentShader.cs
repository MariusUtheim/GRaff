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
}
";
        
        public static FragmentShader Default { get; }
            = new FragmentShader(
                GRaff_Header,
                GRaff_GetFragColor,
                @"
                out highp vec4 out_FragColor;

                void main() {
                    out_FragColor = GRaff_GetFragColor();
			    }
                ");

    }
}