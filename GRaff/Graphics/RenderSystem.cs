using System;
using System.Linq;
using System.Diagnostics.Contracts;
using GRaff.Synchronization;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
using GLPrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;
using coord = System.Double;
#else
using OpenTK.Graphics.ES30;
using GLPrimitiveType = OpenTK.Graphics.ES30.PrimitiveType;
using coord = System.Single;
#endif

namespace GRaff.Graphics
{
	public class RenderSystem : IDisposable
	{
		private readonly int _array;
		private readonly int _vertexBuffer, _colorBuffer, _texCoordBuffer;
        private int _vertexCount;

		private static Quadrilateral defaultQuadCoords = new Quadrilateral(0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f);
		private static Quadrilateral defaultTriangleStripCoords = new Quadrilateral(0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f);

		public RenderSystem()
		{
			_array = GL.GenVertexArray();
			GL.BindVertexArray(_array);

			_vertexBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, GraphicsPoint.PointerType, false, 0, 0);

			_colorBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);

			_texCoordBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 2, GraphicsPoint.PointerType, false, 0, 0);
		}

		~RenderSystem()
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
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Async.Run(() =>
			{
			    if (Giraffe.IsRunning)
				{
					GL.DeleteVertexArray(_array);
					GL.DeleteBuffer(_vertexBuffer);
					GL.DeleteBuffer(_colorBuffer);
					GL.DeleteBuffer(_texCoordBuffer);
				}
			});
			IsDisposed = true;
		}

		public bool IsDisposed { get; private set; }


        public void SetVertices(params GraphicsPoint[] vertices) => SetVertices(UsageHint.StreamDraw, vertices);
		public void SetVertices(UsageHint usage, params GraphicsPoint[] vertices)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentNullException>(vertices != null);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(2 * sizeof(coord) * vertices.Length), vertices, (BufferUsageHint)usage);
            _vertexCount = vertices.Length;
		}

        public void SetVertices(params coord[] vertices) => SetVertices(UsageHint.StreamDraw, vertices);
		public void SetVertices(UsageHint usage, params coord[] vertices)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentNullException>(vertices != null);
            Contract.Requires<ArgumentException>(vertices.Length % 2 == 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(coord) * vertices.Length), vertices, (BufferUsageHint)usage);
            _vertexCount = vertices.Length / 2;
		}


        public void SetColors(Color[] colors) => SetColors(UsageHint.StreamDraw, colors);
		public void SetColors(UsageHint usage, Color[] colors)
		{
            //TODO// Select colors for each primitive? (e.g. when drawing PrimitiveType.Triangles, allow colors.Length == vertices.Length / 3)
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentNullException>(colors != null);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
            GL.EnableVertexAttribArray(1);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(4 * colors.Length), colors, (BufferUsageHint)usage);
		}

        public void SetColor(Color color) => SetColor(UsageHint.StreamDraw, color);
        public void SetColor(UsageHint usage, Color color)
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
            GL.DisableVertexAttribArray(1);
            GL.VertexAttrib4N(1, color.R, color.G, color.B, color.A);
        }

        public void SetTexCoords(GraphicsPoint[] texCoords) => SetTexCoords(UsageHint.StreamDraw, texCoords);
		public void SetTexCoords(UsageHint usage, params GraphicsPoint[] texCoords)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentNullException>(texCoords != null);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(2 * sizeof(coord) * texCoords.Length), texCoords, (BufferUsageHint)usage);
		}

		public void SetTexCoords(UsageHint usage, coord[] texCoords)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentNullException>(texCoords != null);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(coord) * texCoords.Length), texCoords, (BufferUsageHint)usage);
		}

		public void QuadTexCoords(UsageHint usage, int count)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(8 * sizeof(coord) * count), Enumerable.Repeat(defaultQuadCoords, count).ToArray(), (BufferUsageHint)usage);
		}

		public void TriangleStripCoords(UsageHint usage, int count)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentOutOfRangeException>(count >= 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(8 * sizeof(coord) * count), Enumerable.Repeat(defaultTriangleStripCoords, count).ToArray(), (BufferUsageHint)usage);
		}

        public void Render(PrimitiveType type)
        {
            Contract.Requires<ObjectDisposedException>(!IsDisposed);

            ShaderProgram.Current.SetUniform("GRaff_IsTextured", false);

            GL.BindVertexArray(_array);
            GL.DisableVertexAttribArray(2);

            GL.DrawArrays((GLPrimitiveType)type, 0, _vertexCount);
        }

		public void Render(TextureBuffer buffer, PrimitiveType type)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);

            ShaderProgram.Current.SetUniform("GRaff_IsTextured", true);

            GL.BindVertexArray(_array);
            GL.EnableVertexAttribArray(2);
            buffer.Bind();
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
            GL.DrawArrays((GLPrimitiveType)type, 0, _vertexCount);
		}
	}
}
