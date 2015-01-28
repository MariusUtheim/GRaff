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
		/// Gets or sets the x-coordinate of the center of the view in the room.
		/// </summary>
		public static double X { get; set; }

		/// <summary>
		/// Gets or sets the y-coordinate of the center of the view in the room.
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
	
		public static Rectangle FocusRegion
		{
			get { return new Rectangle(X - Width / 2, Y - Height / 2, Width, Height); }
			set { X = value.Center.X; Y = value.Center.Y; Width = value.Width; Height = value.Height; }
		}

		/// <summary>
		/// Gets or sets the rotation of the view. The view is rotated around the center of the view.
		/// </summary>
		public static Angle Rotation { get; set; }

		/// <summary>
		/// Gets or sets the center of the view in the room.
		/// </summary>
		public static Point Center
		{
			get { return new Point(X, Y); }
			set { X = value.X; Y = value.Y; }
		}

		/// <summary>
		/// Gets a Matrix that represents a transformation from points in view to the region [-1, 1] x [-1, 1] used by OpenGL.
		/// </summary>
		/// <returns></returns>
		public static AffineMatrix GetMatrix()
		{
			return AffineMatrix.Scaling(2 / Width, -2 / Height) * AffineMatrix.Rotation(Rotation) * AffineMatrix.Translation(-X, -Y);
		}

		internal static void LoadMatrix(int programId)
		{
			var tr = GetMatrix();
			//Matrix4 project = Matrix4.CreateOrthographicOffCenter((float)rect.Left, (float)rect.Right, (float)rect.Bottom, (float)rect.Top, 1, 0);
			//Matrix4 translateFromOrigin = Matrix4.CreateTranslation((float)Center.X, (float)Center.Y, 0);
			//Matrix4 rotate = Matrix4.CreateRotationZ((float)Rotation.Radians);
			//Matrix4 translateToOrigin = Matrix4.CreateTranslation(-(float)Center.X, -(float)Center.Y, 0);

/**/			var projectionMatrix = new Matrix4(
				(float)tr.M00, (float)tr.M10, 0, 0,
				(float)tr.M01, (float)tr.M11, 0, 0,
				0, 0, 1, 0,
				(float)tr.M02, (float)tr.M12, 0, 1
			);
			/**
			var projectionMatrix = new Matrix4(
				(float)tr.M00, (float)tr.M01, 0, (float)tr.M02,
				(float)tr.M01, (float)tr.M11, 0, (float)tr.M12,
				0, 0, 1, 0,
				0, 0, 0, 1
			);
			/**/

			int matrixLocation;
			matrixLocation = GL.GetUniformLocation(programId, "GRaff_ViewMatrix");
			GL.UniformMatrix4(matrixLocation, false, ref projectionMatrix);
		}


		/// <summary>
		/// Gets a factor representing the actual horizontal zoom of the view.
		/// A factor of 1.0 means the view is not zoomed; larger values indicates the view is zoomed in.
		/// </summary>
		public static double HZoom
		{
			get { return Width / Room.Width; }
		}

		/// <summary>
		/// Gets a factor representing the actual vertical zoom of the view.
		/// A factor of 1.0 means the view is not zoomed; larger values indicates the view is zoomed in.
		/// </summary>
		public static double VZoom
		{
			get { return Height / Room.Height; }
		}

		/// <summary>
		/// Gets a GRaff.Rectangle that is orthogonal to the axes, and that contains the whole section of the room visible in the View. 
		/// </summary>
		public static Rectangle BoundingBox
		{
			get
			{
				double left = double.PositiveInfinity, right = double.NegativeInfinity, top = double.NegativeInfinity, bottom = double.PositiveInfinity;
				var transform = GetMatrix();
				

				throw new NotImplementedException();
			}
		}
	}
}
