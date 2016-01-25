using System;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK.Graphics.ES30;

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
			private float v1;
			private float v2;
			private float v3;
			private float v4;
			private float v5;
			private float v6;
			private float v7;
			private float v8;

			public Quadrilateral(float v1, float v2, float v3, float v4, float v5, float v6, float v7, float v8) : this()
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
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

			GL.GenBuffers(1, out _colorBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);
			
			GL.GenBuffers(1, out _texCoordBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
		}

		public void SetVertices(UsageHint usage, params PointF[] vertices)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(2 * sizeof(float) * vertices.Length), vertices, (BufferUsageHint)usage);
		}

		public void SetVertices(UsageHint usage, params float[] vertices)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * vertices.Length), vertices, (BufferUsageHint)usage);
		}

		public void SetColors(UsageHint usage, params Color[] colors)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(4 * colors.Length), colors, (BufferUsageHint)usage);
		}

		public void SetTexCoords(UsageHint usage, params PointF[] texCoords)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(2 * sizeof(float) * texCoords.Length), texCoords, (BufferUsageHint)usage);
		}

		public void SetTexCoords(UsageHint usage, params float[] texCoords)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(sizeof(float) * texCoords.Length), texCoords, (BufferUsageHint)usage);
		}

		public void QuadTexCoords(UsageHint usage, int count)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(8 * sizeof(float) * count), Enumerable.Repeat(defaultQuadCoords, count).ToArray(), (BufferUsageHint)usage);
		}

		public void TriangleStripCoords(UsageHint usage, int count)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, _texCoordBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(8 * sizeof(float) * count), Enumerable.Repeat(defaultTriangleStripCoords, count).ToArray(), (BufferUsageHint)usage);
		}

		internal void Render(PrimitiveType type, int vertexCount)
		{
			GL.BindVertexArray(_array);
			GL.DrawArrays((OpenTK.Graphics.ES30.PrimitiveType)type, 0, vertexCount);
		}
	}
}
