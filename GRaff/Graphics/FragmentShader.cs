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
vec4 GRaff_GetFragColor(sampler2D tex, vec2 texCoord, vec4 baseColor) {
    if (GRaff_IsTextured)
        return texture(tex, texCoord).rgba * baseColor;
    else
        return baseColor;
}
vec4 GRaff_GetFragColor(void) { return GRaff_GetFragColor(GRaff_Texture, GRaff_TexCoord, GRaff_Color); }
vec4 GRaff_GetFragColor(vec2 texCoord) { return GRaff_GetFragColor(GRaff_Texture, texCoord, GRaff_Color); }

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