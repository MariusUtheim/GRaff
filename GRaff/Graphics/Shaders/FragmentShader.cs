#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics.Shaders
{
    public class FragmentShader : Shader
    {
        public FragmentShader(params string[] source)
            : base(ShaderType.FragmentShader, source) { }


        
        public static FragmentShader Default { get; }
            = new FragmentShader(
                ShaderHints.Header,
                ShaderHints.GetFragColor,
@"
out highp vec4 out_FragColor;

void main() {
    out_FragColor = GRaff_GetFragColor();
}
");

    }
}