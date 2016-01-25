using System;
using System.Linq;
using System.Runtime.InteropServices;
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
	internal class TexturedRenderSystem
	{
		private int _array;
		private int _vertexBuffer, _colorBuffer, _texCoordBuffer;

		private static Quadrilateral defaultQuadCoords = new Quadrilateral(0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f);
		private static Quadrilateral defaultTriangleStripCoords = new Quadrilateral(0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f);
		[StructLayout(LayoutKind.Sequential)]
		struct Quadrilateral
		{
			private coord v1;
			private coord v2;
			private coord v3;
			private coord v4;
			private coord v5;
			private coord v6;
			private coord v7;
			private coord v8;

			public Quadrilateral(coord v1, coord v2, coord v3, coord v4, coord v5, coord v6, coord v7, coord v8) : this()
			{
				this.v1 = v1;
				this.v2 = v2;
				this.v3 = v3;
				this.v4 = v4;
				this.v5 = v5;
				this.v6 = v6;
				this.v7 = v7;
				this.v8 = v8;
			}
		}

		public TexturedRenderSystem()
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
			
			GL.GenBuffers(1, out _texCoordBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 2, GraphicsPoint.PointerType, false, 0, 0);
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

		public void SetTexCoords(UsageHint usage, params GraphicsPoint[] texCoords)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(2 * sizeof(coord) * texCoords.Length), texCoords, (BufferUsageHint)usage);
		}

		public void SetTexCoords(UsageHint usage, params coord[] texCoords)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(coord) * texCoords.Length), texCoords, (BufferUsageHint)usage);
		}

		public void QuadTexCoords(UsageHint usage, int count)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(8 * sizeof(coord) * count), Enumerable.Repeat(defaultQuadCoords, count).ToArray(), (BufferUsageHint)usage);
		}

		public void TriangleStripCoords(UsageHint usage, int count)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(8 * sizeof(coord) * count), Enumerable.Repeat(defaultTriangleStripCoords, count).ToArray(), (BufferUsageHint)usage);
		}

		internal void Render(PrimitiveType type, int vertexCount)
		{
			GL.BindVertexArray(_array);
			GL.DrawArrays((GLPrimitiveType)type, 0, vertexCount);
		}
	}
}
