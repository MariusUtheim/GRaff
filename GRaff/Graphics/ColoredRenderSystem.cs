using System;
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
#warning IDisposable
	internal class ColoredRenderSystem
	{
		private int _array;
		private int _vertexBuffer, _colorBuffer;

		public ColoredRenderSystem()
		{
			GL.GenVertexArrays(1, out _array);
			GL.BindVertexArray(_array);

			GL.GenBuffers(1, out _vertexBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, GraphicsPoint.PointerType, false, 0, 0);

			GL.GenBuffers(1, out _colorBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);

		}

		public void SetVertices(UsageHint usage, params GraphicsPoint[] vertices)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(2 * sizeof(coord) * vertices.Length), vertices, (BufferUsageHint)usage);
		}

		public void SetVertices(UsageHint usage, params coord[] vertices)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(coord) * vertices.Length), vertices, (BufferUsageHint)usage);
		}

		public void SetColors(UsageHint usage, params Color[] colors)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(4 * colors.Length), colors, (BufferUsageHint)usage);
		}

		internal void Render(PrimitiveType type, int vertexCount)
		{
			GL.BindVertexArray(_array);
			GL.DrawArrays((GLPrimitiveType)type, 0, vertexCount);
		}
	}
}