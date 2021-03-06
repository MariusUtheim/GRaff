﻿using System;
using GRaff.Synchronization;
using OpenTK.Graphics.OpenGL4;


namespace GRaff.Graphics.Shaders
{

    public class ShaderProgram : IDisposable
	{

        public static ShaderProgram Default { get; } = new ShaderProgram(VertexShader.Default, FragmentShader.Default);

		public ShaderProgram(VertexShader vertexShader, FragmentShader fragmentShader)
		{
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


        #region IDisposable support

        public bool IsDisposed { get; private set; }

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
			if (!IsDisposed)
			{
                Async.Run(() =>
                {
                    if (_Graphics.IsContextActive)
                        GL.DeleteProgram(Id);
                });
                IsDisposed = true;
			}
		}

        #endregion

    }
}
