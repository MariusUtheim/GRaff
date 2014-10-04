using OpenTK;
using OpenTK.Graphics.OpenGL4;


namespace GameMaker
{
	/// <summary>
	/// Defines which part of the room is being drawn to the screen.
	/// </summary>
	public static class View
	{
		private static int _shaderProgram;
		private static int _vertexShader, _fragmentShader;

		static View()
		{
			_vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(_vertexShader,
				"#version 300 es\n" +
				"uniform mat4 projectionMatrix;" +
				"in vec2 in_Position;" +
				"in vec4 in_Color;" +
				"in vec2 in_TexCoord;" +
				"out vec4 pass_Color;" +
				"out vec2 pass_TexCoord;" +
				"void main(void) {" +
				"	gl_Position = projectionMatrix * vec4(in_Position.x, in_Position.y, 0, 1.0);" +
				"   pass_Color = in_Color;" +
				"	pass_TexCoord = in_TexCoord;" +
				"}");
			GL.CompileShader(_vertexShader);

			_fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(_fragmentShader,
				"#version 300 es\n" +
				"in vec4 pass_Color;" +
				"in vec2 pass_TexCoord;" +
				"out vec4 out_Color;" +
				"uniform sampler2D tex;" +
				"uniform bool drawTexture;" +
				"void main () {" +
				"   vec4 c = vec4(pass_Color.x / 255.0, pass_Color.y / 255.0, pass_Color.z / 255.0, pass_Color.w / 255.0);" +
				"	if (drawTexture) gl_FragColor = texture(tex, pass_TexCoord).zyxw * c;" +
				"	else gl_FragColor = c;" +
				"}");
			GL.CompileShader(_fragmentShader);

			_shaderProgram = GL.CreateProgram();
			GL.AttachShader(_shaderProgram, _vertexShader);
			GL.AttachShader(_shaderProgram, _fragmentShader);
			GL.LinkProgram(_shaderProgram);
			GL.BindAttribLocation(_shaderProgram, 0, "in_Position");
			GL.BindAttribLocation(_shaderProgram, 1, "in_Color");
			GL.BindAttribLocation(_shaderProgram, 2, "in_TexCoord");
			GL.UseProgram(_shaderProgram);

			GL.ValidateProgram(_shaderProgram);
			var msg = GL.GetProgramInfoLog(_shaderProgram);
			int res;
			GL.GetProgram(_shaderProgram, GetProgramParameterName.ValidateStatus, out res);
			if (msg != "" || res != 1)
			{
			}

		}

		internal static void EnableTexture()
		{
			GL.Uniform1(GL.GetUniformLocation(_shaderProgram, "drawTexture"), 1);
		}
		internal static void DisableTexture()
		{
			GL.Uniform1(GL.GetUniformLocation(_shaderProgram, "drawTexture"), 0);
		}

		/// <summary>
		/// Gets or sets the x-coordinate of the top-left corner of the view in the room.
		/// </summary>
		public static double X { get; set; }

		/// <summary>
		/// Gets or sets the y-coordinate of the top-left corner of the view in the room.
		/// </summary>
		public static double Y { get; set; }

		/// <summary>
		/// Gets or sets the width of the view in the room.
		/// </summary>
		public static double Width { get; set; }

		/// <summary>
		/// Gets or sets the height of the view in the room.
		/// </summary>
		public static double Height { get; set; }
	
		/// <summary>
		/// Gets or sets the rotation of the view. The view is rotated around the center of the view.
		/// </summary>
		public static Angle Rotation { get; set; }

		/// <summary>
		/// Gets or sets a GameMaker.Rectangle representing the view in the room.
		/// </summary>
		public static Rectangle RoomView
		{
			get { return new Rectangle(X, Y, Width, Height); }
			set
			{
				X = value.Left;
				Y = value.Top;
				Width = value.Width;
				Height = value.Height;
			}
		}

		/// <summary>
		/// Gets or sets the center of the view in the room.
		/// </summary>
		public static Point Center
		{
			get { return new Point(X + Width / 2, Y + Height / 2); }
			set { X = value.X - Width / 2; Y = value.Y - Height / 2; }
		}


		internal static void LoadMatrix()
		{
			Rectangle rect = ActualView();

			Matrix4 project = Matrix4.CreateOrthographicOffCenter((float)rect.Left, (float)rect.Right, (float)rect.Bottom, (float)rect.Top, 1, 0);
            Matrix4 translateFromOrigin = Matrix4.CreateTranslation((float)Center.X, (float)Center.Y, 0);
			Matrix4 rotate = Matrix4.CreateRotationZ((float)Rotation.Radians);
			Matrix4 translateToOrigin = Matrix4.CreateTranslation(-(float)Center.X, -(float)Center.Y, 0);

			Matrix4 projectionMatrix = translateToOrigin * rotate * translateFromOrigin * project;

			Vector4 testVector = new Vector4((float)Center.X + 40, (float)Center.Y + 40, 0, 1);
			Vector4 res = testVector;
			res = Vector4.Transform(res, translateToOrigin);
			res = Vector4.Transform(res, rotate);
			res = Vector4.Transform(res, translateFromOrigin);


			//Matrix4 projectionMatrix = Matrix4.Identity;
			int matrixLocation = GL.GetUniformLocation(_shaderProgram, "projectionMatrix");
			GL.UniformMatrix4(matrixLocation, false, ref projectionMatrix);
			//GL.UseProgram(0);
			/*
			GL.LoadIdentity();
			GL.Ortho(-w, w, h, -h, 0.0, 1.0);
			GL.Rotate(View.Rotation.Degrees, 0, 0, 1);
			GL.Translate(rect.Left - w, rect.Top - h, 0);
			*/
		}


		/// <summary>
		/// Gets a GameMaker.Rectangle representing which part of the room is actually being drawn.
		/// This is equal to GameMaker.View.RoomView, unless part of that view is outside of the room.
		/// The actual view is translated to fit inside the room; if the dimensions of the view are larger
		/// than the dimensions of the room, the actual view will also be scaled down.
		/// </summary>
		public static Rectangle ActualView()
		{
#warning DESIGN Ensure that ActualView is always inside the room?
			return RoomView;
		/*	double x = RoomView.Left, y = RoomView.Top, w = RoomView.Width, h = RoomView.Height;

			if (w > Room.Width)
			{
				x = 0;
				w = Room.Width;
			}
			else
				x = GMath.Median(0, x, Room.Width - w);

			if (h > Room.Height)
			{
				y = 0;
				h = Room.Height;
			}
			else
				y = GMath.Median(0, y, Room.Height - h);

			return new Rectangle(x, y, w, h) ;
			*/
		}

		/// <summary>
		/// Gets a factor representing the actual horizontal zoom of the view.
		/// A factor of 1.0 means the view is not zoomed; larger values indicates the view is zoomed in.
		/// </summary>
		public static double HZoom
		{
			get { return ActualView().Width / (double)Room.Width; }
		}

		/// <summary>
		/// Gets a factor representing the actual vertical zoom of the view.
		/// A factor of 1.0 means the view is not zoomed; larger values indicates the view is zoomed in.
		/// </summary>
		public static double VZoom
		{
			get { return ActualView().Height / (double)Room.Height; }
		}
	}
}
