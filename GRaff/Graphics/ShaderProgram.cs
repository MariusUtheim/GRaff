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

        public int Id { get; }

        public void Bind()
		{
			Current = this;
			View.LoadMatrixToProgram();
		}

        public IDisposable Use()
        {
            return UseContext.CreateAt(
                $"{typeof(ShaderProgram).FullName}.{nameof(Use)}",
                ShaderProgram.Current,
                () => this.Bind(),
                previous => previous.Bind()
                );
        }


        private bool _tryGetLocation(string name, out int location)
        {
            location = GL.GetUniformLocation(Id, name);
            if (location < 0)
            {
                _Graphics.ClearError();
                return false;
            }
            else
                return true;
        }
        
        internal protected void SetUniform(string name, bool value)
        {
            if (_tryGetLocation(name, out int location))
                GL.ProgramUniform1(Id, location, value ? 1 : 0);
        }

        internal protected void SetUniform(string name, int value)
		{
            if (_tryGetLocation(name, out int location))
                GL.ProgramUniform1(Id, location, value);
		}

        internal protected void SetUniform(string name, double value)
		{
            if (_tryGetLocation(name, out int location))
                GL.ProgramUniform1(Id, location, value);
        }

        internal protected void SetUniform(string name, (double v1, double v2) values)
        {
            if (_tryGetLocation(name, out int location))
                GL.ProgramUniform2(Id, location, (float)values.v1, (float)values.v2);
        }
        

        #region IDisposable support

        ~ShaderProgram()
		{
			Dispose(false);
		}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (Context.IsActive)
					GL.DeleteProgram(Id);
				else
					Async.Run(() => GL.DeleteProgram(Id));
				_disposed = true;
			}
		}

        #endregion

    }
}
