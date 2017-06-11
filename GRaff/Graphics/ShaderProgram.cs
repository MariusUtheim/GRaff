using System;
using GRaff.Synchronization;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
using coords = System.Double;
#else
using OpenTK.Graphics.ES30;
using coords = System.Single;
#endif


namespace GRaff.Graphics
{
#warning Review class
    public class ShaderProgram : IDisposable
	{
		private bool _disposed = false;

        public static ShaderProgram Default { get; } = new ShaderProgram(VertexShader.Default, FragmentShader.Default);

        public static ShaderProgram BlackWhite { get; } = new ShaderProgram(VertexShader.Default, FragmentShader.BlackWhite);

        public static ShaderProgram Sepia { get; } = new ShaderProgram(VertexShader.Default, FragmentShader.Sepia);

		public ShaderProgram(VertexShader vertexShader, FragmentShader fragmentShader)
		{
			Contract.Requires<ArgumentNullException>(vertexShader != null && fragmentShader != null);
			Id = GL.CreateProgram();
            
			GL.AttachShader(Id, vertexShader.Id);
            GL.AttachShader(Id, fragmentShader.Id);

			GL.LinkProgram(Id);
			string log;
			if ((log = GL.GetProgramInfoLog(Id)) != "")
				throw new ShaderException("Linking a GRaff.ShaderProgram caused a message: " + log);

			GL.BindAttribLocation(Id, 0, "in_Position");
			GL.BindAttribLocation(Id, 1, "in_Color");
			GL.BindAttribLocation(Id, 2, "in_TexCoord");

            GL.BindFragDataLocation(Id, 0, "out_FragColor");

            GL.DetachShader(Id, vertexShader.Id);
            GL.DetachShader(Id, fragmentShader.Id);
        }

        private static ShaderProgram _current;
		public static ShaderProgram Current
		{
			get { return _current; }
			private set { _current = value; GL.UseProgram(_current.Id); }
		}

		public void SetCurrent()
		{
			Current = this;
			View.LoadMatrixToProgram();
		}
		
		public static int GetCurrentId()
		{
			return GL.GetInteger(GetPName.CurrentProgram);
		}

        public void SetUniform(string name, bool value)
        {
            var location = GL.GetUniformLocation(Id, name);
            if (location < 0)
                _Graphics.ClearError();
            else
            {
                GL.Uniform1(location, value ? 1 : 0);
                _Graphics.ErrorCheck();
            }
        }

        public void SetUniform(string name, int value)
		{
            var location = GL.GetUniformLocation(Id, name);
            GL.Uniform1(location, value);
            _Graphics.ErrorCheck();
		}
        
        public void SetUniform(string name, double value)
		{
            var location = GL.GetUniformLocation(Id, name);
            _Graphics.ErrorCheck();
            GL.Uniform1(location, (float)value);
            _Graphics.ErrorCheck();
		}

		public int Id { get; private set; }

        public IDisposable Use() => new ShaderProgramContext(this);

		~ShaderProgram()
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
					GL.DeleteProgram(Id);
				else
					Async.Run(() => GL.DeleteProgram(Id));
				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}


        private class ShaderProgramContext : IDisposable
        {
            private ShaderProgram _previous;
            private bool _isDisposed = false;

            public ShaderProgramContext(ShaderProgram program)
            {
                this._previous = ShaderProgram.Current;
                program.SetCurrent();
            }

            ~ShaderProgramContext()
            {
                Async.Throw(new ObjectDisposedIncorrectlyException("A context returned from GRaff.ShaderProgram.Use was garbage collected before Dispose was called."));
            }

            public void Dispose()
            {
                if (!_isDisposed)
                {
                    GC.SuppressFinalize(this);
                    _isDisposed = true;
                    _previous.SetCurrent();
                }
                else
                    throw new ObjectDisposedException("ShaderProgram");
            }
        }

    }
}
