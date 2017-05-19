using System;
using System.Diagnostics.Contracts;
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
		public const string ShaderVersion = "400 core";

		private bool _disposed = false;

		public static ShaderProgram DefaultColored = new ShaderProgram(Shader.DefaultColoredVertexShader, Shader.DefaultColoredFragmentShader);
		public static ShaderProgram DefaultTextured = new ShaderProgram(Shader.DefaultTexturedVertexShader, Shader.DefaultTexturedFragmentShader);
		public static ShaderProgram CurrentColored;
		public static ShaderProgram CurrentTextured;

		public ShaderProgram(params Shader[] shaders)
		{
			Contract.Requires<ArgumentNullException>(shaders != null);
			Id = GL.CreateProgram();

			foreach (var shader in shaders)
				GL.AttachShader(Id, shader.Id);

			GL.LinkProgram(Id);
			string log;
			if ((log = GL.GetProgramInfoLog(Id)) != "")
				throw new ShaderException("Linking a GRaff.ShaderProgram caused a message: " + log);

			GL.BindAttribLocation(Id, 0, "in_Position");
			GL.BindAttribLocation(Id, 1, "in_Color");
			GL.BindAttribLocation(Id, 2, "in_TexCoord");

			foreach (var shader in shaders)
				GL.DetachShader(Id, shader.Id);
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

		private void _setUniform(string location, double value)
		{
			var ptr = GL.GetUniformLocation(Id, location);
			if (ptr >= 0)
				GL.Uniform1(ptr, value);
		}

		public void UpdateUniformValues()
		{
			SetCurrent();
			_setUniform("GRaff_LoopCount", Time.LoopCount);
			_setUniform("GRaff_RoomWidth", Room.Current.Width);
			_setUniform("GRaff_RoomHeight", Room.Current.Height);
		}

		protected void SetUniform(string name, int value)
		{
			GL.Uniform1(GL.GetUniformLocation(Id, name), value);
		}

		protected void SetUniform(string name, uint value)
		{
			GL.Uniform1(GL.GetUniformLocation(Id, name), value);
		}


		protected void SetUniform(string name, double value)
		{
			GL.Uniform1(GL.GetUniformLocation(Id, name), (coords)value);
		}

		public int Id { get; private set; }

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

	}
}
