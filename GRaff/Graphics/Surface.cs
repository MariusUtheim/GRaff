using System;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK.Graphics.ES30;
using GLPrimitiveType = OpenTK.Graphics.ES30.PrimitiveType;

namespace GRaff.Graphics
{
	public sealed class Surface
	{
		private static readonly int sizeofPoint = Marshal.SizeOf(typeof(Point)), sizeofColor = Marshal.SizeOf(typeof(Color));
		private static readonly Point[] defaultTexCoords = { new Point(0.0f, 0.0f), new Point(1.0f, 0.0f), new Point(0.0f, 1.0f), new Point(1.0f, 1.0f)};
		private int _vertexArray;
		private int _vertexBuffer, _colorBuffer, _textureBuffer;

		public Surface(int width, int height)
		{
			GL.GenVertexArrays(1, out _vertexArray);
			GL.BindVertexArray(_vertexArray);

			GL.GenBuffers(1, out _vertexBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

			GL.GenBuffers(1, out _colorBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, 0, 0);

			GL.GenBuffers(1, out _textureBuffer);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(8 * sizeof(float)), defaultTexCoords, BufferUsageHint.StreamDraw);
			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
		}


		#region To be implemented


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

		private void Render(Point[] vertices, Color[] colors, PrimitiveType type)
		{
			ShaderProgram.Current = ShaderProgram.DefaultColored;
			GL.BindVertexArray(_vertexArray);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), vertices, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofColor), colors, BufferUsageHint.StreamDraw);
			GL.DrawArrays((GLPrimitiveType)type, 0, vertices.Length);
		}

		private void RenderTextured(Texture texture, Point[] vertices, Color[] colors, PrimitiveType type)
		{
			GL.BindVertexArray(_vertexArray);
			GL.BindTexture(TextureTarget.Texture2D, texture.Id);

			ShaderProgram.Current = ShaderProgram.DefaultTextured;
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), vertices, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofColor), colors, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * sizeofPoint), texture.GetTexCoords(), BufferUsageHint.StreamDraw);
			GL.DrawArrays((GLPrimitiveType)type, 0, vertices.Length);
		}

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
			Render(new[] { location }, new[] { color }, PrimitiveType.Points);
		}

		public void DrawLine(Color col1, Color col2, Point p1, Point p2)
		{
			Render(new[] { p1, p2 }, new[] { col1, col2 }, PrimitiveType.Lines);
		}

		public void DrawTriangle(Color col1, Color col2, Color col3, Point p1, Point p2, Point p3)
		{
			Render(new[] { p1, p2, p3 }, new[] { col1, col2, col3 }, PrimitiveType.LineStrip);
		}

		public void FillTriangle(Color col1, Color col2, Color col3, Point p1, Point p2, Point p3)
		{
			Render(new[] { p1, p2, p3 }, new[] { col1, col2, col3 }, PrimitiveType.Triangles);
		}

		public void DrawRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double w, double h)
		{
			Render(new[] { new Point(x, y), new Point(x + w, y), new Point(x + w, y + h), new Point(x, y + h) },
				   new[] { col1, col2, col3, col4 }, PrimitiveType.LineLoop);
		}

		public void FillRectangle(Color col1, Color col2, Color col3, Color col4, double x, double y, double w, double h)
		{
			Render(new[] { new Point(x, y), new Point(x + w, y), new Point(x, y + h), new Point(x + w, y + h) },
				   new[] { col1, col2, col4, col3 }, PrimitiveType.TriangleStrip);
		}

		public void DrawCircle(Color color, Point center, double radius)
		{
			int precision = (int)(GMath.Tau * radius);
			Render(Polygon.Circle(center, radius).Vertices.ToArray(), Enumerable.Repeat(color, precision).ToArray(), PrimitiveType.LineLoop);
		}

		public void FillCircle(Color col1, Color col2, Point center, double radius)
		{
			int precision = (int)GMath.Ceiling(radius);
			Point[] vertices = new Point[precision + 2];
			int i = 0;
			vertices[i++] = center;
			foreach (Point p in Polygon.Circle(center, radius).Vertices)
				vertices[i++] = p;
			vertices[i] = new Point(center.X + radius, center.Y);

			Color[] colors = new Color[precision + 2];
			colors[0] = col1;
			for (int j = 1; j < colors.Length; j++)
				colors[j] = col2;

			Render(vertices, colors, PrimitiveType.TriangleFan);
		}

		public void DrawPolygon(Color color, Polygon polygon)
		{
			Render(polygon.Vertices.ToArray(), Enumerable.Repeat(color, polygon.Length).ToArray(), PrimitiveType.LineLoop);
		}

		public void DrawSprite(Sprite sprite, int subimage, double x, double y)
		{
			float left = (float)(x - sprite.XOrigin), top = (float)(y - sprite.YOrigin), right = left + sprite.Width, bottom = top + sprite.Height;
			RenderTextured(sprite.SubImage(subimage), new[] { new Point(left, top), new Point(right, top), new Point(left, bottom), new Point(right, bottom) },
				   new[] { Color.White, Color.White, Color.White, Color.White }, PrimitiveType.TriangleStrip);
		}

		public void DrawSprite(Sprite sprite, int subimage, Color blend, AffineMatrix transform)
		{
			Point[] vertices = {
				transform * new Point(-sprite.XOrigin, -sprite.YOrigin),
				transform * new Point(sprite.XOrigin, -sprite.YOrigin),
				transform * new Point(-sprite.XOrigin, sprite.YOrigin),
				transform * new Point(sprite.XOrigin, sprite.YOrigin)
			};

			RenderTextured(sprite.SubImage(subimage), vertices, new[] { blend, blend, blend, blend }, PrimitiveType.TriangleStrip);
		}

		public void DrawImage(Image image)
		{
			AffineMatrix t = image.Transform.GetMatrix();
			RenderTextured(image.CurrentTexture, new[] {
					t * new Point(-image.Sprite.XOrigin, -image.Sprite.YOrigin),
					t * new Point(image.Sprite.Width - image.Sprite.XOrigin, -image.Sprite.YOrigin),
					t * new Point(-image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin),
					t * new Point(image.Sprite.Width - image.Sprite.XOrigin, image.Sprite.Height - image.Sprite.YOrigin),
			}, new[] { image.Blend, image.Blend, image.Blend, image.Blend }, PrimitiveType.TriangleStrip);

		}

		public void DrawText(FontCharacter font, Color color, string text, double x, double y)
		{
			throw new NotImplementedException();
			/*
			GL.BindVertexArray(_vertexArray);
			GL.BindTexture(TextureTarget.Texture2D, font.Texture.Id);

			ShaderProgram.EnableTexture();

			int len = 4 * text.Length;

			Point[] vertices = new Point[len];
			Point[] texCoords = new Point[len];
			double dx;
			double y0 = y, y1 = y + font.Height;
			int v = 0, t = 0;
			for (int i = 0; i < text.Length; i++)
			{
				dx = Font.GetWidth(text[i]);

				vertices[v++] = new Point(x, y0);
				vertices[v++] = new Point(x + dx, y0);
				vertices[v++] = new Point(x + dx, y1);
				vertices[v++] = new Point(x, y1);

				x += dx;
				Font.PushTexCoords(text[i], ref t, ref texCoords);
			}

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(len * sizeofPoint), vertices, BufferUsageHint.StreamDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(len * sizeofColor), Enumerable.Repeat(color, len).ToArray(), BufferUsageHint.StreamDraw);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _textureBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(len * sizeofPoint), texCoords, BufferUsageHint.StreamDraw);

			GL.DrawArrays(GLPrimitiveType.Quads, 0, vertices.Length);

			ShaderProgram.DisableTexture();
			*/
		}
	}
}
