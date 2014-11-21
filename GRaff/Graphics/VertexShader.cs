using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES30;

namespace GRaff.Graphics
{
	public class VertexShader : IDisposable
	{
		private bool _disposed = false;

		public VertexShader(string source)
		{
			Id = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(Id, source);
			GL.CompileShader(Id);
			
			string msg;
			if ((msg = GL.GetShaderInfoLog(Id)) != "") /*C#6.0*/
				throw new ShaderException("Compiling a GRaff.VertexShader caused a message: " + msg);
		}

		public static VertexShader DefaultColored
		{ get; }
		= new VertexShader(
			@"#version " + ShaderProgram.ShaderVersion + @"
				uniform highp mat4 projectionMatrix;
				in lowp vec2 in_Position;
				in lowp vec4 in_Color;
				out lowp vec4 pass_Color;
				void main(void) {
					gl_Position = projectionMatrix * vec4(in_Position.x, in_Position.y, 0.0, 1.0);
					pass_Color = in_Color;
				}");

		public static VertexShader DefaultTextured{ get; } = new VertexShader(
			@"#version " + ShaderProgram.ShaderVersion + @"
				uniform highp mat4 projectionMatrix;
				in lowp vec2 in_Position;
				in lowp vec4 in_Color;
				in highp vec2 in_TexCoord;
				out lowp vec4 pass_Color;
				out lowp vec2 pass_TexCoord;
				void main(void) {
					gl_Position = projectionMatrix * vec4(in_Position.x, in_Position.y, 0.0, 1.0);
					pass_Color = in_Color;
					pass_TexCoord = in_TexCoord;
				}");




		~VertexShader()
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

		public int Id { get; private set; }
	}
}
