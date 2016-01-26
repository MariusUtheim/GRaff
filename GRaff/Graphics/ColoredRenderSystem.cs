using System;
using System.Diagnostics.Contracts;
using System.IO;
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
	internal class ColoredRenderSystem : IDisposable
	{
		private readonly int _array;
		private readonly int _vertexBuffer, _colorBuffer;

		public ColoredRenderSystem()
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
		}

		~ColoredRenderSystem()
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
			GL.DeleteVertexArray(_array);
			GL.DeleteBuffer(_vertexBuffer);
			GL.DeleteBuffer(_colorBuffer);
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

		internal void Render(PrimitiveType type, int vertexCount)
		{
			Contract.Requires<ObjectDisposedException>(!IsDisposed);
			GL.BindVertexArray(_array);
			GL.DrawArrays((GLPrimitiveType)type, 0, vertexCount);
		}
	
	}
}