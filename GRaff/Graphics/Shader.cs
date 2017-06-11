using System;
using GRaff.Synchronization;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics
{
	public abstract class Shader : IDisposable
	{
		public const string Version = "420 core";
		public const string Header = "#version " + Version + @"
";


        private bool _disposed;

		protected Shader(ShaderType type, params string[] source)
		{
			Id = GL.CreateShader(type);
			GL.ShaderSource(Id, source.Length, source, (int[])null);
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
