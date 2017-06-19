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

		public ShaderProgram(VertexShader vertexShader, FragmentShader fragmentShader)
		{
			Contract.Requires<ArgumentNullException>(vertexShader != null && fragmentShader != null);
			Id = GL.CreateProgram();
            
			GL.AttachShader(Id, vertexShader.Id);
            GL.AttachShader(Id, fragmentShader.Id);

			GL.LinkProgram(Id);
			
			GL.BindAttribLocation(Id, 0, "in_Position");
			GL.BindAttribLocation(Id, 1, "in_Color");
			GL.BindAttribLocation(Id, 2, "in_TexCoord");

            GL.BindFragDataLocation(Id, 0, "out_FragColor");

            GL.DetachShader(Id, vertexShader.Id);
            GL.DetachShader(Id, fragmentShader.Id);

			string log;
			if ((log = GL.GetProgramInfoLog(Id)) != "")
				throw new ShaderException("Linking a GRaff.ShaderProgram caused a message: " + log);

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

        #region Uniforms access

        private void _verifyLocation(ShaderUniformLocation location)
        {
            Contract.Requires<InvalidOperationException>(location.Program == this);
        }

        protected ShaderUniformLocation UniformLocation(string name) => new ShaderUniformLocation(this, name);

        protected int GetUniformInt(ShaderUniformLocation location)
        {
            _verifyLocation(location);
            GL.GetUniform(Id, location.Location, out int value);
            return value;
        }

		protected float GetUniformFloat(ShaderUniformLocation location)
		{
			_verifyLocation(location);
			GL.GetUniform(Id, location.Location, out float value);
			return value;
		}

        protected double GetUniformDouble(ShaderUniformLocation location)
        {
            _verifyLocation(location);
            GL.GetUniform(Id, location.Location, out double value);
            return value;
        }

        protected (float x, float y) GetUniformVec2(ShaderUniformLocation location)
        {
            _verifyLocation(location);
            var values = new float[2];
            GL.GetUniform(Id, location.Location, values);
            return (x: values[0], y: values[1]);
        }

        protected void SetUniformInt(ShaderUniformLocation location, int value)
        {
            _verifyLocation(location);
            GL.ProgramUniform1(Id, location.Location, value);
        }

        protected void SetUniformFloat(ShaderUniformLocation location, float value)
        {
            _verifyLocation(location);
            GL.ProgramUniform1(Id, location.Location, value);
        }

        protected void SetUniformDouble(ShaderUniformLocation location, double value)
        {
            _verifyLocation(location);
            GL.ProgramUniform1(Id, location.Location, value);
        }

        protected void SetUniformVec2(ShaderUniformLocation location, (float x, float y) value)
        {
            _verifyLocation(location);
            GL.ProgramUniform2(Id, location.Location, value.x, value.y);
        }
        protected void SetUniformVec2(ShaderUniformLocation location, (double x, double y) value)
        {
            _verifyLocation(location);
            GL.ProgramUniform2(Id, location.Location, (float)value.x, (float)value.y);
        }

        #endregion


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
				if (_Graphics.IsContextActive)
					GL.DeleteProgram(Id);
				else
					Async.Run(() => GL.DeleteProgram(Id));
				_disposed = true;
			}
		}

        #endregion

    }
}
