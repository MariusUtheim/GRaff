using System;
using GRaff.Synchronization;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics
{
	public partial class Shader : IDisposable
	{
		public const string Version = "420 core";
		public const string Header = "#version " + Version + @"
";

        public static Shader DefaultVertexShader = new Shader(true,
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

        public static Shader DefaultFragmentShader = new Shader(false,
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


        private bool _disposed;

		public Shader(bool isVertexShader, string source)
		{
			Id = GL.CreateShader(isVertexShader ? ShaderType.VertexShader : ShaderType.FragmentShader);
			GL.ShaderSource(Id, source);
			GL.CompileShader(Id);

            _Graphics.ErrorCheck();

            var msg = GL.GetShaderInfoLog(Id);
            if (msg != "")
				throw new ShaderException("Compiling a GRaff.Shader caused a message: " + msg);
		}

		public int Id { get; private set; }


		~Shader()
		{
			Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
				}

				if (Context.IsAlive)
					GL.DeleteShader(Id);
				else
					Async.Run(() => GL.DeleteShader(Id));
				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}


	}
}
