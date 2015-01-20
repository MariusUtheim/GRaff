

namespace GRaff.Graphics
{
	public partial class Shader
    {

		public static Shader DefaultColoredVertexShader = new Shader(true,
				Header + @"
				in highp vec2 in_Position;
				in highp vec4 in_Color;
				out lowp vec4 pass_Color;

				uniform highp mat4 GRaff_ViewMatrix;

				void main(void) {
					gl_Position = GRaff_ViewMatrix * vec4(in_Position.x, in_Position.y, 0.0, 1.0);
					pass_Color = in_Color;
				}");


		public static Shader DefaultTexturedVertexShader = new Shader(true,
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

		public static Shader DefaultColoredFragmentShader = new Shader(false,
			Header + @"
			in highp vec4 pass_Color;
			
			void main () {
				gl_FragColor = pass_Color;
			}");

		public static Shader DefaultTexturedFragmentShader = new Shader(false,
			Header + @"
			in lowp vec4 pass_Color;
			in highp vec2 pass_TexCoord;
			uniform highp sampler2D tex;

			void main () {
				gl_FragColor = texture(tex, pass_TexCoord).bgra * pass_Color;
			}");

	}
}
