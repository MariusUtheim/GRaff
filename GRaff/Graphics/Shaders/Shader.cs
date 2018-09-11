﻿using System;
using GRaff.Synchronization;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics.Shaders
{
	public abstract class Shader : IDisposable
	{
        private bool _disposed;

		protected Shader(ShaderType type, params string[] source)
		{
            var src = String.Concat(source);
			Id = GL.CreateShader(type);
			GL.ShaderSource(Id, src);
			GL.CompileShader(Id);

            var msg = GL.GetShaderInfoLog(Id);
            if (msg != "")
                throw new ShaderException("Compiling a GRaff.Shader caused a message: " + msg, String.Concat(source));

			_Graphics.ErrorCheck();
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

                if (_Graphics.IsContextActive)
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