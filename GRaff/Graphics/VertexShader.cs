#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics
{
    public class VertexShader : Shader
    {
        public VertexShader(string source)
            : base(ShaderType.VertexShader, source) { }

        public static VertexShader Default { get; }
            = new VertexShader(
                Header + @"
		        in highp vec2 in_Position;
		        in lowp vec4 in_Color;
		        in highp vec2 in_TexCoord;
		        out lowp vec4 pass_Color;
		        out highp vec2 pass_TexCoord;

		        uniform highp mat4 GRaff_ViewMatrix;

		        void main(void) {
		        	gl_Position = GRaff_ViewMatrix * vec4(in_Position.x, in_Position.y, 0.0, 1.0);
		        	pass_Color = in_Color;
		        	pass_TexCoord = in_TexCoord;
		        }");
    }
    
}
