using System;
using OpenTK;
using OpenTK.Graphics.ES30;
using GRaff.Graphics;

namespace GRaff
{
	/// <summary>
	/// Defines which part of the room is being drawn to the screen.
	/// </summary>
	public static class View
	{
		static View()
		{

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
		/// Gets or sets a GRaff.Rectangle representing the view in the room.
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
			Rectangle rect = RoomView;
			
			Matrix4 project = Matrix4.CreateOrthographicOffCenter((float)rect.Left, (float)rect.Right, (float)rect.Bottom, (float)rect.Top, 1, 0);
            Matrix4 translateFromOrigin = Matrix4.CreateTranslation((float)Center.X, (float)Center.Y, 0);
			Matrix4 rotate = Matrix4.CreateRotationZ((float)Rotation.Radians);
			Matrix4 translateToOrigin = Matrix4.CreateTranslation(-(float)Center.X, -(float)Center.Y, 0);

			Matrix4 projectionMatrix = translateToOrigin * rotate * translateFromOrigin * project;

			ShaderProgram previousProgram = ShaderProgram.Current;

			int matrixLocation;
			matrixLocation = GL.GetUniformLocation(ShaderProgram.DefaultColored.Id, "projectionMatrix");
			ShaderProgram.Current = ShaderProgram.DefaultColored;
			GL.UniformMatrix4(matrixLocation, false, ref projectionMatrix);

			matrixLocation = GL.GetUniformLocation(ShaderProgram.DefaultTextured.Id, "projectionMatrix");
			ShaderProgram.Current = ShaderProgram.DefaultTextured;
			GL.UniformMatrix4(matrixLocation, false, ref projectionMatrix);

			ShaderProgram.Current = previousProgram;
			//GL.UseProgram(0);
		}


		/// <summary>
		/// Gets a factor representing the actual horizontal zoom of the view.
		/// A factor of 1.0 means the view is not zoomed; larger values indicates the view is zoomed in.
		/// </summary>
		public static double HZoom
		{
			get { return RoomView.Width / (double)Room.Width; }
		}

		/// <summary>
		/// Gets a factor representing the actual vertical zoom of the view.
		/// A factor of 1.0 means the view is not zoomed; larger values indicates the view is zoomed in.
		/// </summary>
		public static double VZoom
		{
			get { return RoomView.Height / (double)Room.Height; }
		}
	}
}
