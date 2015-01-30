using System;
using OpenTK;
using OpenTK.Graphics.ES30;
using GRaff.Graphics;
using System.Linq;

namespace GRaff
{
	/// <summary>
	/// Defines which part of the room is being drawn to the screen.
	/// </summary>
	public static class View
	{

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
			double w = 2.0 / Width, h = -2.0 / Height, c = GMath.Cos(Rotation), s = -GMath.Sin(Rotation);
			return new AffineMatrix(w * c, w * s, -w * (c * X + s * Y), -h * s, h * c, h * (s * X - c * Y));
			// Result is given by Scale(w, h) * Rotate(t) * Translate(-X, -Y)
		}

		internal static void LoadMatrix(int programId)
		{
			var tr = GetMatrix();

			var projectionMatrix = new Matrix4(
				(float)tr.M00, (float)tr.M10, 0, 0,
				(float)tr.M01, (float)tr.M11, 0, 0,
							0, 0, 1, 0,
				(float)tr.M02, (float)tr.M12, 0, 1
			);

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
		/// Anything outside this GRaff.Rectangle will not be seen by the View.
		/// </summary>
		/// <remarks>
		/// Things outside this GRaff.Rectangle will not be seen by the View, but they are still drawn automatically by default.
		/// The developer needs to manually disable drawing for instances outside this region (for example by setting IsVisible to false).
		/// </remarks>
		public static Rectangle BoundingBox
		{
			get
			{
				double c = GMath.Cos(Rotation), s = GMath.Sin(Rotation), w = Width / 2, h = Height / 2;
				Point[] pts = new[] {
					new Point(X + c * w - s * h, Y + s * w + c *h),
					new Point(X + c * -w - s * h, Y + s * -w + c * h),
					new Point(X + c * -w - s * -h,Y +  s * -w + c * -h),
					new Point(X + c * w - s * -h, Y + s * w + c * -h)
				};

				return new Rectangle(new Point(pts.Min(p => p.X), pts.Min(p => p.Y)), new Point(pts.Max(p => p.X), pts.Max(p => p.Y)));
			}
		}
	}
}
