using System;
using OpenTK.Graphics.ES30;

namespace GRaff.Graphics
{
	public class FragmentShader : IDisposable
	{
		private bool _disposed = false;

		public FragmentShader(string source)
		{
			Id = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(Id, source);
			GL.CompileShader(Id);
			string msg;/*C#6.0*/
			if ((msg = GL.GetShaderInfoLog(Id)) != "")
				throw new ShaderException("Compiling a GRaff.VertexShader caused a message: " + msg);
		}

		~FragmentShader()
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

				if (Giraffe.Window.Exists)
					GL.DeleteShader(Id);
				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public static FragmentShader DefaultColored { get; } = new FragmentShader(@"
			#version 400 core
			in lowp vec4 pass_Color;
			out lowp vec4 out_Color;
			void main () {
				gl_FragColor = pass_Color;
			}");

		public static FragmentShader DefaultTextured { get; } = new FragmentShader(@"
			#version 400 core
			in lowp vec4 pass_Color;
			in mediump vec2 pass_TexCoord;
			out lowp vec4 out_Color;
			uniform highp sampler2D tex;
			void main () {
				gl_FragColor = texture(tex, pass_TexCoord).bgra * pass_Color;
			}");

		public int Id { get; private set; }
	}
}