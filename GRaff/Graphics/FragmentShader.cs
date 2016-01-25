using System;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


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
			string msg;
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


		public int Id { get; private set; }
	}
}