using System;
using System.Linq;
using GRaff.Graphics;
using GRaff.Synchronization;
using OpenTK;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff
{
#warning Review class
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

		public static void InverseFocusRegion(Rectangle region, Vector size)
		{
			X -= region.Left;
			Y -= region.Top;
			Width *= region.Width / size.X;
			Height *= region.Height / size.Y;
		}

		public static void SetTransformation(Transform t)
		{
			X = t.X;
			Y = t.Y;
			Width = t.XScale;
			Height = t.YScale;
			Rotation = t.Rotation;
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
		public static Matrix GetMatrix()
		{
			double w = 2.0 / Width, h = -2.0 / Height, c = GMath.Cos(Rotation), s = -GMath.Sin(Rotation);
			return new Matrix(w * c, w * s, -w * (c * X + s * Y), -h * s, h * c, h * (s * X - c * Y));
			// Result is given by Scale(w, h) * Rotate(t) * Translate(-X, -Y)
		}
		
		internal static void LoadMatrixToProgram()
		{
			if (ShaderProgram.Current == null)
				return;

			var tr = GetMatrix();

            var projectionMatrix = new Matrix4(
				(float)tr.M00, (float)tr.M01, 0, (float)tr.M02,
				(float)tr.M10, (float)tr.M11, 0, (float)tr.M12,
							0,			   0, 1, 0,
							0,			   0, 0, 1
			);

			int matrixLocation;
			matrixLocation = GL.GetUniformLocation(ShaderProgram.Current.Id, "GRaff_ViewMatrix");
            GL.UniformMatrix4(matrixLocation, true, ref projectionMatrix);

            _Graphics.ErrorCheck();
        }

        public static Point ScreenToRoom(double x, double y)
		{
			return GetMatrix().Inverse * new Point(2.0 * x / Window.Width - 1.0, -2.0 * y / Window.Height + 1.0);
			//return GetMatrix() * new Point(
		}

		public static Point RoomToScreen(double x, double y)
		{
			var p = GetMatrix() * new Point(x, y);
			return new Point((p.X + 1.0) / 2.0 * Window.Width, (p.Y - 1.0) / 2.0 * -Window.Height);
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

				Point tl = new Point(pts.Min(p => p.X), pts.Min(p => p.Y)), br = new Point(pts.Max(p => p.X), pts.Max(p => p.Y));
                return new Rectangle(tl, br - tl);
			}
		}

		private class ViewContext : IDisposable
		{
			private double _prevX, _prevY, _prevW, _prevH;
			private Angle _prevR;
			private bool _isDisposed = false;

			public ViewContext(double x, double y, double width, double height, Angle rotation)
			{
				this._prevX = View.X;
				this._prevY = View.Y;
				this._prevW = View.Width;
				this._prevH = View.Height;
				this._prevR = View.Rotation;
				View.X = x;
				View.Y = y;
				View.Width = width;
				View.Height = height;
				View.Rotation = rotation;
				View.LoadMatrixToProgram();
			}

			~ViewContext()
			{
				Async.Throw(new ObjectDisposedIncorrectlyException($"A context returned from {nameof(GRaff.View.UseView)} was garbage collected before Dispose was called."));
			}

			public void Dispose()
			{
				if (!_isDisposed)
				{
					GC.SuppressFinalize(this);
					_isDisposed = true;
					View.X = _prevX;
					View.Y = _prevY;
					View.Width = _prevW;
					View.Height = _prevH;
					View.Rotation = _prevR;
					View.LoadMatrixToProgram();
				}
				else
					throw new ObjectDisposedException(nameof(ViewContext));
			}

		}

		public static IDisposable UseView(double x, double y, double width, double height, Angle rotation = default(Angle))
		{
			return new ViewContext(x, y, width, height, rotation);
		}
	}
}
