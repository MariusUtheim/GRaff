using System;
using System.Diagnostics;
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
	public class View
	{
        //TODO// Define up direction
        private static readonly Triangle GLTriangle = new Triangle((-1, 1), (1, 1), (-1, -1));
        private static Matrix GLToViewMatrix;

        private Transform _transform;
        private bool _needsValidation = false;

        public View(Matrix viewMatrix)
        {
            Contract.Requires<ArgumentNullException>(viewMatrix != null);
            Contract.Requires<MatrixException>(viewMatrix.Determinant != 0);
            this._transform = new Transform(viewMatrix);
        }

        internal static void UpdateGLToRoomMatrix()
        {
            GLToViewMatrix = Matrix.Mapping(GLTriangle, new Triangle((0, 0), (Window.Width, 0), (0, Window.Height)));
        }

        public static View FullWindow() => Rectangle(Window.ClientRectangle);

        public static View Triangle(Triangle t) => new View(Matrix.Mapping(t, GLTriangle));

        public static View Rectangle(double x, double y, double width, double height) => Rectangle(new Rectangle(x, y, width, height));

        public static View Rectangle(Rectangle rect)
        {
            var m = Matrix.Translation(-(Vector)rect.Center).Scale(2.0 / rect.Width, -2.0 / rect.Height);
            return new View(m);
        }

        public static View Centered(Point location, Vector size) => Rectangle(new Rectangle((location - size / 2), size));

        public static View DrawTo(Point location)
        {
            return new View(View.Current.ViewMatrix * Matrix.Translation(location));
        }

        internal static void Validate()
        {
            if (Current._needsValidation)
                LoadMatrixToProgram();
        }
                

		internal static void LoadMatrixToProgram()
		{
			if (ShaderProgram.Current == null)
				return;

			var tr = Current.ViewMatrix;

			var projectionMatrix = new Matrix4(
				(float)tr.M00, (float)tr.M01, 0, (float)tr.M02,
				(float)tr.M10, (float)tr.M11, 0, (float)tr.M12,
							0, 0, 1, 0,
							0, 0, 0, 1
			);

			int matrixLocation;
			matrixLocation = GL.GetUniformLocation(ShaderProgram.Current.Id, "GRaff_ViewMatrix");
			GL.UniformMatrix4(matrixLocation, true, ref projectionMatrix);

            Current._needsValidation = false;
			_Graphics.ErrorCheck();
		}



        public static View Current { get; private set; } = new View(new Matrix());


        private void _invalidate(object _)
        {
            _needsValidation = false;
        }


		public Matrix ViewMatrix
        {
            get => _transform.GetMatrix();
            set => _transform = new Transform(value);
        }

        public double X
        {
            get => _transform.X;
            set => _invalidate(_transform.X = value);
        }

        public double Y
        {
            get => _transform.Y;
            set => _invalidate(_transform.Y = value);
        }

        public Point Location
        {
            get => _transform.Location;
            set => _invalidate(_transform.Location = value);
        }

        public Angle Rotation
        {
            get => _transform.Rotation;
            set => _invalidate(_transform.Rotation = value);
        }

        public double XScale
        {
            get => _transform.XScale;
            set => _invalidate(_transform.XScale = value);
        }

        public double YScale
        {
            get => _transform.YScale;
            set => _invalidate(_transform.YScale = value);
        }

        public Vector Scale
        {
            get => _transform.Scale;
            set => _invalidate(_transform.Scale = value);
        }

        public double XShear
        {
            get => _transform.XShear;
            set => _invalidate(_transform.XShear = value);
        }

        public double YShear
        {
            get => _transform.YShear;
            set => _invalidate(_transform.YShear = value);
        }

        /// <summary>
        /// Returns a Rectangle that is guaranteed to contain the whole view region. 
        /// </summary>
        /// <value>The bounding box.</value>
        public Rectangle BoundingBox
        {
            get
            {
                var pts = ScreenToView(Window.ClientRectangle).Vertices.ToArray();
                double left = pts.Min(p => p.X), right = pts.Max(p => p.X), top = pts.Min(p => p.Y), bottom = pts.Max(p => p.Y);
                return new Rectangle(left, top, right - left, bottom - top);
            }
        }

		public void Bind()
		{
			Current = this;
			LoadMatrixToProgram();
		}

        public Point ScreenToView(Point point) => GLToViewMatrix * ViewMatrix * point;
        public Line ScreenToView(Line line) => new Line(ScreenToView(line.Origin), ScreenToView(line.Destination));
        public Triangle ScreenToView(Triangle tri) => new Triangle(ScreenToView(tri.V1), ScreenToView(tri.V2), ScreenToView(tri.V3));
        public Polygon ScreenToView(Rectangle rect) => new Polygon(new[] { ScreenToView(rect.TopLeft), ScreenToView(rect.TopRight), ScreenToView(rect.BottomRight), ScreenToView(rect.BottomLeft) });
        public Polygon ScreenToView(Polygon polygon) => new Polygon(polygon.Vertices.Select(ScreenToView));

        public Point ViewToScreen(Point point) => ViewMatrix.Inverse * GLToViewMatrix.Inverse * point;
        public Line ViewToScreen(Line line) => new Line(ViewToScreen(line.Origin), ViewToScreen(line.Destination));
		public Triangle ViewToScreen(Triangle tri) => new Triangle(ViewToScreen(tri.V1), ViewToScreen(tri.V2), ViewToScreen(tri.V3));
		public Polygon ViewToScreen(Rectangle rect) => new Polygon(new[] { ViewToScreen(rect.TopLeft), ViewToScreen(rect.TopRight), ViewToScreen(rect.BottomRight), ScreenToView(rect.BottomLeft) });
		public Polygon ViewToScreen(Polygon polygon) => new Polygon(polygon.Vertices.Select(ViewToScreen));


		public IDisposable Use()
		{
            return UseContext.CreateAt($"{typeof(View).FullName}.{nameof(Use)}",
                                       View.Current,
                                       () => { View.Current = this; LoadMatrixToProgram(); },
                                       view => { View.Current = view; LoadMatrixToProgram(); }
            );
		}


	}
}
