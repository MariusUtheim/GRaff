using System;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace GameMaker
{
	public class Surface
	{
		private static readonly IntPtr _1p = new IntPtr(Marshal.SizeOf(typeof(Point))), _2p = new IntPtr(2 * Marshal.SizeOf(typeof(Point))), _4p = new IntPtr(4 * Marshal.SizeOf(typeof(Point)));
		private static readonly IntPtr _1c = new IntPtr(Marshal.SizeOf(typeof(Color))), _2c = new IntPtr(2 * Marshal.SizeOf(typeof(Color))), _4c = new IntPtr(4 * Marshal.SizeOf(typeof(Color)));
		private static readonly IntPtr _4 = new IntPtr(4);
		private int _vertexArray;
		private int _vertexBuffer, _colorBuffer, _textureBuffer;

		public Surface(int width, int height)
		{
			var f = sizeof(float);
			_vertexArray = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArray);

			_vertexBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

			_colorBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, false, 0, 0);

			_textureBuffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, _4p, new float[] { 0, 0, 1, 0, 1, 1, 0, 1 }, BufferUsageHint.StaticDraw);
			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
		}


		#region To be implemented

		public void DrawText(Color color, Object text, double x, double y)
		{
			throw new NotImplementedException();
		}


		public void Blit(Surface dest, IntRectangle srcRect, IntRectangle destRect)
		{
			throw new NotImplementedException();
		}

		public int Width
		{
			get { throw new NotImplementedException(); }
		}

		public int Height
		{
			get { throw new NotImplementedException(); }
		}

		public IntVector Size
		{
			get { return new IntVector(Width, Height); }
		}

		#endregion

		public void Clear(Color color)
		{
			GL.ClearColor(color.ToOpenGLColor());
		}

		public Color GetPixel(double x, double y)
		{
			Color c = 0;
			GL.ReadPixels((int)x, (int)(Room.Height - y), 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, ref c);
			return c;
		}

		public void SetPixel(Color color, Point location)
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, _1p, new[] { location }, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, _1c, new[] { color }, BufferUsageHint.StreamDraw);
			GL.DrawArrays(PrimitiveType.Points, 0, 1);
		}

		public void DrawLine(Color col1, Color col2, Point p1, Point p2)
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, _2p, new[] { p1, p2 }, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, _2c, new[] { col1, col2 }, BufferUsageHint.StreamDraw);
			GL.DrawArrays(PrimitiveType.Lines, 0, 2);
		}

		public void DrawRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double w, double h)
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, _4p, new[] { new Point(x, y), new Point(x + w, y), new Point(x + w, y + h), new Point(x, y + h) }, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, _4c, new[] { col1, col2, col3, col4 }, BufferUsageHint.StreamDraw);
			GL.DrawArrays(PrimitiveType.LineLoop, 0, 4);
		}

		public void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double w, double h)
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, _4p, new[] { new Point(x, y), new Point(x + w, y), new Point(x + w, y + h), new Point(x, y + h) }, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, _4c, new[] { col1, col2, col3, col4 }, BufferUsageHint.StreamDraw);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
		}

		public void DrawCircle(Color color, Point center, double radius)
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			int precision = (int)(GMath.Tau * radius);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(precision * 2 * sizeof(float)), Polygon.EnumerateCircle(center, radius, precision).ToArray(), BufferUsageHint.StreamDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(precision * 4), Enumerable.Repeat(color, precision).ToArray(), BufferUsageHint.StreamDraw);

			GL.DrawArrays(PrimitiveType.LineLoop, 0, precision);
		}

		public void FillCircle(Color col1, Color col2, Point center, double radius)
		{
			GL.BindVertexArray(_vertexArray);

			int precision = (int)GMath.Ceiling(radius);
			Point[] vertices = new Point[precision + 2];
			int i = 0;
			vertices[i++] = center;
			foreach (Point p in Polygon.EnumerateCircle(center, radius, precision))
				vertices[i++] = p;
			vertices[i] = new Point(center.X + radius, center.Y);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * 2 * sizeof(float)), vertices, BufferUsageHint.StreamDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			Color[] colors = new Color[precision + 2];
			colors[0] = col1;
			for (int j = 1; j < colors.Length; j++)
				colors[j] = col2;
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(colors.Length * 4), colors, BufferUsageHint.StreamDraw);

			GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertices.Length);
		}

		public void DrawPolygon(Color color, Polygon polygon)
		{
			GL.BindVertexArray(_vertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(polygon.Length * 2 * sizeof(double)), polygon.Vertices.ToArray(), BufferUsageHint.StreamDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(polygon.Length * 4), Enumerable.Repeat(color, polygon.Length).ToArray(), BufferUsageHint.StreamDraw);

			GL.DrawArrays(PrimitiveType.LineLoop, 0, polygon.Length);
		}

		public void DrawSprite(double x, double y, Sprite sprite, int subimage)
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindTexture(TextureTarget.Texture2D, sprite.Texture.Id);
			View.EnableTexture();
			FillRectangle(Color.White, Color.White, Color.White, Color.White, x, y, sprite.Width, sprite.Height);
			View.DisableTexture();
		}

		public void DrawImage(Image image)
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindTexture(TextureTarget.Texture2D, image.CurrentTexture.Id);
			View.EnableTexture();

			Matrix t = image.Transform.GetMatrix();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, _4p, new[] {
					t * new Point(-image.Sprite.XOrigin, -image.Sprite.YOrigin),
					t * new Point(image.Sprite.Width - image.Sprite.XOrigin, -image.Sprite.YOrigin),
					t * new Point(image.Sprite.Width - image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin),
					t * new Point(-image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin)
			}, BufferUsageHint.StreamDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, _4c, new[] { image.Blend, image.Blend, image.Blend, image.Blend }, BufferUsageHint.StreamDraw);

			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			View.DisableTexture();
		}
	}
}
