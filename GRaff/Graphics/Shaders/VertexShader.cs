﻿#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics.Shaders
{
    public class VertexShader : Shader
    {
        public VertexShader(params string[] source)
            : base(ShaderType.VertexShader, source) { }
        
        public static VertexShader Default { get; }
            = new VertexShader(
                ShaderHints.Header,
                @"
in highp vec2 in_Position;
in lowp vec4 in_Color;
in highp vec2 in_TexCoord;
out lowp vec4 GRaff_Color;
out highp vec2 GRaff_TexCoord;

uniform highp mat4 GRaff_ViewMatrix;

void main(void) {
    gl_Position = GRaff_ViewMatrix * vec4(in_Position.x, in_Position.y, 0.0, 1.0);
    GRaff_Color = in_Color;
    GRaff_TexCoord = in_TexCoord;
}");

    }
    
}
