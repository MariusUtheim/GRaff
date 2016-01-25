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
		public const string Version = "400 core";
		public const string Header = "#version " + Version + @"
";
        private bool _disposed;

		public Shader(bool isVertexShader, string source)
		{
			Id = GL.CreateShader(isVertexShader ? ShaderType.VertexShader : ShaderType.FragmentShader);
			GL.ShaderSource(Id, source);
			GL.CompileShader(Id);

			string msg;
			if ((msg = GL.GetShaderInfoLog(Id)) != "")
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
