using System;
using System.Diagnostics;
using System.Linq;
using GRaff.Graphics;
using GRaff.Graphics.Shaders;
using OpenTK;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff
{
	/// <summary>
	/// Defines which part of the room is being drawn to the screen.
	/// </summary>
	public class View
	{
		private static int FramebufferViewWidth, FramebufferViewHeight;
        private static readonly Triangle GLTriangle = new Triangle((-1, 1), (1, 1), (-1, -1));
        private static Matrix GLToScreenMatrix;

        private Transform _transform;
        private bool _needsValidation = false;

        public View(Matrix viewMatrix)
        {
            Contract.Requires<ArgumentNullException>(viewMatrix != null);
            Contract.Requires<MatrixException>(viewMatrix.Determinant != 0);
            this._transform = new Transform(viewMatrix);
        }

		internal static void InitializeFramebufferSize(int width, int height)
		{
			FramebufferViewWidth = width;
			FramebufferViewHeight = height;
		}

        internal static void UpdateGLToScreenMatrix()
        {
            GLToScreenMatrix = Matrix.Mapping(GLTriangle, new Triangle((0, 0), (Window.Width, 0), (0, Window.Height)));
        }

        /// <summary>
        /// Creates a View that identically maps drawing vertices to pixels in the window.
        /// </summary>
        public static View FullWindow() => Rectangle(Window.ClientRectangle);

       
		/// <summary>
        /// Creates a View that maps from the specified Rectangle to pixels in the window.
        /// That is, the vertex (x, y) is mapped to (0, 0) on the screen, and (x + width, y + height)
        /// is mapped to (Window.Width, Window.Height).
        /// </summary>
        public static View Rectangle(double x, double y, double width, double height) => Rectangle(new Rectangle(x, y, width, height));

		/// <summary>
        /// Creates a View that maps from the specified Rectangle to pixels in the window.
        /// That is, the vertex (rect.Left, rect.Top) is mapped to (0, 0) on the window, and 
		/// (rect.Right, rect.Bottom) is mapped to (Window.Width, Window.Height).
        /// </summary>
        public static View Rectangle(Rectangle rect)
        {
			var m = Matrix.Translation(-(Vector)rect.Center).Scale(2.0 / rect.Width, -2.0 / rect.Height);
            return new View(m);
        }

        /// <summary>
        /// Creates a View that maps from a rectangle centered at the specified location and with the
		/// specified size, to pixels on the screen. For example, in this View, an ellipse drawn at
		/// location with the specified size will tangent each edge of the window.
        /// </summary>
        public static View Centered(Point location, Vector size) => Rectangle(new Rectangle((location - size / 2), size));
        
        /// <summary>
        /// Translates the current view, so that the specified Point, as seen in the current View, 
		/// will be translated to (0, 0) in the new View.
        /// </summary>
        public static View TranslateTo(Point location) => new View(View.Current.ViewMatrix * Matrix.Translation(location));

        /// <summary>
        /// Creates a View that should be used when drawing to Framebuffers.
        /// </summary>
		public static View Framebuffer() => Rectangle(0, FramebufferViewHeight, FramebufferViewWidth, -FramebufferViewHeight);      

        /// <summary>
        /// Gets the current View that is used by drawing functions. 
        /// </summary>
        public static View Current { get; private set; } = new View(new Matrix());

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

        /// <summary>
        /// Gets the Matrix that is used to map drawing vertices to screen pixels.
        /// </summary>
        public Matrix ViewMatrix => _transform.GetMatrix();

        /// <summary>
        /// Gets the Matrix that is used to map drawing vertices to OpenGL coordinates,
		/// which are contained in the rectangle [-1, 1] x [-1, 1]
        /// </summary>
		public Matrix ViewToScreen => GLToScreenMatrix * ViewMatrix;

        /// <summary>
        /// Gets the Matrix that is used to map 
        /// </summary>
        /// <value>The screen to view.</value>
		public Matrix ScreenToView => ViewMatrix.Inverse * GLToScreenMatrix.Inverse;


		void _invalidate(object _)
        {
            _needsValidation = true;
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
        /// Gets a Rectangle that is guaranteed to contain the whole view region. 
        /// </summary>
        /// <value>The bounding box.</value>
        public Rectangle BoundingBox
        {
            get
            {
                var pts = (ScreenToView * Window.ClientRectangle).Vertices.ToArray();
                double left = pts.Min(p => p.X), right = pts.Max(p => p.X), top = pts.Min(p => p.Y), bottom = pts.Max(p => p.Y);
                return new Rectangle(left, top, right - left, bottom - top);
            }
        }

        /// <summary>
        /// Makes this View the current View to be used by drawing functions.
        /// </summary>
		public void Bind()
		{
			Current = this;
			LoadMatrixToProgram();
		}

        /// <summary>
        /// Temporarily makes this View the current View to be used by drawing functions.
		/// This returns a use context that must be disposed when done drawing. When it is
		/// disposed, the previous View is automatically made current. 
        /// </summary>
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
