using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;
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
	internal class TexturedRenderSystem : IDisposable
	{
		private readonly int _array;
		private readonly int _vertexBuffer, _colorBuffer, _texCoordBuffer;

		private static Quadrilateral defaultQuadCoords = new Quadrilateral(0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f);
		private static Quadrilateral defaultTriangleStripCoords = new Quadrilateral(0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f);
		[StructLayout(LayoutKind.Sequential)]
		struct Quadrilateral
		{
			public readonly coord V1;
			public readonly coord V2;
			public readonly coord V3;
			public readonly coord V4;
			public readonly coord V5;
			public readonly coord V6;
			public readonly coord V7;
			public readonly coord V8;

			public Quadrilateral(coord v1, coord v2, coord v3, coord v4, coord v5, coord v6, coord v7, coord v8) : this()
			{
				this.V1 = v1;
				this.V2 = v2;
				this.V3 = v3;
				this.V4 = v4;
				this.V5 = v5;
				this.V6 = v6;
				this.V7 = v7;
				this.V8 = v8;
			}
		}

		public TexturedRenderSystem()
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

		~TexturedRenderSystem()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			if (Giraffe.IsRunning)
			{
				GL.DeleteVertexArray(_array);
				GL.DeleteBuffer(_vertexBuffer);
				GL.DeleteBuffer(_colorBuffer);
				GL.DeleteBuffer(_texCoordBuffer);
			}
			IsDisposed = true;
		}

		public bool IsDisposed { get; private set; }

		public void SetVertices(UsageHint usage, params GraphicsPoint[] vertices)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentNullException>(vertices != null);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(2 * sizeof(coord) * vertices.Length), vertices, (BufferUsageHint)usage);
		}

		public void SetVertices(UsageHint usage, params coord[] vertices)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentNullException>(vertices != null);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(coord) * vertices.Length), vertices, (BufferUsageHint)usage);
		}

		public void SetColors(UsageHint usage, params Color[] colors)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentNullException>(colors != null);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(4 * colors.Length), colors, (BufferUsageHint)usage);
		}

		public void SetTexCoords(UsageHint usage, params GraphicsPoint[] texCoords)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			Contract.Requires<ArgumentNullException>(texCoords != null);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(2 * sizeof(coord) * texCoords.Length), texCoords, (BufferUsageHint)usage);
		}

		public void SetTexCoords(UsageHint usage, params coord[] texCoords)
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

		internal void Render(PrimitiveType type, int vertexCount)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			GL.BindVertexArray(_array);
			GL.DrawArrays((GLPrimitiveType)type, 0, vertexCount);
		}
	}
}
