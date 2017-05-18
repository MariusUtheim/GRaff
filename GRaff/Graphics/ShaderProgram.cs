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

		public static ShaderProgram Current { get; private set; }

		public void SetCurrent()
		{
			Current = this;
			GL.UseProgram(Id);
			View.LoadMatrix(this);
		}
		
		public static int GetCurrentId()
		{
			return GL.GetInteger(GetPName.CurrentProgram);
		}

		public void UpdateUniformValues()
		{
			int currentProgram = GL.GetInteger(GetPName.CurrentProgram);
			try
			{
				int location;

				SetCurrent();

				location = GL.GetUniformLocation(Id, "GRaff_LoopCount");
				GL.Uniform1(location, Time.LoopCount);

				location = GL.GetUniformLocation(Id, "GRaff_RoomWidth");
				GL.Uniform1(location, Room.Current.Width);

				location = GL.GetUniformLocation(Id, "GRaff_RoomHeight");
				GL.Uniform1(location, Room.Current.Height);

				View.LoadMatrix(this);
			}
			finally
			{
				GL.UseProgram(currentProgram);
			}
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
