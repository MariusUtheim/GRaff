using System;
using OpenTK.Graphics.OpenGL4;

namespace GRaff.Graphics.Shaders
{
    public static class ShaderHints
    {
        public static string Version { get; } = $"{GL.GetInteger(GetPName.MajorVersion)}{GL.GetInteger(GetPName.MinorVersion)}0";

        public static string Header { get; } = "#version " + Version + Environment.NewLine;
   
        /// <summary>
        /// Implements the method GRaff_GetFragColor() which gets the current fragment as 
        /// specified by Draw commands. This depends on the specified color, as well as the
        /// texture if the Draw command was specified to be textured. 
        /// </summary>
        /// <value>The color of the get frag.</value>
        public static string GetFragColor { get; } =
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
vec4 GRaff_GetFragColor(vec4 baseColor) { return GRaff_GetFragColor(GRaff_Texture, GRaff_TexCoord, baseColor); }
";

        public static string gl_FragCoord { get; } = @"
layout(origin_upper_left) in vec4 gl_FragCoord;
";

        public static string out_FragColor { get; } = @"
out vec4 out_FragColor;
";
    }
}
