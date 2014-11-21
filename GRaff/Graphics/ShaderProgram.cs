using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.ES30;

namespace GRaff.Graphics
{
	public class ShaderProgram : IDisposable
	{
		private bool _disposed = false;
		public static ShaderProgram DefaultColored { get; } = _defaultProgram();
		static ShaderProgram _defaultProgram()
		{
			var program = new ShaderProgram(VertexShader.DefaultColored, FragmentShader.DefaultColored);
			GL.BindAttribLocation(program.Id, 0, "in_Position");
			GL.BindAttribLocation(program.Id, 1, "in_Color");
			return program;
		}

		public static ShaderProgram DefaultTextured { get; } = _texturedProgram();

		public static string ShaderVersion
		{
			get { return "400 core"; }
		}

		static ShaderProgram _texturedProgram()
		{
			var program = new ShaderProgram(VertexShader.DefaultTextured, FragmentShader.DefaultTextured);
			GL.BindAttribLocation(program.Id, 0, "in_Position");
			GL.BindAttribLocation(program.Id, 1, "in_Color");
			GL.BindAttribLocation(program.Id, 2, "in_TexCoord");
			return program;
		}

		public ShaderProgram(VertexShader vertexShader, FragmentShader fragmentShader)
		{
			Id = GL.CreateProgram();
			GL.AttachShader(Id, vertexShader.Id);
			GL.AttachShader(Id, fragmentShader.Id);
			GL.LinkProgram(Id);
			string log;/*C#6.0*/
			if ((log = GL.GetProgramInfoLog(Id)) != "")
				throw new ShaderException("Linking a GRaff.ShaderProgram caused a message: " + log);
			GL.DetachShader(Id, vertexShader.Id);
			GL.DetachShader(Id, fragmentShader.Id);
		}

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

				if (Giraffe.Window.Exists)
					GL.DeleteProgram(Id);
				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private static ShaderProgram _currentProgram;
        public static ShaderProgram Current
		{
			get
			{
				int actualProgramId;
				GL.GetInteger(GetPName.CurrentProgram, out actualProgramId);
				if (_currentProgram.Id != actualProgramId)
					throw new InvalidOperationException("The current program id is not equal to the id of the managed ShaderProgram object. Did you manually invoke a GL function (e.g. GL.UseProgram)?");
				return _currentProgram;
			}
			set
			{
				_currentProgram = value;
				GL.UseProgram(_currentProgram.Id);
			}
		}


		public int Id { get; private set; }
	}
}
